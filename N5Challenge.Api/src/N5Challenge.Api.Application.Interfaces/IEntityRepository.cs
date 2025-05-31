using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Application.Interfaces;

public interface IEntityRepository<TDomainModel, TId> : IRepository
{
    void Add(TDomainModel entity);
    Task AddAsync(TDomainModel entity, CancellationToken cancellationToken = default);
    IEnumerable<TDomainModel> GetAll();
    Task<IEnumerable<TDomainModel>> GetAllAsync(CancellationToken cancellationToken = default);
    TDomainModel? GetById(TId id);
    Task<TDomainModel?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    void Update(TDomainModel entity);
    void Delete(TDomainModel entity);
}