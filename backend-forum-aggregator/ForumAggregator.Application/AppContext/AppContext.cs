namespace ForumAggregator.Application;

using System;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class AppContext: IAppContext
{
    private Guid _userId;
    public Guid UserId {get => _userId;}

    private readonly IHttpContextAccessor _httpContext;

    public AppContext(IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;
        
        string aux = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        _userId =  string.IsNullOrWhiteSpace(aux) ? Guid.Empty : Guid.Parse(aux);
    }
}