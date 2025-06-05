using N5Challenge.Api.Responses.PermissionType;

namespace N5Challenge.Api.Responses.Permission;
public record PermissionResponse(
    int Id,
    string EmployeeFirstName,
    string EmployeeLastName,
    PermissionTypeResponse Type,
    DateTime Date
);
