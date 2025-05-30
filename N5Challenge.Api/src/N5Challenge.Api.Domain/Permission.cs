using System.Reflection.Metadata;

namespace N5Challenge.Api.Domain;

public class Permission
{
    public int Id { get; set; }
    public string? EmployeeFirstName { get; set; }
    public string? EmployeeLastName { get; set; }
    public PermissionType? Type { get; set; }
    public DateTime? Date { get; set; }
}
