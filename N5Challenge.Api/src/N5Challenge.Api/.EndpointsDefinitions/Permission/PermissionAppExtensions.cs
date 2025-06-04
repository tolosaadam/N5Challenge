namespace N5Challenge.Api.EndpointsDefinitions.Permission;

public static class EndPointDefinition
{
    public static WebApplication MapPermissionEndpoints(this WebApplication app)
    {
        app.MapGet("/permissions", Endpoints.GetAll)
            .Produces<IEnumerable<Responses.Permission.PermissionResponse>>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("Get All Permission")
            .WithTags("Permission-Queries")
            .AllowAnonymous()
            .WithOpenApi();

        app.MapPost("/permissions", Endpoints.Create)
            .Produces<int>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("Create Permission")
            .WithTags("Permission-Commands")
            .AllowAnonymous()
            .WithOpenApi();

        app.MapPatch("/permissions/{id}", Endpoints.UpdatePartial)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("Update Partial Permission")
            .WithTags("Permission-Commands")
            .AllowAnonymous()
            .WithOpenApi();

        app.MapPut("/permissions/{id}", Endpoints.Update)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("Update Permission")
            .WithTags("Permission-Commands")
            .AllowAnonymous()
            .WithOpenApi();

        return app;
    }

    public static IServiceCollection AddPermissionServices(this IServiceCollection services, IConfiguration configuration)
    {

        return services;
    }
}
