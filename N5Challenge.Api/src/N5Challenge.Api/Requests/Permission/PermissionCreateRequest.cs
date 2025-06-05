using N5Challenge.Api.Responses.Permission;

namespace N5Challenge.Api.Requests.Permission;

public record PermissionCreateRequest(
    string EmployeeFirstName,
    string EmployeeLastName,
    int PermissionTypeId
    );
