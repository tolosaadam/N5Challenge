using AutoMapper;
using MediatR;
using N5Challenge.Api.Application.Exceptions;
using N5Challenge.Api.Application.Interfaces.Persistence;
using N5Challenge.Api.Domain.Constants;
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
    int PermissionTypeId) : IRequest<int>, ICommand, IPublishAuditableEvent, IValidate
{
    public OperationEnum Operation => OperationEnum.request;
    public string Topic => EntityRawNameConstants.PERMISSIONS;
}

public class CreatePermissionCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper autoMapper,
    IElasticPermissionTypeRepository elasticPermissionTypeRepository,
    IElasticPermissionRepository elasticPermissionRepository,
    IKafkaProducer kafkaProducer)
    : IRequestHandler<CreatePermissionCommand, int>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _autoMapper = autoMapper;
    private readonly IElasticPermissionTypeRepository _elasticPermissionTypeRepository = elasticPermissionTypeRepository;
    private readonly IElasticPermissionRepository _elasticPermissionRepository = elasticPermissionRepository;
    private readonly IKafkaProducer _kafkaProducer = kafkaProducer;

    public async Task<int> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
    {
        var ptDomain = await _elasticPermissionTypeRepository.GetByIdAsync(request.PermissionTypeId, cancellationToken);

        if (ptDomain is null)
        {
            throw new RelatedEntityNotFoundException(
                nameof(Domain.Permission),
                nameof(Domain.PermissionType),
                request.PermissionTypeId);
        }

        var pRepository = _unitOfWork.GetEfRepository<IEfPermissionRepository>();

        var pDomain = _autoMapper.Map<Domain.Permission>(request);

        pDomain.Date = DateTime.UtcNow;

        var getId = await pRepository.AddAsync(pDomain, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var id = getId();

        pDomain.Id = id;

        await _kafkaProducer.PublishEntityEventAsync(request.Topic, pDomain, request.Operation, cancellationToken);

        return id;
    }
}
