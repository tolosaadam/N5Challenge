namespace N5Challenge.Api.EndpointsDefinitions.PermissionType;

public static class EndPointDefinition
{
    public static WebApplication MapPermissionTypeEndpoints(this WebApplication app)
    {
        app.MapGet("/permission-types", Endpoints.GetAll)
            .Produces<IEnumerable<Responses.PermissionType.PermissionTypeResponse>>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("Get All PermissionTypes")
            .WithTags("PermissionType-Queries")
            .AllowAnonymous()
            .WithOpenApi();

        return app;
    }

    public static IServiceCollection AddPermissionTypeServices(this IServiceCollection services, IConfiguration configuration)
    {

        return services;
    }
}