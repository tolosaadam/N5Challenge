
using Microsoft.Extensions.Options;
using N5Challenge.Api.Application.Interfaces.Persistence;
using N5Challenge.Api.Domain;
using N5Challenge.Api.Domain.Configuration;
using Nest;
using System.Threading;

namespace N5Challenge.Api.Infraestructure.Services.ElasticSearch;
public class ElasticSearch<TIndexableEntity>(IElasticClient elasticClient, IOptions<ElasticSearchSettings> esSettings) : IElasticSearch<TIndexableEntity>
    where TIndexableEntity : class, IIndexableEntity
{
    private readonly IElasticClient _elasticClient = elasticClient;
    private readonly string _indexName = esSettings?.Value?.DefaultIndexName!;

    public async Task IndexAsync(TIndexableEntity entity, string indexName, CancellationToken cancellationToken)
    {
        var indexResponse = await _elasticClient
            .IndexAsync(entity, i => i
            .Index(_indexName)
            .Id(entity.Id), cancellationToken);

        if (!indexResponse.IsValid)
        {
            throw new Exception($"Error indexing document Id={entity.Id}: {indexResponse.ServerError}");
        }
    }

    public async Task IndexAsync(IEnumerable<TIndexableEntity> entities, string indexName, CancellationToken cancellationToken)
    {
        var bulkResponse = await _elasticClient
            .BulkAsync(b => b
            .Index(_indexName)
            .IndexMany(entities, (descriptor, entity) => descriptor.Id(entity.Id)),
            cancellationToken);


        if (bulkResponse.Errors)
        {
            throw new Exception("Error indexing bulk documents");
        }
    }

    public void Index(TIndexableEntity entity, string indexName)
    {
        var indexResponse = _elasticClient
            .Index(entity, i => i
            .Index(_indexName)
            .Id(entity.Id));

        if (!indexResponse.IsValid)
        {
            throw new Exception($"Error indexing document Id={entity.Id}: {indexResponse.ServerError}");
        }
    }

    public void Index(IEnumerable<TIndexableEntity> entities, string indexName)
    {
        var bulkResponse = _elasticClient
            .Bulk(b => b
            .Index(_indexName)
            .IndexMany(entities, (descriptor, entity) => descriptor.Id(entity.Id)));


        if (bulkResponse.Errors)
        {
            throw new Exception("Error indexing bulk documents");
        }
    }
}
