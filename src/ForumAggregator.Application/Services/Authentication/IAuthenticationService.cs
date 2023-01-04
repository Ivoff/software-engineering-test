namespace ForumAggregator.Application.Services;

public interface IAuthenticationService
{
    public Task GenerateCookie(Guid userId, string userName);

    public Task SignOut();    
}