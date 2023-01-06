namespace ForumAggregator.WebApi.Controllers.Forum;

using System;
using System.Collections.Generic;

public record AddModeratorRequest(
    Guid ForumId,
    ICollection<Moderator> Moderators
);