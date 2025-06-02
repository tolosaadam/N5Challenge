using AutoMapper;
using MediatR;
using N5Challenge.Api.Application.Exceptions;
using N5Challenge.Api.Application.Interfaces;
using N5Challenge.Api.Application.Interfaces.Persistence;
using N5Challenge.Api.Application.Permission.Commands.Create;
using N5Challenge.Api.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Application.Permission.Commands.Update;

public record UpdatePermissionCommand(
    int Id,
    string EmployeeFirstName,
    string EmployeeLastName,
    int PermissionTypeId,
    DateTime Date) : IRequest, ICommand, IAuditable, IPublishEvent
{
    public OperationEnum Operation => OperationEnum.modify;
}

public class UpdatePermissionCommandHandler(IUnitOfWork unitOfWork, IMapper autoMapper) : IRequestHandler<UpdatePermissionCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _autoMapper = autoMapper;


    public async Task Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
    {

        var pRepository = _unitOfWork.GetRepository<IPermissionRepository>();

        var pEntity = await pRepository.GetByIdAsync(request.Id, cancellationToken);

        if (pEntity is null)
        {
            throw new NotFoundException(nameof(Domain.Permission), request.Id);
        }

        var ptRepository = _unitOfWork.GetRepository<IPermissionTypeRepository>();

        var ptDomain = await ptRepository.GetByIdAsync(request.PermissionTypeId, cancellationToken);

        if (ptDomain is null)
        {
            throw new RelatedEntityNotFoundException(
                nameof(Domain.Permission),
                nameof(Domain.PermissionType),
                request.PermissionTypeId);
        }

        var pDomain = _autoMapper.Map<Domain.Permission>(request);

        pRepository.Update(pDomain);
    }
}