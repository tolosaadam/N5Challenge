
using AutoMapper;
using Microsoft.Extensions.Logging;
using N5Challenge.Api.Application.Interfaces.Persistence;
using N5Challenge.Api.Domain;
using N5Challenge.Common.Infraestructure;
using Nest;
using System.Security.Cryptography;
using System.Threading;

namespace N5Challenge.Api.Infraestructure.ElasticSearch;
public abstract class ElasticSearchRepository<TDomainModel, TDomainId, TEntityModel, TEntityId>(
    IMapper autoMapper,
    IElasticClient elasticClient,
    ILogger<ElasticSearchRepository<TDomainModel, TDomainId, TEntityModel, TEntityId>> logger) 
    : Repository<TDomainModel, TDomainId, TEntityModel, TEntityId>(autoMapper),
    IReadRepository<TDomainModel, TDomainId>
    where TEntityModel : class, IEntity<TEntityId>
    where TDomainModel : class, IDomainEntity<TDomainId>
{
    private readonly IElasticClient _elasticClient = elasticClient;
    protected abstract string IndexName { get; }
    private readonly ILogger<ElasticSearchRepository<TDomainModel, TDomainId, TEntityModel, TEntityId>> _logger = logger;

    public virtual IEnumerable<TDomainModel> GetAll()
    {
        var response = _elasticClient.Search<TEntityModel>(s => s
        .Index(IndexName)
        .From(0)
        .Size(100)
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
        .From(0)
        .Size(100)
        .Query(q => q.MatchAll()),
        cancellationToken);

        if (!response.IsValid)
        {
            _logger.LogError("Error fetching all documents from ElasticSearch: {Error}", response.OriginalException?.Message);
            return [];
        }

        return MapToDomainModel(response.Documents);
    }

    public virtual TDomainModel? GetById(TDomainId id)
    {
        var response = _elasticClient.Get<TEntityModel>(id!.ToString(), g => g.Index(IndexName));

        if (!response.IsValid || !response.Found)
        {
            _logger.LogWarning("Document with id {Id} not found in ElasticSearch.", id);
            return null;
        }

        return MapToDomainModel(response.Source);
    }

    public virtual async Task<TDomainModel?> GetByIdAsync(TDomainId id, CancellationToken cancellationToken = default)
    {
        var response = await _elasticClient.GetAsync<TEntityModel>(id!.ToString(), g => g.Index(IndexName), cancellationToken);

        if (!response.IsValid || !response.Found)
        {
            _logger.LogWarning("Document with id {Id} not found in ElasticSearch.", id);
            return null;
        }

        return MapToDomainModel(response.Source);
    }
}
