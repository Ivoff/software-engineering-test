namespace ForumAggregator.WebApi.Controllers.Post;

using System;

public record ReadPostResponse(
    Guid Id,
    Guid ForumId,
    string ForumName,
    Guid AuthorId,
    string AuthorName,
    string Title,
    string Content,
    bool Deleted
);