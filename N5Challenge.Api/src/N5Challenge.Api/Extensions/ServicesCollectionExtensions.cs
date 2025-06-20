﻿using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using N5Challenge.Api.Application;
using N5Challenge.Api.Application.Behaviors;
using N5Challenge.Api.Application.Interfaces.Persistence;
using N5Challenge.Api.AutoMapperProfiles;
using N5Challenge.Api.Infraestructure.ElasticSearch;
using N5Challenge.Api.Infraestructure.Services.Kafka;
using N5Challenge.Api.Infraestructure.SQL;
using Nest;

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
        services.AddValidatorsFromAssemblyContaining<ApplicationAssemblyReference>();

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
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), x =>
            {
                x.MigrationsAssembly("N5Challenge.Api.Infraestructure.SQL");
            });
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IEfRepositoryFactory, EfRepositoryFactory>();
        services.AddScoped<IEfPermissionRepository, EfPermissionRepository>();
        services.AddScoped<IEfPermissionTypeRepository, EfPermissionTypeRepository>();

        return services;
    }

    public static IServiceCollection AddSwaggerSettings(this IServiceCollection services)
    {
        services.AddSwaggerGen();

        return services;
    }

    public static IServiceCollection AddBehaviorSettings(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuditableEventBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

        return services;
    }

    public static IServiceCollection AddElasticSearchSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = new ConnectionSettings(new Uri(configuration["ElasticSearch:Url"]!))
        .DisableDirectStreaming()
        .EnableApiVersioningHeader();

        var elasticClient = new ElasticClient(settings);

        services.AddSingleton<ElasticSearchSeeder>();
        services.AddSingleton<IElasticClient>(elasticClient);

        services.AddScoped<IElasticPermissionRepository, ElasticPermissionRepository>();
        services.AddScoped<IElasticPermissionTypeRepository, ElasticPermissionTypeRepository>();
        return services;
    }

    public static IServiceCollection AddKafkaSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KafkaSettings>(configuration.GetSection("Kafka"));
        services.AddScoped<IKafkaProducer, KafkaProducer>();

        return services;
    }

    public static IServiceCollection AddCorsSettings(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins", policy =>
            {
                policy.WithOrigins("*")
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        return services;
    }
}
