namespace ForumAggregator.WebApi.Controllers.Authentication;

public record LoginRequest(
    string Email,
    string Password
);