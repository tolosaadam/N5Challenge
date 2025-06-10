using AutoMapper;
using Microsoft.EntityFrameworkCore;
using N5Challenge.Api.Application.Interfaces.Persistence;
using N5Challenge.Api.Infraestructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Infraestructure.SQL;

public class PermissionTypeRepository(AppDbContext context, IMapper autoMapper)
    : EfRepository<Domain.PermissionType, PermissionTypeDB, int>(autoMapper), IPermissionTypeRepository
{
    protected override DbSet<PermissionTypeDB> DbSet => context.Set<PermissionTypeDB>();
}
