namespace ForumAggregator.WebApi.Controllers.Post;

using System;

public record UpdatePostRequest(
    Guid PostId,
    string Title,
    string Content
);