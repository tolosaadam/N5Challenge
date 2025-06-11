using AutoMapper;
using Microsoft.Extensions.Logging;
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

public class ElasticPermissionTypeRepository(
    IMapper autoMapper,
    IElasticClient elasticClient,
    ILogger<ElasticPermissionTypeRepository> logger)
    : ElasticSearchRepository<Domain.PermissionType, PermissionTypeDB, int>(autoMapper, elasticClient, logger),
    IElasticPermissionTypeRepository
{
    protected override string IndexName => IndexNamesConstans.PERMISSION_TYPE_INDEX_NAME;
}
