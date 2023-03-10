using ForumAggregator.Application.DependencyInjection;
using ForumAggregator.Infraestructure.DependencyInjection;
using ForumAggregator.Infraestructure.DbContext;
using ForumAggregator.WebApi.Controllers.Authentication;
using ForumAggregator.WebApi.Controllers.User;
using ForumAggregator.WebApi.Controllers.Forum;
using ForumAggregator.WebApi.Controllers.Post;
using ForumAggregator.Domain.DependencyInjection;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).CustomAddCookie();
builder.Services.AddInfraestructure();
builder.Services.AddApplication();
builder.Services.AddControllers();
// Need to reference domain so that Domain Services can be registered
// Just like the need to reference Infraestructure so that 
// Infraestructure Services can be registered as well
builder.Services.AddDomain();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
builder.Services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateUserRequest>, UpdateUserRequestValidator>();
builder.Services.AddScoped<IValidator<CreateForumRequest>, CreateForumRequestValidator>();
builder.Services.AddScoped<IValidator<ModeratorRequest>, ModeratorRequestValidator>();
builder.Services.AddScoped<IValidator<UpdateForumRequest>, UpdateForumRequestValidator>();
builder.Services.AddScoped<IValidator<BlackListedRequest>, BlackListedRequestValidator>();
builder.Services.AddScoped<IValidator<AddPostRequest>, AddPostRequestValidator>();
builder.Services.AddScoped<IValidator<UpdatePostRequest>, UpdatePostRequestValidator>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins(
                "http://localhost:4200",
                "http://localhost:8080"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();;
        });
});

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

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();