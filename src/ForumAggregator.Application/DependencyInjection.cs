namespace ForumAggregator.Application.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

using ForumAggregator.Application.Services;
using ForumAggregator.Application.UseCases;

using Microsoft.AspNetCore.Authentication;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<Application.Services.IAuthenticationService, Application.Services.AuthenticationService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserAuthenticationUseCase, UserAuthenticationUserCase>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IForumUseCase, ForumUseCase>();
        services.AddScoped<IPostUseCase, PostUseCase>();
        services.AddScoped<Application.Services.IForumService, Application.Services.ForumService>();
        
        services.AddScoped<IAppContext, AppContext>();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
        return services;
    }

    public static AuthenticationBuilder CustomAddCookie(this AuthenticationBuilder builder)
    {
        builder.AddCookie(options => {
            options.LoginPath = "/auth/login";
            options.Cookie.Name = "auth_cookie";
            options.ExpireTimeSpan = TimeSpan.FromDays(30);
        });
        
        return builder;
    }
}