using AutoMapper;
using Microsoft.EntityFrameworkCore;
using N5Challenge.Api.Application.Interfaces.Persistence;
using N5Challenge.Api.Domain;
using N5Challenge.Api.Infraestructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Infraestructure.SQL;

public class EfRepository<TDomainModel, TEntityModel, TId>(
    AppDbContext context,
    IMapper autoMapper) : Repository<AppDbContext, TDomainModel, TEntityModel, TId>(context, autoMapper),
    IReadRepository<TDomainModel, TId>,
    IWriteRepository<TDomainModel, TId>,
    IEfRepository
    where TEntityModel : class, IEntity<TId>
    where TDomainModel : class, IDomainEntity<TId>
{
    protected readonly DbSet<TEntityModel> _dbSet = context.Set<TEntityModel>();

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

    public virtual IEnumerable<TDomainModel> GetAll() =>
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

    public virtual void Delete(TDomainModel domainModel) =>
        _dbSet
        .Remove(MapToEntityModel(domainModel));


    public virtual TDomainModel? Update(TDomainModel domainModel)
    {
        var entityModel = MapToEntityModel(domainModel);

        var result = _dbSet.Update(entityModel);

        return MapToDomainModel(result.Entity);
    }
}
