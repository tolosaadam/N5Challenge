
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using N5Challenge.Api.Application.Interfaces;
using N5Challenge.Api.Application.Interfaces.Persistence;
using N5Challenge.Api.Domain;
using N5Challenge.Api.Domain.Configuration;
using Nest;
using System.Threading;

namespace N5Challenge.Api.Infraestructure.Services.ElasticSearch;
public class ElasticSearch(IElasticClient elasticClient, IOptions<ElasticSearchSettings> esSettings, ILogger<ElasticSearch> logger) : IElasticSearch
{
    private readonly IElasticClient _elasticClient = elasticClient;
    private readonly string _defaultIndexName = esSettings?.Value?.DefaultIndexName!;
    private readonly ILogger<ElasticSearch> _logger = logger;

    public async Task IndexAsync(IIndexableEntity entity, string indexName, CancellationToken cancellationToken)
    {
        var indexResponse = await _elasticClient
            .IndexAsync(entity, i => i
            .Index(indexName ?? _defaultIndexName)
            .Id(entity.Id), cancellationToken);

        if (!indexResponse.IsValid)
        {
            _logger.LogWarning("Error indexing document. Entity: {@Entity}, Index: {IndexName}", entity, indexName);
        }
    }

    public async Task IndexAsync(IEnumerable<IIndexableEntity> entities, string indexName, CancellationToken cancellationToken)
    {
        var bulkResponse = await _elasticClient
            .BulkAsync(b => b
            .Index(indexName ?? _defaultIndexName)
            .IndexMany(entities, (descriptor, entity) => descriptor.Id(entity.Id)),
            cancellationToken);


        if (bulkResponse.Errors)
        {
            _logger.LogWarning("Error indexing bulk documents. Entities: {@Entities}, Index: {IndexName}", entities, indexName);
        }
    }

    public void Index(IIndexableEntity entity, string indexName)
    {
        var indexResponse = _elasticClient
            .Index(entity, i => i
            .Index(indexName ?? _defaultIndexName)
            .Id(entity.Id));

        if (!indexResponse.IsValid)
        {
            _logger.LogWarning("Error indexing document. Entity: {@Entity}, Index: {IndexName}", entity, indexName);
        }
    }

    public void Index(IEnumerable<IIndexableEntity> entities, string indexName)
    {
        var bulkResponse = _elasticClient
            .Bulk(b => b
            .Index(indexName ?? _defaultIndexName)
            .IndexMany(entities, (descriptor, entity) => descriptor.Id(entity.Id)));


        if (bulkResponse.Errors)
        {
            _logger.LogWarning("Error indexing bulk documents. Entities: {@Entities}, Index: {IndexName}", entities, indexName);
        }
    }
}
