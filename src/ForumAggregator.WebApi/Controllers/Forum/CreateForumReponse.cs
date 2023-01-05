namespace ForumAggregator.WebApi.Controllers.Forum;

using System;

public record CreateForumResponse(
    Guid id,
    string Name
);