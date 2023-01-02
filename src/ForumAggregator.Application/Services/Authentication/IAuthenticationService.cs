namespace ForumAggregator.Application.Services.Authentication;

public interface IAuthenticationService
{
    public string GenerateToken(Guid userId, string userName);

    public bool AuthenticateToken(string token);
}