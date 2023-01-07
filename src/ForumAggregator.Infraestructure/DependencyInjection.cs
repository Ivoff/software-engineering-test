namespace ForumAggregator.Infraestructure.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.Configuration.Json;

using ForumAggregator.Infraestructure.Repository;
using ForumAggregator.Domain.Shared.Interfaces;
using ForumAggregator.Infraestructure.DbContext;


public static class ServiceCollectionExtension
{
    public static IServiceCollection AddInfraestructure(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IForumRepository, ForumRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        
        services.AddDbContext<DatabaseContext>((options) => {
            options.UseNpgsql(@"Host=localhost;Port=5000;Username=postgres;Password=postgres;Database=forum_aggregator;IncludeErrorDetail=true");
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
        });

        return services;
    }
}