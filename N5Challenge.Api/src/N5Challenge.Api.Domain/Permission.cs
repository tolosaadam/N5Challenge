using System.Reflection.Metadata;

namespace N5Challenge.Api.Domain;

public class Permission : DomainModel<int>
{
    public string? EmployeeFirstName { get; set; }
    public string? EmployeeLastName { get; set; }
    public DateTime? Date { get; set; }

    public int PermissionTypeId { get; set; }
}
