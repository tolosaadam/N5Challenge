using Microsoft.EntityFrameworkCore;
using N5Challenge.Api.EndpointsDefinitions;
using N5Challenge.Api.Extensions;
using N5Challenge.Api.Infraestructure.SQL;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine($"Ambiente actual: {builder.Environment.EnvironmentName}");

builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddMediatRSettings()
    .AddAutoMapperSettings()
    .AddValidatorSettings()
    .AddInfraestructureSettings(builder.Configuration, builder.Environment)
    .AddSwaggerSettings();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

if (app.Environment.IsDevelopment())
{
    db.Database.EnsureCreated();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{   
    db.Database.Migrate();
}

app.UseHttpsRedirection();

//builder.Configuration
//    .AddJsonFile("appsettings.json", optional: false)
//    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
//    .AddEnvironmentVariables();

app.MapPermissionEndpoints()
    .Run();
