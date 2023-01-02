namespace ForumAggregator.Application.Services.Authentication;

using System;

public class AuthenticationService : IAuthenticationService
{
    public string GenerateToken(Guid userId, string userName)
    {
        throw new NotImplementedException();
    }

    public bool AuthenticateToken(string token)
    {
        throw new NotImplementedException();
    }
}