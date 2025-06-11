using Microsoft.Extensions.Logging;
using N5Challenge.Consumer.Domain.Models.Interfaces;
using Nest;

namespace N5Challenge.Consumer.ElasticSearch;

public class ElasticSearchService(
    IElasticClient elasticClient,
    ILogger<ElasticSearchService> logger) : 
    IElasticSearchService
{
    private readonly IElasticClient _elasticClient = elasticClient;
    private readonly ILogger<ElasticSearchService> _logger = logger;

    public async Task IndexAsync(IIndexableEntity entity, string indexName, CancellationToken cancellationToken)
    {
        var indexResponse = await _elasticClient
            .IndexAsync(entity, i => i
            .Index(indexName)
            .Id(entity.Id!.ToString())
            .Refresh(Elasticsearch.Net.Refresh.WaitFor), cancellationToken);

        if (!indexResponse.IsValid)
        {
            _logger.LogWarning("Error indexing document. Entity: {@Entity}, Index: {IndexName}", entity, indexName);
        }
    }

    public async Task IndexAsync(IEnumerable<IIndexableEntity> entities, string indexName, CancellationToken cancellationToken)
    {
        var bulkResponse = await _elasticClient
            .BulkAsync(b => b
            .Index(indexName)
            .IndexMany(entities, (descriptor, entity) => descriptor.Id(entity.Id!.ToString()))
            .Refresh(Elasticsearch.Net.Refresh.WaitFor), cancellationToken);


        if (bulkResponse.Errors)
        {
            _logger.LogWarning("Error indexing bulk documents. Entities: {@Entities}, Index: {IndexName}", entities, indexName);
        }
    }

    public void Index(IIndexableEntity entity, string indexName)
    {
        var indexResponse = _elasticClient
            .Index(entity, i => i
            .Index(indexName)
            .Id(entity.Id!.ToString())
            .Refresh(Elasticsearch.Net.Refresh.WaitFor));

        if (!indexResponse.IsValid)
        {
            _logger.LogWarning("Error indexing document. Entity: {@Entity}, Index: {IndexName}", entity, indexName);
        }
    }

    public void Index(IEnumerable<IIndexableEntity> entities, string indexName)
    {
        var bulkResponse = _elasticClient
            .Bulk(b => b
            .Index(indexName)
            .IndexMany(entities, (descriptor, entity) => descriptor.Id(entity.Id!.ToString()))
            .Refresh(Elasticsearch.Net.Refresh.WaitFor));


        if (bulkResponse.Errors)
        {
            _logger.LogWarning("Error indexing bulk documents. Entities: {@Entities}, Index: {IndexName}", entities, indexName);
        }
    }
}
