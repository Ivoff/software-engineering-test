namespace ForumAggregator.Application.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

using ForumAggregator.Application.Services.Authentication;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        return services;
    }
}