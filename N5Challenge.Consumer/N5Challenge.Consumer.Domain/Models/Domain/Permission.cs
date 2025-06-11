using N5Challenge.Consumer.Domain.Models.Indexables;
using N5Challenge.Consumer.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Consumer.Domain.Models.Domain;

public class Permission : IDomainEntity<int>
{
    public int Id { get; set; }
    public string? EmployeeFirstName { get; set; }
    public string? EmployeeLastName { get; set; }
    public DateTime Date { get; set; }
    public int PermissionTypeId { get; set; }
}
