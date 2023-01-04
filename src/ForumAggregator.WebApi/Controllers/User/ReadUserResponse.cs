namespace ForumAggregator.WebApi.Controllers.User;

using System;

public record ReadUserReponse(
    Guid Id,
    string Name,
    string NewEmail,
    bool Deleted
);