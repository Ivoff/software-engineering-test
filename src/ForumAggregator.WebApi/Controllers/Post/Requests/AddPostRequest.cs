namespace ForumAggregator.WebApi.Controllers.Post;

using System;

public record AddPostRequest(
    Guid ForumId,
    string Title,
    string Content
);