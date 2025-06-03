using AutoMapper;
using N5Challenge.Api.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Infraestructure.SQL;

public class PermissionTypeRepository(AppDbContext context, IMapper autoMapper)
    : EntityRepository<Domain.PermissionType, Entities.PermissionTypeDB, int>(context, autoMapper), IPermissionTypeRepository
{
}
