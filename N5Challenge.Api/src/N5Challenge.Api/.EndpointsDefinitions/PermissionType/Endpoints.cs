using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using N5Challenge.Api.Application.Permission.Queries.GetAll;
using N5Challenge.Api.Application.PermissionType.Queries.GetAll;

namespace N5Challenge.Api.EndpointsDefinitions.PermissionType;

public class Endpoints
{
    internal static async Task<IResult> GetAll(
    [FromServices] IMediator mediator,
    [FromServices] IMapper autoMapper,
    CancellationToken cancellationToken)
    {
        var query = new GetAllPermissionTypeQuery();
        var result = await mediator.Send(query, cancellationToken);
        var mappedResult = autoMapper.Map<IEnumerable<Responses.PermissionType.PermissionTypeResponse>>(result);
        return Results.Ok(mappedResult);
    }
}
