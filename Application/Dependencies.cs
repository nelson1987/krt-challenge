using Application.UseCases.CreateLimite;
using Domain.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class Dependencies
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddScoped<IValidator<CreateLimiteCommand>, CreateLimiteValidator>()
            .AddScoped<ICreateLimiteHandler, CreateLimiteHandler>()
            .AddDomainServices();
        return services;
    }

    private static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services
            .AddScoped<ILimiteService, LimiteService>();
        return services;
    }
}