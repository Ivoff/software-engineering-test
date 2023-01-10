namespace ForumAggregator.WebApi.Controllers.Forum;

using System;

public record ReadForumResponse(
    Guid id,
    Guid OwnerId,
    string OwnerName,
    string Name,
    string Description,
    bool Deleted
);