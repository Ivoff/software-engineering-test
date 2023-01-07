namespace ForumAggregator.WebApi.Controllers.Post;

using System;

public record PostRequest(
    Guid ForumId,
    string Title,
    string Content
);