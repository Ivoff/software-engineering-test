namespace ForumAggregator.Domain.Services;

using System;
using ForumAggregator.Domain.CommentRegistry;
using ForumAggregator.Domain.ForumRegistry;
using ForumAggregator.Domain.PostRegistry;
using ForumAggregator.Domain.Shared.Interfaces;

public class ForumContentModerationService: IForumContentModerationService, IDomainService
{
    public ForumContentModerationResult RemovePost(in Post post, in Forum forum, Guid actorUserId)
    {
        // Deletion Check.
        
        if (post.Deleted)
            return DeletedResult(post);

        if (forum.Deleted)
            return DeletedResult(forum);

        // End of Deletion Check.

        if (post.ForumId == forum.Id)
        {
            Moderator? aux = forum.GetModeratorByUserId(actorUserId);
            if (aux == null)
                return new ForumContentModerationResult() { Value = false, Result = "Actor User is not a Moderator." };
            
            Moderator mod = (Moderator) aux;
            if (mod.CheckForAuthority(EAuthority.DeletePost))
            {
                IDomainResult<bool> result = post.Delete();
                return new ForumContentModerationResult() { Value = result.Value, Result = result.Result };
            }

            return new ForumContentModerationResult() { Value = false, Result = "Actor User has no Authority to remove the Post." };
        }

        return new ForumContentModerationResult() { Value = false, Result = "Post does not belong to Forum." };
    }

    public ForumContentModerationResult RemoveComment(in Comment comment, in Post post, in Forum forum, Guid actorUserId)
    {
        // Deletion Check.

        if (post.Deleted)
            return DeletedResult(post);

        if (forum.Deleted)
            return DeletedResult(forum);

        if (comment.Deleted)
            return DeletedResult(comment);

        // End of Deletion Check.

        if (comment.PostId != post.Id)
            return new ForumContentModerationResult() { Value = false, Result = "Comment does not belong to Post." };
        
        if (post.ForumId != forum.Id)
            return new ForumContentModerationResult() { Value = false, Result = "Post from the Comment does not belong to Forum." };

        Moderator? aux = forum.GetModeratorByUserId(actorUserId);

        if (aux == null)
            return new ForumContentModerationResult() { Value = false, Result = "Actor User is not a Moderator." };
        
        Moderator mod = (Moderator) aux;

        if (mod.CheckForAuthority(EAuthority.DeleteComment))
        {
            IDomainResult<bool> result = comment.Delete();
            return new ForumContentModerationResult()
            {
                Value = result.Value,
                Result = result.Result
            };
        }

        return new ForumContentModerationResult() { Value = false, Result = "Actor User has no Authority to remove the Comment." };
    }

    private ForumContentModerationResult DeletedResult(Object obj)
    {        
        return new ForumContentModerationResult()
        {
            Value = false,
            Result = $"{obj.GetType().Name} has already been deleted"
        };
    }
}