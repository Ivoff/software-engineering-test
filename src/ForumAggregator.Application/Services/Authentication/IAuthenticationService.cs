namespace ForumAggregator.Application.Services;

public interface IAuthenticationService
{
    public string GenerateCookie(Guid userId, string userName);

    public bool AuthenticateCookie(string token);
}