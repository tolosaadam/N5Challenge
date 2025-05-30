using FluentValidation;
using MediatR;
using N5Challenge.Api.Application;
using N5Challenge.Api.AutoMapperProfiles;

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
}
