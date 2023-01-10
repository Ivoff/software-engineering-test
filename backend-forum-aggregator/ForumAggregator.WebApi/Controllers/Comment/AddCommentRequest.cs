namespace ForumAggregator.WebApi.Controllers.Comment;

using System;

public record AddCommentRequest(
    Guid PostId,
    Guid? ParentId,
    Guid AuthorId,
    string Content
);