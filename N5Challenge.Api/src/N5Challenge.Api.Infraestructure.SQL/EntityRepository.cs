using AutoMapper;
using Microsoft.EntityFrameworkCore;
using N5Challenge.Api.Application.Interfaces.Persistence;
using N5Challenge.Api.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Infraestructure.SQL;

public class EntityRepository<TDomainModel, TEntityModel, TId>(
    AppDbContext context,
    IMapper autoMapper) : Repository(context), IEntityRepository<TDomainModel, TId>
    where TDomainModel : class
    where TEntityModel : class, IEntity<TId>
{
    protected readonly DbSet<TEntityModel> _dbSet = context.Set<TEntityModel>();
    private readonly IMapper _autoMapper = autoMapper;

    public virtual Func<TId> Add(TDomainModel domainModel)
    {
        var entityModel = MapToEntityModel(domainModel);

        _dbSet.Add(entityModel);

        return () => entityModel.Id;
    }    

    public virtual async Task<Func<TId>> AddAsync(TDomainModel domainModel, CancellationToken cancellationToken = default)
    {
        var entityModel = MapToEntityModel(domainModel);

        await _dbSet.AddAsync(entityModel, cancellationToken);

        return () => entityModel.Id;
    }
        
    public IEnumerable<TDomainModel> GetAll() =>
        MapToDomainModel(
            _dbSet
            .ToList()
        );

    public virtual async Task<IEnumerable<TDomainModel>> GetAllAsync(CancellationToken cancellationToken = default) =>
        MapToDomainModel(
            await _dbSet
            .AsNoTracking()
            .ToListAsync(cancellationToken)
        );

    public virtual TDomainModel? GetById(TId id) =>
        MapToDomainModel(
            _dbSet
            .AsNoTracking()
            .FirstOrDefault(x => x.Id!.Equals(id))
        );

    public virtual async Task<TDomainModel?> GetByIdAsync(TId id, CancellationToken cancellationToken = default) =>
        MapToDomainModel(
            await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id!.Equals(id), cancellationToken: cancellationToken)
        );

    public virtual void Delete(TDomainModel entity) =>
        _dbSet
        .Remove(MapToEntityModel(entity));


    public virtual void Update(TDomainModel entity) =>
        _dbSet
        .Update(MapToEntityModel(entity));

    protected virtual TDomainModel? MapToDomainModel(TEntityModel? entityModel) =>
        entityModel is null ? null : _autoMapper.Map<TEntityModel?, TDomainModel?>(entityModel);

    protected virtual TEntityModel MapToEntityModel(TDomainModel domainModel) =>
        _autoMapper.Map<TDomainModel, TEntityModel>(domainModel);

    protected virtual IEnumerable<TDomainModel> MapToDomainModel(IEnumerable<TEntityModel> entityModel) =>
        entityModel is null ? [] : _autoMapper.Map<IEnumerable<TDomainModel>>(entityModel);

    protected virtual IEnumerable<TEntityModel> MapToEntityModel(IEnumerable<TDomainModel> domainModel) =>
        _autoMapper.Map<IEnumerable<TDomainModel>, IEnumerable<TEntityModel>>(domainModel);
}
