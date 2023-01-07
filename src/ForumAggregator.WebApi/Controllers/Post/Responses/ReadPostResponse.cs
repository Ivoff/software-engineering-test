namespace ForumAggregator.WebApi.Controllers.Post;

using System;

public record ReadPostResponse(
    Guid Id,
    Guid ForumId,
    Guid AuthorId,
    string Title,
    string Content,
    bool Deleted
);