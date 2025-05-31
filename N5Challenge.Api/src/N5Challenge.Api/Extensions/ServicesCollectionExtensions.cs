using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using N5Challenge.Api.Application;
using N5Challenge.Api.Application.Interfaces;
using N5Challenge.Api.AutoMapperProfiles;
using N5Challenge.Api.Infraestructure.SQL;

namespace N5Challenge.Api.Extensions;

public static class ServicesCollectionExtensions
{
    public static IServiceCollection AddMediatRSettings(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyReference).Assembly);
        });

        return services;
    }

    public static IServiceCollection AddValidatorSettings(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<ApplicationAssemblyReference>()
            .AddClasses(c => c.AssignableTo(typeof(IValidator<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );

        return services;
    }

    public static IServiceCollection AddAutoMapperSettings(this IServiceCollection services)
    {
        services.AddAutoMapper(
                typeof(PermissionProfile),
                typeof(PermissionTypeProfile));

        return services;
    }

    public static IServiceCollection AddInfraestructureSettings(this IServiceCollection services, IConfiguration configuration)
    {
        // DbContext
        services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("TestDb"));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IRepositoryFactory, RepositoryFactory>();

        services.Scan(scan => scan
            .FromAssemblyOf<InfraestructureSQLAssemblyReference>()
            .AddClasses(c => c.AssignableTo(typeof(IRepository)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );

        return services;
    }
}
