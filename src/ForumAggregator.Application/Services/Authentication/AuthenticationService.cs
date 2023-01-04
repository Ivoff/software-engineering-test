namespace ForumAggregator.Application.Services;

using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IHttpContextAccessor _httpContext;

    public AuthenticationService(IHttpContextAccessor http_context)
    {
        _httpContext = http_context;
    }
    
    public async Task GenerateCookie(Guid userId, string userName)
    {
        ICollection<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };

        ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await _httpContext.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
    }

    public async Task SignOut()
    {
        await _httpContext.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}