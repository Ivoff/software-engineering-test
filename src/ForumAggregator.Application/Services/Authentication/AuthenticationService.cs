namespace ForumAggregator.Application.Services;

using System;

public class AuthenticationService : IAuthenticationService
{
    public string GenerateCookie(Guid userId, string userName)
    {
        throw new NotImplementedException();
    }

    public bool AuthenticateCookie(string token)
    {
        throw new NotImplementedException();
    }
}