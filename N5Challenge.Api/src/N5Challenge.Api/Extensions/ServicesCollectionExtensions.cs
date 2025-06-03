using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using N5Challenge.Api.Application;
using N5Challenge.Api.Application.Behaviors;
using N5Challenge.Api.Application.Interfaces.Persistence;
using N5Challenge.Api.Application.Permission.Commands.Create;
using N5Challenge.Api.Application.Permission.Commands.Update;
using N5Challenge.Api.AutoMapperProfiles;
using N5Challenge.Api.Infraestructure.Services.ElasticSearch;
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
        services.AddScoped<CreatePermissionCommandValidator>();
        services.AddScoped<UpdatePermissionCommandValidator>();

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
        services.AddScoped<IRepositoryFactory, RepositoryFactory>();
        services.AddScoped<IRepository, Repository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IPermissionTypeRepository, PermissionTypeRepository>();

        return services;
    }

    public static IServiceCollection AddSwaggerSettings(this IServiceCollection services)
    {
        services.AddSwaggerGen();

        return services;
    }

    public static IServiceCollection AddBehaviorSettings(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(EventBehavior<,>));

        return services;
    }

    public static IServiceCollection AddElasticSearchSettings(this IServiceCollection services, IConfiguration configuration)
    {

        services.Configure<ElasticSearchSettings>(configuration.GetSection("ElasticSearch"));

        var settings = new ConnectionSettings(new Uri(configuration["ElasticSearch:Url"]))
        .DisableDirectStreaming()
        .EnableApiVersioningHeader()
        .DefaultIndex(configuration["ElasticSearch:IndexName"]);

        var elasticClient = new ElasticClient(settings);

        services.AddSingleton<IElasticClient>(elasticClient);

        services.AddScoped(typeof(IElasticSearch), typeof(ElasticSearch));
        return services;
    }

    public static IServiceCollection AddKafkaSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KafkaSettings>(configuration.GetSection("Kafka"));
        services.AddScoped<IKafkaProducer, KafkaProducer>();

        return services;
    }
}
