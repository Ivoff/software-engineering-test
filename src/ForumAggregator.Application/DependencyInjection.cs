namespace ForumAggregator.Application.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

using ForumAggregator.Application.Services;
using ForumAggregator.Application.UseCases;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRegistrationUseCase, UserRegistrationUserCase>();

        return services;
    }
}