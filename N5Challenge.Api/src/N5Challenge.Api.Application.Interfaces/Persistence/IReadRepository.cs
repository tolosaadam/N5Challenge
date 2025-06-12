using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Application.Interfaces.Persistence;

public interface IReadRepository<TDomainModel, TDomainId>
{
    IEnumerable<TDomainModel> GetAll();
    Task<IEnumerable<TDomainModel>> GetAllAsync(CancellationToken cancellationToken = default);
    TDomainModel? GetById(TDomainId id);
    Task<TDomainModel?> GetByIdAsync(TDomainId id, CancellationToken cancellationToken = default);
}
