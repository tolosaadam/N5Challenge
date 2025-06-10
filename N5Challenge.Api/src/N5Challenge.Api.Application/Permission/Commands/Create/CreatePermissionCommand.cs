using AutoMapper;
using MediatR;
using N5Challenge.Api.Application.Constants;
using N5Challenge.Api.Application.Exceptions;
using N5Challenge.Api.Application.Interfaces.Persistence;
using N5Challenge.Api.Application.Models;
using N5Challenge.Api.Application.Permission.Queries.GetAll;
using N5Challenge.Api.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace N5Challenge.Api.Application.Permission.Commands.Create;

public record CreatePermissionCommand(
    string EmployeeFirstName,
    string EmployeeLastName,
    int PermissionTypeId) : IRequest<int>, ICommand, IPublishEvent, IValidate
{
    public OperationEnum Operation => OperationEnum.request;
    public string Topic => "permission";
}

public class CreatePermissionCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper autoMapper,
    IElasticSearch elasticSearch)
    : IRequestHandler<CreatePermissionCommand, int>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _autoMapper = autoMapper;
    private readonly IElasticSearch _elasticSearch = elasticSearch;

    public async Task<int> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
    {
        var ptRepository = _unitOfWork.GetEfRepository<IPermissionTypeRepository>();

        var ptDomain = await ptRepository.GetByIdAsync(request.PermissionTypeId, cancellationToken);

        if (ptDomain is null)
        {
            throw new RelatedEntityNotFoundException(
                nameof(Domain.Permission),
                nameof(Domain.PermissionType),
                request.PermissionTypeId);
        }

        var pRepository = _unitOfWork.GetEfRepository<IPermissionRepository>();

        var pDomain = _autoMapper.Map<Domain.Permission>(request);

        pDomain.Date = DateTime.UtcNow;

        var getId = await pRepository.AddAsync(pDomain, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var id = getId();

        #region ElasticSearch
        var indexablePermission = _autoMapper.Map<IndexablePermission>((pDomain, id));
        await _elasticSearch.IndexAsync(indexablePermission, IndexNamesConstans.PERMISSION_INDEX_NAME, cancellationToken);
        #endregion

        return id;
    }
}
