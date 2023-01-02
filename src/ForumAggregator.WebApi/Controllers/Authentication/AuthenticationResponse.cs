namespace ForumAggregator.WebApi.Controllers.Authentication;

using System;

public record AuthenticationResponse(
    Guid Id,
    string Name,
    string Token
);