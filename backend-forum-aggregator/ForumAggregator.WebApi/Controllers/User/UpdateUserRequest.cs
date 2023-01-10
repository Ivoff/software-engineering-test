namespace ForumAggregator.WebApi.Controllers.User;

using System;

public record UpdateUserRequest(
    Guid Id,
    string NewName,
    string NewPassword,
    string NewEmail
);