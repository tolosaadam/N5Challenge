using AutoMapper;
using MediatR;
using N5Challenge.Api.Application.Constants;
using N5Challenge.Api.Application.Interfaces.Persistence;
using N5Challenge.Api.Application.Models;
using N5Challenge.Api.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Application.Permission.Queries.GetAll;

public record GetAllPermissionQuery() : IRequest<IEnumerable<Domain.Permission>>, IPublishEvent
{
    public OperationEnum Operation => OperationEnum.get;
    public string Topic => "permission";
}

public class GetAllPermissionQueryHandler(
    IElasticPermissionRepository elasticPermissionRepository)
    : IRequestHandler<GetAllPermissionQuery, IEnumerable<Domain.Permission>>
{
    private readonly IElasticPermissionRepository _elasticPermissionRepository = elasticPermissionRepository;

    public async Task<IEnumerable<Domain.Permission>> Handle(GetAllPermissionQuery request, CancellationToken cancellationToken)
    {
        var result = await _elasticPermissionRepository.GetAllAsync(true, cancellationToken);

        return result;
    }
}