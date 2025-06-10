using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Application.Interfaces.Persistence;

public interface IReadRepository<TDomainModel, TId> : IRepository
{
    IEnumerable<TDomainModel> GetAll();
    Task<IEnumerable<TDomainModel>> GetAllAsync(CancellationToken cancellationToken = default);
    TDomainModel? GetById(TId id);
    Task<TDomainModel?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
}
