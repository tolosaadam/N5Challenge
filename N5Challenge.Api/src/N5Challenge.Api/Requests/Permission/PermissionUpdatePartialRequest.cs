namespace N5Challenge.Api.Requests.Permission;

public record PermissionUpdatePartialRequest(
    string? EmployeeFirstName,
    string? EmployeeLastName,
    int? PermissionTypeId,
    DateTime? Date
    );
