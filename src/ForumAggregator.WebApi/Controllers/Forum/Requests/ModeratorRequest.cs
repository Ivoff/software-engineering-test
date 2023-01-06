namespace ForumAggregator.WebApi.Controllers.Forum;

using System;
using System.Collections.Generic;

public record ModeratorRequest(
    Guid ForumId,
    ICollection<Moderator> Moderators
);