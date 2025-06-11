using AutoMapper;
using Microsoft.Extensions.Logging;
using N5Challenge.Api.Application.Interfaces.Persistence;
using N5Challenge.Api.Domain;
using N5Challenge.Common.Infraestructure;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace N5Challenge.Api.Infraestructure;

public abstract class Repository<TDomainModel, TDomainId, TEntityModel, TEntityId>(
    IMapper autoMapper)
    where TEntityModel : class, IEntity<TEntityId>
    where TDomainModel : class, IDomainEntity<TDomainId>
{
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

public abstract class Repository<TDomainModel, TEntityModel, TId>(
    IMapper autoMapper)
    : Repository<TDomainModel, TId, TEntityModel, TId>(autoMapper)
    where TEntityModel : class, IEntity<TId>
    where TDomainModel : class, IDomainEntity<TId>
{
}
