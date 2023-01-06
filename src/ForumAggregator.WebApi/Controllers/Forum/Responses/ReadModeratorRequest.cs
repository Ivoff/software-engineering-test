namespace ForumAggregator.WebApi.Controllers.Forum;

using System;

public record AuthorityResponse(
    string Authority,
    int Code
);

public record ReadModeratorResponse(
    Guid Id,
    Guid ForumId,
    Guid UserId,
    ICollection<AuthorityResponse> Authorities
);