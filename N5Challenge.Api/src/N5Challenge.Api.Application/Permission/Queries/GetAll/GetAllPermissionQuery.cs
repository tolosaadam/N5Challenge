using MediatR;
using N5Challenge.Api.Application.Interfaces;
using N5Challenge.Api.Application.Interfaces.Persistence;
using N5Challenge.Api.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Application.Permission.Queries.GetAll;

public record GetAllPermissionQuery() : IRequest<IEnumerable<Domain.Permission>>, IAuditable, IPublishEvent
{
    public OperationEnum Operation => OperationEnum.get;
}

public class GetAllPermissionQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllPermissionQuery, IEnumerable<Domain.Permission>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<IEnumerable<Domain.Permission>> Handle(GetAllPermissionQuery request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.GetRepository<IPermissionRepository>();
        var permissions = await repo.GetAllAsync(cancellationToken);
        return permissions;
    }
}