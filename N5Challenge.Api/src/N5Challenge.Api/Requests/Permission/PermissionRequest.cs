using N5Challenge.Api.Responses.Permission;

namespace N5Challenge.Api.Requests.Permission;

public record PermissionRequest(
    string EmployeeFirstName,
    string EmployeeLastName,
    PermissionTypeResponse Type,
    DateTime Date
    );
