namespace ForumAggregator.WebApi.Controllers.Forum;

using System;

public record BlackListedRequest(
    Guid forumId,
    ICollection<BlackListed> blackListedUsers
);