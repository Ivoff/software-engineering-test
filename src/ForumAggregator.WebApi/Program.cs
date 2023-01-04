using ForumAggregator.Application.DependencyInjection;
using ForumAggregator.Infraestructure.DependencyInjection;
using ForumAggregator.Infraestructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).CustomAddCookie();
builder.Services.AddInfraestructure();
builder.Services.AddApplication();
builder.Services.AddControllers();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    db.Database.Migrate();
}

app.UseCookiePolicy(new CookiePolicyOptions{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.SameAsRequest
});

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();