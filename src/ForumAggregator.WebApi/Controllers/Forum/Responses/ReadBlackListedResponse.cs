namespace ForumAggregator.WebApi.Controllers.Forum;

public record ReadBlackListedResponse(
    Guid Id,
    Guid ForumId,
    Guid UserId,
    bool CanComment,
    bool CanPost,
    bool Deleted
);