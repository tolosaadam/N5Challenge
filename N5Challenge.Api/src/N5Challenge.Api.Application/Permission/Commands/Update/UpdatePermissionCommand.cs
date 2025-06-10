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

namespace N5Challenge.Api.Application.Permission.Commands.Update;

public record UpdatePermissionCommand(
    int Id,
    string EmployeeFirstName,
    string EmployeeLastName,
    int PermissionTypeId,
    DateTime Date) : IRequest, ICommand, IPublishEvent, IValidate
{
    public OperationEnum Operation => OperationEnum.modify;
    public string Topic => "permission";
}

public class UpdatePermissionCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper autoMapper,
    IElasticSearch elasticSearch)
    : IRequestHandler<UpdatePermissionCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _autoMapper = autoMapper;
    private readonly IElasticSearch _elasticSearch = elasticSearch;


    public async Task Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
    {

        var pRepository = _unitOfWork.GetEfRepository<IPermissionRepository>();

        var permission = await pRepository.GetByIdAsync(request.Id, cancellationToken);

        if (permission is null)
        {
            throw new EntityNotFoundException(nameof(Domain.Permission), request.Id);
        }

        var ptRepository = _unitOfWork.GetEfRepository<IPermissionTypeRepository>();

        var ptDomain = await ptRepository.GetByIdAsync(request.PermissionTypeId, cancellationToken);

        if (ptDomain is null)
        {
            throw new RelatedEntityNotFoundException(
                nameof(Domain.Permission),
                nameof(Domain.PermissionType),
                request.PermissionTypeId);
        }

        var pToUpdate = _autoMapper.Map<Domain.Permission>(request);

        var updatedP = pRepository.Update(pToUpdate);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        #region ElasticSearch
        var indexablePermission = _autoMapper.Map<IndexablePermission>(updatedP);
        await _elasticSearch.IndexAsync(indexablePermission, IndexNamesConstans.PERMISSION_INDEX_NAME, cancellationToken);
        #endregion
    }
}
