namespace ForumAggregator.UnitTests.ForumContentModerationTests;

using System;
using System.Linq;
using ForumAggregator.Domain.ForumRegistry;
using ForumAggregator.Domain.CommentRegistry;
using ForumAggregator.Domain.PostRegistry;
using ForumAggregator.Domain.Shared.Interfaces;
using ForumAggregator.Domain.Services.ForumContentModerationService;

public class ForumContentModerationTests
{
    [Fact]
    public void ModeratorShouldRemoveComment()
    {
        // Setting Entities and Aggregates

        Guid forumOwnerUserId = Guid.NewGuid();
        Guid postAuthorUserId = Guid.NewGuid();
        Guid commentAuthorUserId = Guid.NewGuid();

        Moderator moderator = Moderator.Load(
            Guid.NewGuid(),
            Guid.NewGuid(),
            false,
            new List<EAuthority>{
                EAuthority.DeleteComment
            }
        );

        Forum forum = Forum.Load(
            Guid.NewGuid(),
            forumOwnerUserId,
            "Test Forum",
            "This is the Test Forum, welcome.",
            false,
            ModeratorCollection.Load(
                new List<Moderator>{
                    Moderator.Load(Guid.NewGuid(), forumOwnerUserId, false, Enum.GetValues<EAuthority>()),
                    moderator
                }
            ),
            new BlackListedCollection()
        );

        bool postAuthorCannotPost = !forum.GetBlackListedByUserId(postAuthorUserId)?.CanPost ?? false;
        PostAuthor postAuthor = new PostAuthor(postAuthorUserId, postAuthorCannotPost);
        Post post = Post.Load(
            Guid.NewGuid(),
            forum.Id,
            "This is a Test Post, be kind",
            "Lorem ipsum",
            false,
            postAuthor
        );

        bool commentAuthorCannotComment = !forum.GetBlackListedByUserId(commentAuthorUserId)?.CanComment ?? false;
        CommentAuthor commentAuthor = new CommentAuthor(commentAuthorUserId, commentAuthorCannotComment);
        Comment comment = Comment.Load(
            Guid.NewGuid(),
            post.Id,
            null,
            "this is a test comment guys, pls be kind",
            commentAuthor,
            false
        );

        // Moderator Remove Comment Actual Test

        ForumContentModerationService forumContentModerationService = new ForumContentModerationService();
        IDomainResult<bool> result = forumContentModerationService.RemoveComment(comment, post, forum, moderator.UserId);

        Assert.True(result.Value, result.Result);
        Assert.True(comment.Deleted, "Comment should have been marked as Deleted.");
    }

    [Fact]
    public void ModeratorShouldNotRemoveComment()
    {
        // Setting Entities and Aggregates

        Guid forumOwnerUserId = Guid.NewGuid();
        Guid postAuthorUserId = Guid.NewGuid();
        Guid commentAuthorUserId = Guid.NewGuid();

        Moderator moderator = Moderator.Load(
            Guid.NewGuid(),
            Guid.NewGuid(),
            false,
            new List<EAuthority>{
                EAuthority.AlterForumDescription,
                EAuthority.AlterForumName,
                EAuthority.AddModerator
            }
        );

        Forum forum = Forum.Load(
            Guid.NewGuid(),
            forumOwnerUserId,
            "Test Forum",
            "This is the Test Forum, welcome.",
            false,
            ModeratorCollection.Load(
                new List<Moderator>{
                    Moderator.Load(Guid.NewGuid(), forumOwnerUserId, false, Enum.GetValues<EAuthority>()),
                    moderator
                }
            ),
            new BlackListedCollection()
        );

        bool postAuthorCannotPost = !forum.GetBlackListedByUserId(postAuthorUserId)?.CanPost ?? false;
        PostAuthor postAuthor = new PostAuthor(postAuthorUserId, postAuthorCannotPost);
        Post post = Post.Load(
            Guid.NewGuid(),
            forum.Id,
            "This is a Test Post, be kind",
            "Lorem ipsum",
            false,
            postAuthor
        );

        bool commentAuthorCannotComment = !forum.GetBlackListedByUserId(commentAuthorUserId)?.CanComment ?? false;
        CommentAuthor commentAuthor = new CommentAuthor(commentAuthorUserId, commentAuthorCannotComment);
        Comment comment = Comment.Load(
            Guid.NewGuid(),
            post.Id,
            null,
            "this is a test comment guys, pls be kind",
            commentAuthor,
            false
        );

        // Moderator Remove Comment Actual Test

        ForumContentModerationService forumContentModerationService = new ForumContentModerationService();
        IDomainResult<bool> result = forumContentModerationService.RemoveComment(comment, post, forum, moderator.UserId);

        Assert.False(result.Value, result.Result);
        Assert.False(comment.Deleted, "Comment should not have been marked as Deleted.");
    }

    [Fact]
    public void ModeratorTryToDeleteCommentFromDeletedPost()
    {
        // Setting Entities and Aggregates

        Guid forumOwnerUserId = Guid.NewGuid();
        Guid postAuthorUserId = Guid.NewGuid();
        Guid commentAuthorUserId = Guid.NewGuid();

        Moderator moderator = Moderator.Load(
            Guid.NewGuid(),
            Guid.NewGuid(),
            false,
            new List<EAuthority>{
                EAuthority.DeleteComment
            }
        );

        Forum forum = Forum.Load(
            Guid.NewGuid(),
            forumOwnerUserId,
            "Test Forum",
            "This is the Test Forum, welcome.",
            false,
            ModeratorCollection.Load(
                new List<Moderator>{
                    Moderator.Load(Guid.NewGuid(), forumOwnerUserId, false, Enum.GetValues<EAuthority>()),
                    moderator
                }
            ),
            new BlackListedCollection()
        );

        bool postAuthorCannotPost = !forum.GetBlackListedByUserId(postAuthorUserId)?.CanPost ?? false;
        PostAuthor postAuthor = new PostAuthor(postAuthorUserId, postAuthorCannotPost);
        Post post = Post.Load(
            Guid.NewGuid(),
            forum.Id,
            "This is a Test Post, be kind",
            "Lorem ipsum",
            false,
            postAuthor
        );
        post.Delete();

        bool commentAuthorCannotComment = !forum.GetBlackListedByUserId(commentAuthorUserId)?.CanComment ?? false;
        CommentAuthor commentAuthor = new CommentAuthor(commentAuthorUserId, commentAuthorCannotComment);
        Comment comment = Comment.Load(
            Guid.NewGuid(),
            post.Id,
            null,
            "this is a test comment guys, pls be kind",
            commentAuthor,
            false
        );

        // Moderator Remove Comment Actual Test

        ForumContentModerationService forumContentModerationService = new ForumContentModerationService();
        IDomainResult<bool> result = forumContentModerationService.RemoveComment(comment, post, forum, moderator.UserId);

        Assert.False(result.Value, result.Result);
        Assert.False(comment.Deleted, "Comment should not have been marked as Deleted.");
    }

    [Fact]
    public void ModeratorShouldRemovePost()
    {
        // Setting Entities and Aggregates

        Guid forumOwnerUserId = Guid.NewGuid();
        Guid postAuthorUserId = Guid.NewGuid();
        Guid commentAuthorUserId = Guid.NewGuid();

        Moderator moderator = Moderator.Load(
            Guid.NewGuid(),
            Guid.NewGuid(),
            false,
            new List<EAuthority>{
                EAuthority.DeletePost
            }
        );

        Forum forum = Forum.Load(
            Guid.NewGuid(),
            forumOwnerUserId,
            "Test Forum",
            "This is the Test Forum, welcome.",
            false,
            ModeratorCollection.Load(
                new List<Moderator>{
                    Moderator.Load(Guid.NewGuid(), forumOwnerUserId, false, Enum.GetValues<EAuthority>()),
                    moderator
                }
            ),
            new BlackListedCollection()
        );

        bool postAuthorCannotPost = !forum.GetBlackListedByUserId(postAuthorUserId)?.CanPost ?? false;
        PostAuthor postAuthor = new PostAuthor(postAuthorUserId, postAuthorCannotPost);
        Post post = Post.Load(
            Guid.NewGuid(),
            forum.Id,
            "This is a Test Post, be kind",
            "Lorem ipsum",
            false,
            postAuthor
        );

        // Moderator Remove Comment Actual Test

        ForumContentModerationService forumContentModerationService = new ForumContentModerationService();
        IDomainResult<bool> result = forumContentModerationService.RemovePost(post, forum, moderator.UserId);

        Assert.True(result.Value, result.Result);
        Assert.True(post.Deleted, "Comment should have been marked as Deleted.");
    }

    [Fact]
    public void ModeratorShouldNotRemovePost()
    {
        // Setting Entities and Aggregates

        Guid forumOwnerUserId = Guid.NewGuid();
        Guid postAuthorUserId = Guid.NewGuid();
        Guid commentAuthorUserId = Guid.NewGuid();

        Moderator moderator = Moderator.Load(
            Guid.NewGuid(),
            Guid.NewGuid(),
            false,
            new List<EAuthority>{
                EAuthority.AlterForumDescription,
                EAuthority.AlterForumName
            }
        );

        Forum forum = Forum.Load(
            Guid.NewGuid(),
            forumOwnerUserId,
            "Test Forum",
            "This is the Test Forum, welcome.",
            false,
            ModeratorCollection.Load(
                new List<Moderator>{
                    Moderator.Load(Guid.NewGuid(), forumOwnerUserId, false, Enum.GetValues<EAuthority>()),
                    moderator
                }
            ),
            new BlackListedCollection()
        );

        bool postAuthorCannotPost = !forum.GetBlackListedByUserId(postAuthorUserId)?.CanPost ?? false;
        PostAuthor postAuthor = new PostAuthor(postAuthorUserId, postAuthorCannotPost);
        Post post = Post.Load(
            Guid.NewGuid(),
            forum.Id,
            "This is a Test Post, be kind",
            "Lorem ipsum",
            false,
            postAuthor
        );

        // Moderator Remove Comment Actual Test

        ForumContentModerationService forumContentModerationService = new ForumContentModerationService();
        IDomainResult<bool> result = forumContentModerationService.RemovePost(post, forum, moderator.UserId);

        Assert.False(result.Value, result.Result);
        Assert.False(post.Deleted, "Comment should not have been marked as Deleted.");
    }

    [Fact]
    public void ModeratorTryToDeletePostFromDeletedForum()
    {
        // Setting Entities and Aggregates

        Guid forumOwnerUserId = Guid.NewGuid();
        Guid postAuthorUserId = Guid.NewGuid();
        Guid commentAuthorUserId = Guid.NewGuid();

        Moderator moderator = Moderator.Load(
            Guid.NewGuid(),
            Guid.NewGuid(),
            false,
            new List<EAuthority>{
                EAuthority.DeletePost
            }
        );

        Forum forum = Forum.Load(
            Guid.NewGuid(),
            forumOwnerUserId,
            "Test Forum",
            "This is the Test Forum, welcome.",
            false,
            ModeratorCollection.Load(
                new List<Moderator>{
                    Moderator.Load(Guid.NewGuid(), forumOwnerUserId, false, Enum.GetValues<EAuthority>()),
                    moderator
                }
            ),
            new BlackListedCollection()
        );
        forum.Remove(forumOwnerUserId);

        bool postAuthorCannotPost = !forum.GetBlackListedByUserId(postAuthorUserId)?.CanPost ?? false;
        PostAuthor postAuthor = new PostAuthor(postAuthorUserId, postAuthorCannotPost);
        Post post = Post.Load(
            Guid.NewGuid(),
            forum.Id,
            "This is a Test Post, be kind",
            "Lorem ipsum",
            false,
            postAuthor
        );

        // Moderator Remove Comment Actual Test

        ForumContentModerationService forumContentModerationService = new ForumContentModerationService();
        IDomainResult<bool> result = forumContentModerationService.RemovePost(post, forum, moderator.UserId);

        Assert.False(result.Value, result.Result);
        Assert.False(post.Deleted, "Comment should not have been marked as Deleted.");
    }
}