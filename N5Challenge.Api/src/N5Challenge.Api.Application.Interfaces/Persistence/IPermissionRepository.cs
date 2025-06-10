using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Application.Interfaces.Persistence;

public interface IPermissionRepository : 
    IReadRepository<Domain.Permission, int>,
    IWriteRepository<Domain.Permission, int>,
    IEfRepository
{
    Task<IEnumerable<Domain.Permission>> GetAllAsync(bool includeType, CancellationToken cancellationToken = default);
}
