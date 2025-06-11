using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Common.Infraestructure.Indexables;

public class IndexablePermission : IndexableEntity
{
    public string? EmployeeFirstName { get; set; }
    public string? EmployeeLastName { get; set; }
    public DateTime Date { get; set; }
    public int PermissionTypeId { get; set; }
}
