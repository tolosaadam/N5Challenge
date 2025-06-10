using AutoMapper;
using Microsoft.EntityFrameworkCore;
using N5Challenge.Api.Application.Interfaces.Persistence;
using N5Challenge.Api.Infraestructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace N5Challenge.Api.Infraestructure.SQL;

public class PermissionRepository(AppDbContext context, IMapper autoMapper)
    : EfRepository<Domain.Permission, PermissionDB, int>(context, autoMapper), IPermissionRepository
{
    public async Task<IEnumerable<Domain.Permission>> GetAllAsync(bool includeType, CancellationToken cancellationToken = default)
    {
        return MapToDomainModel(
            await _dbSet
            .AsNoTracking()
            .Include(x => x.Type)
            .ToListAsync(cancellationToken)
        );
    }
}
