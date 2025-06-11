using AutoMapper;
using MediatR;
using N5Challenge.Api.Application.Constants;
using N5Challenge.Api.Application.Exceptions;
using N5Challenge.Api.Application.Interfaces.Persistence;
using N5Challenge.Api.Application.Models;
using N5Challenge.Api.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Application.Permission.Commands.UpdatePartial;

public record UpdatePartialPermissionCommand(
    int Id,
    string? EmployeeFirstName,
    string? EmployeeLastName,
    int? PermissionTypeId,
    DateTime? Date) : IRequest, ICommand, IPublishEvent, IValidate
{
    public OperationEnum Operation => OperationEnum.modify_partial;
    public string Topic => "permission";
}

public class UpdatePartialPermissionCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper autoMapper,
    IElasticPermissionRepository elasticPermissionRepository,
    IElasticPermissionTypeRepository elasticPermissionTypeRepository)
    : IRequestHandler<UpdatePartialPermissionCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _autoMapper = autoMapper;
    private readonly IElasticPermissionRepository _elasticPermissionRepository = elasticPermissionRepository;
    private readonly IElasticPermissionTypeRepository _elasticPermissionTypeRepository = elasticPermissionTypeRepository;

    public async Task Handle(UpdatePartialPermissionCommand request, CancellationToken cancellationToken)
    {
        var pRepository = _unitOfWork.GetEfRepository<IEfPermissionRepository>();

        var permission = await _elasticPermissionRepository.GetByIdAsync(request.Id, cancellationToken);

        if (permission is null)
        {
            throw new EntityNotFoundException(nameof(Domain.Permission), request.Id);
        }

        if (request.PermissionTypeId is not null)
        {
            var ptDomain = await _elasticPermissionTypeRepository.GetByIdAsync(request.PermissionTypeId.Value, cancellationToken);

            if (ptDomain is null)
            {
                throw new RelatedEntityNotFoundException(
                    nameof(Domain.Permission),
                    nameof(Domain.PermissionType),
                    request.PermissionTypeId);
            }
        }

        _autoMapper.Map(request, permission);

        var updatedP = pRepository.Update(permission);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}