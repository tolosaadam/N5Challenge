using AutoMapper;
using MediatR;
using N5Challenge.Api.Application.Interfaces.Persistence;
using N5Challenge.Api.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Application.PermissionType.Queries.GetAll;

public record GetAllPermissionTypeQuery() : IRequest<IEnumerable<Domain.PermissionType>>, IPublishEvent
{
    public OperationEnum Operation => OperationEnum.get;

    public string Topic => "permission_type";
};

public class GetAllPermissionTypeQueryHandler(
    IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllPermissionTypeQuery, IEnumerable<Domain.PermissionType>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<IEnumerable<Domain.PermissionType>> Handle(GetAllPermissionTypeQuery request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.GetEfRepository<IEfPermissionTypeRepository>();
        var permissionTypes = await repo.GetAllAsync(cancellationToken);

        return permissionTypes;
    }
}
