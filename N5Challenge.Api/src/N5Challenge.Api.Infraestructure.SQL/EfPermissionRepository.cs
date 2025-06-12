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

public class EfPermissionRepository(
    AppDbContext context,
    IMapper autoMapper)
    : EfRepository<Domain.Permission, PermissionDB, int>(autoMapper), IEfPermissionRepository
{
    protected override DbSet<PermissionDB> DbSet => context.Set<PermissionDB>();

    public async Task<IEnumerable<Domain.Permission>> GetAllAsync(bool includeType, CancellationToken cancellationToken = default)
    {
        return MapToDomainModel(
            await DbSet
            .AsNoTracking()
            .Include(x => x.Type)
            .ToListAsync(cancellationToken)
        );
    }
}
