using AutoMapper;
using Microsoft.EntityFrameworkCore;
using N5Challenge.Api.Application.Interfaces.Persistence;
using N5Challenge.Api.Domain;
using N5Challenge.Common.Infraestructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Infraestructure.SQL;

public abstract class EfRepository<TDomainModel, TEntityModel, TId>(
    IMapper autoMapper) : Repository<TDomainModel, TEntityModel, TId>(autoMapper),
    IEfRepository,
    IReadRepository<TDomainModel, TId>,
    IWriteRepository<TDomainModel, TId>
    where TEntityModel : class, IEntity<TId>
    where TDomainModel : class, IDomainEntity<TId>
{
    protected abstract DbSet<TEntityModel> DbSet { get; }

    public virtual Func<TId> Add(TDomainModel domainModel)
    {
        var entityModel = MapToEntityModel(domainModel);

        DbSet.Add(entityModel);

        return () => entityModel.Id;
    }

    public virtual async Task<Func<TId>> AddAsync(TDomainModel domainModel, CancellationToken cancellationToken = default)
    {
        var entityModel = MapToEntityModel(domainModel);

        await DbSet.AddAsync(entityModel, cancellationToken);

        return () => entityModel.Id;
    }

    public virtual IEnumerable<TDomainModel> GetAll() =>
        MapToDomainModel(
            DbSet
            .ToList()
        );

    public virtual async Task<IEnumerable<TDomainModel>> GetAllAsync(CancellationToken cancellationToken = default) =>
        MapToDomainModel(
            await DbSet
            .AsNoTracking()
            .ToListAsync(cancellationToken)
        );

    public virtual TDomainModel? GetById(TId id) =>
        MapToDomainModel(
            DbSet
            .AsNoTracking()
            .FirstOrDefault(x => x.Id!.Equals(id))
        );

    public virtual async Task<TDomainModel?> GetByIdAsync(TId id, CancellationToken cancellationToken = default) =>
        MapToDomainModel(
            await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id!.Equals(id), cancellationToken: cancellationToken)
        );

    public virtual void Delete(TDomainModel domainModel) =>
        DbSet
        .Remove(MapToEntityModel(domainModel));


    public virtual TDomainModel? Update(TDomainModel domainModel)
    {
        var entityModel = MapToEntityModel(domainModel);

        var result = DbSet.Update(entityModel);

        return MapToDomainModel(result.Entity);
    }
}
