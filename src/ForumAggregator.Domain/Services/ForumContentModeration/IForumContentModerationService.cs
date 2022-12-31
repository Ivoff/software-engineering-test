namespace ForumAggregator.Domain.Services.ForumContentModerationService;

using System;
using ForumAggregator.Domain.Shared.Interfaces;
using ForumAggregator.Domain.CommentRegistry;
using ForumAggregator.Domain.ForumRegistry;
using ForumAggregator.Domain.PostRegistry;

public interface IForumContentModerationService: IDomainService
{
    ForumContentModerationResult RemoveComment (Comment comment, Post post, Forum forum, Guid actorUserId);

    ForumContentModerationResult RemovePost (Post post, Forum forum, Guid actorUserId);
}