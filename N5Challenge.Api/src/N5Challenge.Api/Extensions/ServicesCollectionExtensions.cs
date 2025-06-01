using FluentValidation;
using Microsoft.EntityFrameworkCore;
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

    public static IServiceCollection AddInfraestructureSettings(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("N5ChallengeDb");
            });
        }
        else
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), x =>
                {
                    x.MigrationsAssembly("N5Challenge.Api.Infraestructure.SQL");
                });
            });
        }

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

    public static IServiceCollection AddSwaggerSettings(this IServiceCollection services)
    {
        services.AddSwaggerGen();

        return services;
    }
}
