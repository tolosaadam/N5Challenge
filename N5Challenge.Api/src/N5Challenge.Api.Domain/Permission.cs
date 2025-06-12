namespace N5Challenge.Api.Domain;

public class Permission : DomainEntity<int>
{
    public string? EmployeeFirstName { get; set; }
    public string? EmployeeLastName { get; set; }
    public DateTime Date { get; set; }
    public int PermissionTypeId { get; set; }
    public PermissionType? Type { get; set; }
}
