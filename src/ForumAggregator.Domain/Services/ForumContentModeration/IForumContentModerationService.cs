namespace ForumAggregator.Domain.Services;

using System;
using ForumAggregator.Domain.Shared.Interfaces;
using ForumAggregator.Domain.CommentRegistry;
using ForumAggregator.Domain.ForumRegistry;
using ForumAggregator.Domain.PostRegistry;

public interface IForumContentModerationService: IDomainService
{
    ForumContentModerationResult RemoveComment (in Comment comment, in Post post, in Forum forum, Guid actorUserId);

    ForumContentModerationResult RemovePost (in Post post, in Forum forum, Guid actorUserId);
}