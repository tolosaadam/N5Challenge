using AutoMapper;
using MediatR;
using N5Challenge.Api.Application.Exceptions;
using N5Challenge.Api.Application.Interfaces.Persistence;
using N5Challenge.Common.Constants;
using N5Challenge.Common.Enums;
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
    DateTime? Date) : IRequest, ICommand, IPublishAuditableEvent, IValidate
{
    public OperationEnum Operation => OperationEnum.modify;
    public string Topic => EntityRawNameConstants.PERMISSIONS;
}

public class UpdatePartialPermissionCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper autoMapper,
    IElasticPermissionRepository elasticPermissionRepository,
    IElasticPermissionTypeRepository elasticPermissionTypeRepository,
    IKafkaProducer kafkaProducer)
    : IRequestHandler<UpdatePartialPermissionCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _autoMapper = autoMapper;
    private readonly IElasticPermissionRepository _elasticPermissionRepository = elasticPermissionRepository;
    private readonly IElasticPermissionTypeRepository _elasticPermissionTypeRepository = elasticPermissionTypeRepository;
    private readonly IKafkaProducer _kafkaProducer = kafkaProducer;

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

        await _kafkaProducer.PublishEntityEventAsync(request.Topic, updatedP, request.Operation, cancellationToken);
    }
}