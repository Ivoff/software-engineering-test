namespace ForumAggregator.Domain.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

using ForumAggregator.Domain.Services;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<ForumAggregator.Domain.Services.IUserService, ForumAggregator.Domain.Services.UserService>();

        return services;
    }
}