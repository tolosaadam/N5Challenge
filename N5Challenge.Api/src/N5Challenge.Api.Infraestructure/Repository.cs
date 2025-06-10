using AutoMapper;
using N5Challenge.Api.Application.Interfaces.Persistence;
using N5Challenge.Api.Domain;
using N5Challenge.Api.Infraestructure.Entities;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace N5Challenge.Api.Infraestructure;

public abstract class Repository<TContext, TDomainModel, TEntityModel, TId>(
    TContext context,
    IMapper autoMapper) : IRepository
    where TEntityModel : class, IEntity<TId>
    where TDomainModel : class, IDomainEntity<TId>
{
    protected readonly TContext _context = context;
    private readonly IMapper _autoMapper = autoMapper;

    protected virtual TDomainModel? MapToDomainModel(TEntityModel? entityModel) =>
        entityModel is null ? null : _autoMapper.Map<TEntityModel?, TDomainModel?>(entityModel);

    protected virtual TEntityModel MapToEntityModel(TDomainModel domainModel) =>
        _autoMapper.Map<TDomainModel, TEntityModel>(domainModel);

    protected virtual IEnumerable<TDomainModel> MapToDomainModel(IEnumerable<TEntityModel> entityModel) =>
        entityModel is null ? [] : _autoMapper.Map<IEnumerable<TDomainModel>>(entityModel);

    protected virtual IEnumerable<TEntityModel> MapToEntityModel(IEnumerable<TDomainModel> domainModel) =>
        _autoMapper.Map<IEnumerable<TDomainModel>, IEnumerable<TEntityModel>>(domainModel);
}
