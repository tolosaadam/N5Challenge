using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using N5Challenge.Api.Application.Interfaces.Persistence;
using N5Challenge.Api.Domain.Constants;
using N5Challenge.Api.Infraestructure.Entities;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Infraestructure.ElasticSearch;

public class ElasticPermissionRepository(
    IMapper autoMapper,
    IElasticClient elasticClient,
    ILogger<ElasticPermissionRepository> logger,
    IElasticPermissionTypeRepository permissionTypeRepository) 
    : ElasticSearchRepository<Domain.Permission, PermissionDB, int>(autoMapper, elasticClient, logger),
    IElasticPermissionRepository
{
    protected override string IndexName => IndexNamesConstans.PERMISSION_INDEX_NAME;
    private readonly IElasticPermissionTypeRepository _permissionTypeRepository = permissionTypeRepository;

    public async Task<IEnumerable<Domain.Permission>> GetAllAsync(bool include = false, CancellationToken cancellationToken = default)
    {
        var permissions = await base.GetAllAsync(cancellationToken);

        if (!include)
        {
            return permissions;
        }
        
        var types = await _permissionTypeRepository.GetAllAsync(cancellationToken);

        var permissionTypesById = types.ToDictionary(t => t.Id);

        foreach (var permission in permissions)
        {
            if (permissionTypesById.TryGetValue(permission.PermissionTypeId, out var type))
            {
                permission.Type = type;
            }
        }
        return permissions;
    }
}
