namespace ForumAggregator.WebApi.Controllers.Authentication;

public record RegisterRequest(    
    string Name,
    string Email,
    string Password
);