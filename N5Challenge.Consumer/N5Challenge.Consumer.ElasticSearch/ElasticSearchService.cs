using Microsoft.Extensions.Logging;
using Nest;

namespace N5Challenge.Consumer.ElasticSearch;

public class ElasticSearchService(
    IElasticClient elasticClient,
    ILogger<ElasticSearchService> logger) : 
    IElasticSearchService
{
    private readonly IElasticClient _elasticClient = elasticClient;
    private readonly ILogger<ElasticSearchService> _logger = logger;

    public async Task IndexAsync(Common.Infraestructure.Interfaces.IIndexableEntity entity, string indexName, CancellationToken cancellationToken)
    {
        var indexResponse = await _elasticClient
            .IndexAsync(entity, i => i
            .Index(indexName)
            .Id(entity.Id)
            .Refresh(Elasticsearch.Net.Refresh.WaitFor), cancellationToken);

        if (!indexResponse.IsValid)
        {
            _logger.LogWarning("Error indexing document. Entity: {@Entity}, Index: {IndexName}", entity, indexName);
        }
    }

    public async Task IndexAsync(IEnumerable<Common.Infraestructure.Interfaces.IIndexableEntity> entities, string indexName, CancellationToken cancellationToken)
    {
        var bulkResponse = await _elasticClient
            .BulkAsync(b => b
            .Index(indexName)
            .IndexMany(entities, (descriptor, entity) => descriptor.Id(entity.Id))
            .Refresh(Elasticsearch.Net.Refresh.WaitFor), cancellationToken);


        if (bulkResponse.Errors)
        {
            _logger.LogWarning("Error indexing bulk documents. Entities: {@Entities}, Index: {IndexName}", entities, indexName);
        }
    }

    public void Index(Common.Infraestructure.Interfaces.IIndexableEntity entity, string indexName)
    {
        var indexResponse = _elasticClient
            .Index(entity, i => i
            .Index(indexName)
            .Id(entity.Id)
            .Refresh(Elasticsearch.Net.Refresh.WaitFor));

        if (!indexResponse.IsValid)
        {
            _logger.LogWarning("Error indexing document. Entity: {@Entity}, Index: {IndexName}", entity, indexName);
        }
    }

    public void Index(IEnumerable<Common.Infraestructure.Interfaces.IIndexableEntity> entities, string indexName)
    {
        var bulkResponse = _elasticClient
            .Bulk(b => b
            .Index(indexName)
            .IndexMany(entities, (descriptor, entity) => descriptor.Id(entity.Id))
            .Refresh(Elasticsearch.Net.Refresh.WaitFor));


        if (bulkResponse.Errors)
        {
            _logger.LogWarning("Error indexing bulk documents. Entities: {@Entities}, Index: {IndexName}", entities, indexName);
        }
    }
}
