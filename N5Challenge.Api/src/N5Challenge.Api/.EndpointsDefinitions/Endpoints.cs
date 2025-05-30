using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using N5Challenge.Api.Application.Permission.Commands.Create;
using N5Challenge.Api.Application.Permission.Commands.Update;
using N5Challenge.Api.Application.Permission.Queries.GetAll;

namespace N5Challenge.Api.EndpointsDefinitions;

public class Endpoints
{
    internal static async Task<IResult> GetAll(
        [FromServices] IMediator mediator,
        [FromServices] IMapper autoMapper,
        CancellationToken cancellationToken)
    {
        var query = new GetAllPermissionQuery();
        var result = await mediator.Send(query, cancellationToken);
        var mappedResult = autoMapper.Map<IEnumerable<Responses.Permission.PermissionResponse>>(result);
        return Results.Ok(mappedResult);
    }

    internal static async Task<IResult> Create(
        [FromServices] IMediator mediator,
        [FromServices] IMapper autoMapper,
        Requests.Permission.PermissionRequest request,
        CancellationToken cancellationToken)
    {
        var permission = autoMapper.Map<Requests.Permission.PermissionRequest, Domain.Permission>(request);
        var command = new CreatePermissionCommand(permission);
        var result = await mediator.Send(command, cancellationToken);
        return Results.Created($"/api/permissions/{result}", new { id = result });
    }

    internal static async Task<IResult> Update(
        [FromServices] IMediator mediator,
        [FromServices] IMapper autoMapper,
        int id,
        Requests.Permission.PermissionRequest request,
        CancellationToken cancellationToken)
    {
        var permission = autoMapper.Map<Requests.Permission.PermissionRequest, Domain.Permission>(request);
        var command = new UpdatePermissionCommand(id, permission);
        await mediator.Send(command, cancellationToken);
        return Results.NoContent();
    }
}
