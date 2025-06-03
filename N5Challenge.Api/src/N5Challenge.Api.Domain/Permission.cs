namespace N5Challenge.Api.Domain;

public class Permission : Entity<int>
{
    public string? EmployeeFirstName { get; set; }
    public string? EmployeeLastName { get; set; }
    public DateTime? Date { get; set; }
    public int PermissionTypeId { get; set; }  
}
