using N5Challenge.Consumer.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Consumer.Domain.Models.Indexables;

public class IndexablePermission(string id) : IndexableEntity(id)
{
    public string? EmployeeFirstName { get; set; }
    public string? EmployeeLastName { get; set; }
    public DateTime Date { get; set; }
    public int PermissionTypeId { get; set; }
}
