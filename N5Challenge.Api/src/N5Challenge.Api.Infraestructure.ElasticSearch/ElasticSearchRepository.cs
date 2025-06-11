
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using N5Challenge.Api.Application.Interfaces.Persistence;
using N5Challenge.Api.Domain;
using N5Challenge.Api.Infraestructure.Entities;
using Nest;
using System.Security.Cryptography;
using System.Threading;

namespace N5Challenge.Api.Infraestructure.ElasticSearch;
public abstract class ElasticSearchRepository<TDomainModel, TEntityModel, TId>(
    IMapper autoMapper,
    IElasticClient elasticClient,
    ILogger<ElasticSearchRepository<TDomainModel, TEntityModel, TId>> logger) : Repository<TDomainModel, TEntityModel, TId>(autoMapper),
    IReadRepository<TDomainModel, TId>,
    IWriteRepository<TDomainModel, TId>
    where TEntityModel : class, IEntity<TId>
    where TDomainModel : class, IDomainEntity<TId>
{
    private readonly IElasticClient _elasticClient = elasticClient;
    protected abstract string IndexName { get; }
    private readonly ILogger<ElasticSearchRepository<TDomainModel, TEntityModel, TId>> _logger = logger;

    public virtual IEnumerable<TDomainModel> GetAll()
    {
        var response = _elasticClient.Search<TEntityModel>(s => s
        .Index(IndexName)
        .Query(q => q.MatchAll()));

        if (!response.IsValid)
        {
            _logger.LogError("Error fetching all documents from ElasticSearch: {Error}", response.OriginalException?.Message);
            return [];
        }

        return MapToDomainModel(response.Documents);
    }

    public virtual async Task<IEnumerable<TDomainModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _elasticClient.SearchAsync<TEntityModel>(s => s
        .Index(IndexName)
        .Query(q => q.MatchAll()),
        cancellationToken);

        if (!response.IsValid)
        {
            _logger.LogError("Error fetching all documents from ElasticSearch: {Error}", response.OriginalException?.Message);
            return [];
        }

        return MapToDomainModel(response.Documents);
    }

    public virtual TDomainModel? GetById(TId id)
    {
        var response = _elasticClient.Get<TEntityModel>(id!.ToString(), g => g.Index(IndexName));

        if (!response.IsValid || !response.Found)
        {
            _logger.LogWarning("Document with id {Id} not found in ElasticSearch.", id);
            return null;
        }

        return MapToDomainModel(response.Source);
    }

    public virtual async Task<TDomainModel?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        var response = await _elasticClient.GetAsync<TEntityModel>(id!.ToString(), g => g.Index(IndexName), cancellationToken);

        if (!response.IsValid || !response.Found)
        {
            _logger.LogWarning("Document with id {Id} not found in ElasticSearch.", id);
            return null;
        }

        return MapToDomainModel(response.Source);
    }

    public virtual async Task<Func<TId>> AddAsync(TDomainModel entity, CancellationToken cancellationToken)
    {
        var doc = MapToEntityModel(entity);
        var response = await _elasticClient.IndexAsync(doc, i => i
            .Index(IndexName)
            .Id(entity.Id!.ToString())
            .Refresh(Elasticsearch.Net.Refresh.True), cancellationToken);

        if (!response.IsValid)
            throw new InvalidOperationException($"Failed to add document to index {IndexName}: {response.ServerError?.Error?.Reason}");

        return () => entity.Id;
    }

    public Func<TId> Add(TDomainModel domainModel)
    {
        throw new NotImplementedException();
    }

    public TDomainModel? Update(TDomainModel domainModel)
    {
        throw new NotImplementedException();
    }

    public void Delete(TDomainModel domainModel)
    {
        throw new NotImplementedException();
    }

    //public async Task IndexAsync(IEnumerable<IIndexableEntity> entities, string indexName, CancellationToken cancellationToken)
    //{
    //    var bulkResponse = await _elasticClient
    //        .BulkAsync(b => b
    //        .Index(indexName ?? _defaultIndexName)
    //        .IndexMany(entities, (descriptor, entity) => descriptor.Id(entity.Id)),
    //        cancellationToken);


    //    if (bulkResponse.Errors)
    //    {
    //        _logger.LogWarning("Error indexing bulk documents. Entities: {@Entities}, Index: {IndexName}", entities, indexName);
    //    }
    //}

    //public void Index(IIndexableEntity entity, string indexName)
    //{
    //    var indexResponse = _elasticClient
    //        .Index(entity, i => i
    //        .Index(indexName ?? _defaultIndexName)
    //        .Id(entity.Id));

    //    if (!indexResponse.IsValid)
    //    {
    //        _logger.LogWarning("Error indexing document. Entity: {@Entity}, Index: {IndexName}", entity, indexName);
    //    }
    //}

    //public void Index(IEnumerable<IIndexableEntity> entities, string indexName)
    //{
    //    var bulkResponse = _elasticClient
    //        .Bulk(b => b
    //        .Index(indexName ?? _defaultIndexName)
    //        .IndexMany(entities, (descriptor, entity) => descriptor.Id(entity.Id)));


    //    if (bulkResponse.Errors)
    //    {
    //        _logger.LogWarning("Error indexing bulk documents. Entities: {@Entities}, Index: {IndexName}", entities, indexName);
    //    }
    //}
}
