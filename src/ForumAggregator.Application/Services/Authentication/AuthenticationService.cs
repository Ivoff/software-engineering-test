namespace ForumAggregator.Application.Services;

using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IHttpContextAccessor _http_context;

    public AuthenticationService(IHttpContextAccessor http_context)
    {
        _http_context = http_context;
    }
    
    public async Task GenerateCookie(Guid userId, string userName)
    {
        ICollection<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };

        ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await _http_context.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
    }

    public async Task SignOut()
    {
        await _http_context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}