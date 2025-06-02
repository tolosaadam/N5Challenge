using AutoMapper;
using MediatR;
using N5Challenge.Api.Application.Exceptions;
using N5Challenge.Api.Application.Interfaces;
using N5Challenge.Api.Application.Interfaces.Persistence;
using N5Challenge.Api.Application.Permission.Queries.GetAll;
using N5Challenge.Api.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Application.Permission.Commands.Create;

public record CreatePermissionCommand(
    string EmployeeFirstName,
    string EmployeeLastName,
    int PermissionTypeId) : IRequest<int>, ICommand, IAuditable, IPublishEvent
{
    public OperationEnum Operation => OperationEnum.request;
}

public class CreatePermissionCommandHandler(IUnitOfWork unitOfWork, IMapper autoMapper) : IRequestHandler<CreatePermissionCommand, int>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _autoMapper = autoMapper;

    public async Task<int> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
    {
        var ptRepository = _unitOfWork.GetRepository<IPermissionTypeRepository>();

        var ptDomain = await ptRepository.GetByIdAsync(request.PermissionTypeId, cancellationToken);

        if (ptDomain is null)
        {
            throw new RelatedEntityNotFoundException(
                nameof(Domain.Permission),
                nameof(Domain.PermissionType),
                request.PermissionTypeId);
        }

        var pRepository = _unitOfWork.GetRepository<IPermissionRepository>();

        var pDomain = _autoMapper.Map<Domain.Permission>(request);

        pDomain.Date = DateTime.UtcNow;

        var getId = await pRepository.AddAsync(pDomain, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var id = getId();

        return id;
    }
}
