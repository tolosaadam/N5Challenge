using AutoMapper;

namespace N5Challenge.Api.AutoMapperProfiles;

public class PermissionProfile : Profile
{
    public PermissionProfile()
    {
        _ = CreateMap<Requests.Permission.PermissionCreateRequest,
            Application.Permission.Commands.Create.CreatePermissionCommand>()
            .ConstructUsing(src =>
            new Application.Permission.Commands.Create.CreatePermissionCommand(
                src.EmployeeFirstName,
                src.EmployeeLastName,
                src.PermissionTypeId));

        _ = CreateMap<(Requests.Permission.PermissionUpdateRequest request, int id),
            Application.Permission.Commands.Update.UpdatePermissionCommand>()
            .ConstructUsing(src =>
            new Application.Permission.Commands.Update.UpdatePermissionCommand(
                src.id,
                src.request.EmployeeFirstName,
                src.request.EmployeeLastName,
                src.request.PermissionTypeId,
                src.request.Date));

        _ = CreateMap<Application.Permission.Commands.Create.CreatePermissionCommand,
            Domain.Permission>();

        _ = CreateMap<Application.Permission.Commands.Update.UpdatePermissionCommand,
            Domain.Permission>();

        _ = CreateMap<Domain.Permission,
            Infraestructure.SQL.Entities.PermissionDB>()
            .ReverseMap();

        _ = CreateMap<Domain.Permission,
            Responses.Permission.PermissionResponse>();
    }
}
