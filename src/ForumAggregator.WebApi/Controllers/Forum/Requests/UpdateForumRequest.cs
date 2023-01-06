namespace ForumAggregator.WebApi.Controllers.Forum;

using System;

public record UpdateForumRequest(
    Guid forumId,
    string Name,
    string Description
);