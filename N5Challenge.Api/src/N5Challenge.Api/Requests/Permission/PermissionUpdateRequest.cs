namespace N5Challenge.Api.Requests.Permission;

public record PermissionUpdateRequest(
    string? EmployeeFirstName,
    string? EmployeeLastName,
    int? PermissionTypeId,
    DateTime? Date
    );
