using AutoMapper;
using Microsoft.Extensions.Logging;
using N5Challenge.Api.Application.Interfaces.Persistence;
using N5Challenge.Api.Infraestructure.Entities;
using N5Challenge.Common.Constants;
using N5Challenge.Common.Infraestructure.Indexables;
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
    : ElasticSearchRepository<Domain.PermissionType, int, IndexablePermissionType, string>(autoMapper, elasticClient, logger),
    IElasticPermissionTypeRepository
{
    protected override string IndexName => EntityRawNameConstants.PERMISSION_TYPES;
}
