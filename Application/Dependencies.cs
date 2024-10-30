using Application.UseCases.ChangeLimite;
using Application.UseCases.CreateLimite;
using Application.UseCases.DeleteLimite;
using Domain.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class Dependencies
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddCreateLimite()
            .AddUpdateLimite()
            .AddDeleteLimite()

            .AddDomainServices();
        return services;
    }

    public static IServiceCollection AddCreateLimite(this IServiceCollection services)
    {
        return services
            .AddScoped<IValidator<CreateLimiteCommand>, CreateLimiteValidator>()
            .AddScoped<ICreateLimiteHandler, CreateLimiteHandler>();
    }

    public static IServiceCollection AddUpdateLimite(this IServiceCollection services)
    {
        return services
            .AddScoped<IValidator<ChangeLimiteCommand>, ChangeLimiteValidator>()
            .AddScoped<IChangeLimiteHandler, ChangeLimiteHandler>();
    }

    public static IServiceCollection AddDeleteLimite(this IServiceCollection services)
    {
        return services
            .AddScoped<IValidator<DeleteLimiteCommand>, DeleteLimiteValidator>()
            .AddScoped<IDeleteLimiteHandler, DeleteLimiteHandler>();
    }

    private static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services
            .AddScoped<ILimiteService, LimiteService>();
        return services;
    }
}