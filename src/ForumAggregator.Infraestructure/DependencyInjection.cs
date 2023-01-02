namespace ForumAggregator.Infraestructure.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

using ForumAggregator.Infraestructure.Repository;
using ForumAggregator.Domain.Shared.Interfaces;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddInfraestructure(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        return services;
    }
}