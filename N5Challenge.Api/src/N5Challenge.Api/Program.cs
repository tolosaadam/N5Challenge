using Microsoft.EntityFrameworkCore;
using N5Challenge.Api.EndpointsDefinitions.Permission;
using N5Challenge.Api.EndpointsDefinitions.PermissionType;
using N5Challenge.Api.Extensions;
using N5Challenge.Api.Infraestructure.ElasticSearch;
using N5Challenge.Api.Infraestructure.SQL;
using N5Challenge.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine($"Ambiente actual: {builder.Environment.EnvironmentName}");

builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddMediatRSettings()
    .AddAutoMapperSettings()
    .AddValidatorSettings()
    .AddInfraestructureSettings(builder.Configuration, builder.Environment)
    .AddSwaggerSettings()
    .AddBehaviorSettings()
    .AddElasticSearchSettings(builder.Configuration)
    .AddKafkaSettings(builder.Configuration)
    .AddCorsSettings();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
db.Database.Migrate();
var seeder = scope.ServiceProvider.GetRequiredService<ElasticSearchSeeder>();
await seeder.SeedPermissionsAsync();
await seeder.SeedPermissionTypesAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins")
    .UseHttpsRedirection()
    .UseMiddleware<HandlingExceptionMiddleware>();

app.MapPermissionEndpoints()
    .MapPermissionTypeEndpoints()
    .Run();
