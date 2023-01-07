namespace ForumAggregator.WebApi.Controllers.Forum;

using System;
using System.Collections.Generic;

public record BlackListed(
    Guid UserId,
    bool? CanComment,
    bool? CanPost
);

public record Moderator(
    Guid UserId,
    ICollection<int>? Authorities
);

public record CreateForumRequest(
    string Name,
    string Description,
    ICollection<Moderator> Moderators,
    ICollection<BlackListed> BlackList
);