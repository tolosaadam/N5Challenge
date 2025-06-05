using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Domain;

public class PermissionType : Entity<int>
{
    public string? Description { get; set; }
}
