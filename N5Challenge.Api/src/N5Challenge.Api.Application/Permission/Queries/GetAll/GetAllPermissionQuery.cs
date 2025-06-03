using AutoMapper;
using MediatR;
using N5Challenge.Api.Application.Constants;
using N5Challenge.Api.Application.Interfaces;
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
}

public class GetAllPermissionQueryHandler(
    IUnitOfWork unitOfWork,
    IMapper autoMapper,
    IElasticSearch elasticSearch)
    : IRequestHandler<GetAllPermissionQuery, IEnumerable<Domain.Permission>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _autoMapper = autoMapper;
    private readonly IElasticSearch _elasticSearch = elasticSearch;

    public async Task<IEnumerable<Domain.Permission>> Handle(GetAllPermissionQuery request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.GetRepository<IPermissionRepository>();
        var permissions = await repo.GetAllAsync(cancellationToken);

        #region ElasticSearch
        var indexablePermissions = _autoMapper.Map<IEnumerable<IndexablePermission>>(permissions);
        await _elasticSearch.IndexAsync(indexablePermissions, IndexNamesConstans.PERMISSION_INDEX_NAME, cancellationToken);
        #endregion

        return permissions;
    }
}