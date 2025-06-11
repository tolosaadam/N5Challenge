using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Application.Interfaces.Persistence;

public interface IElasticPermissionRepository : IReadRepository<Domain.Permission, int>, IWriteRepository<Domain.Permission, int>
{
    Task<IEnumerable<Domain.Permission>> GetAllAsync(bool include = false, CancellationToken cancellationToken = default);
}
