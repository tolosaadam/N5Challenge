using N5Challenge.Api.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Infraestructure.SQL.Entities;

public class PermissionDB : Entity<int>
{
    public string? EmployeeFirstName { get; set; }
    public string? EmployeeLastName { get; set; }
    public DateTime? Date { get; set; }

    public int PermissionTypeId { get; set; }
    public PermissionTypeDB? Type { get; set; }
}
