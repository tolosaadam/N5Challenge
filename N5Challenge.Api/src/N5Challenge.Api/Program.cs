using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using N5Challenge.Api.Application.Exceptions;
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
    .AddSwaggerSettings()
    .AddBehaviorSettings()
    .AddElasticSearchSettings(builder.Configuration);

var app = builder.Build();

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
db.Database.Migrate();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(errorApp =>
    errorApp.Run(async context =>
    {
        var exceptionHandler = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;

        if (exceptionHandler is NotFoundException)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new { error = exceptionHandler.Message });
        }
        else if (exceptionHandler is RelatedEntityNotFoundException)
        {
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            await context.Response.WriteAsJsonAsync(new { error = exceptionHandler.Message });
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred." });
        }
    })
);

app.UseHttpsRedirection();

app.MapPermissionEndpoints()
    .Run();
