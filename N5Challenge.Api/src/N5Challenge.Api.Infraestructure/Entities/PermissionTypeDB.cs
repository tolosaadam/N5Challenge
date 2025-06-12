using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Infraestructure.Entities;

public class PermissionTypeDB : Entity<int>
{
    public string? Description { get; set; }

    public ICollection<PermissionDB>? Permissions { get; set; }
}
