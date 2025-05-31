using AutoMapper;
namespace N5Challenge.Api.AutoMapperProfiles;

public class PermissionProfile : Profile
{
    public PermissionProfile()
    {
        _ = CreateMap<Requests.Permission.PermissionCreateRequest,
            Application.Permission.Commands.Create.CreatePermissionCommand>();

        _ = CreateMap<(Requests.Permission.PermissionUpdateRequest request, int id),
            Application.Permission.Commands.Update.UpdatePermissionCommand>()
            .ForMember(src => src.PermissionTypeId, opt => opt.MapFrom(s => s.request.PermissionTypeId))
            .ForMember(src => src.EmployeeFirstName, opt => opt.MapFrom(s => s.request.EmployeeFirstName))
            .ForMember(src => src.EmployeeLastName, opt => opt.MapFrom(s => s.request.EmployeeLastName))
            .ForMember(src => src.Date, opt => opt.MapFrom(s => s.request.Date))
            .ForMember(src => src.Id, opt => opt.MapFrom(s => s.id));

        _ = CreateMap<Application.Permission.Commands.Create.CreatePermissionCommand,
            Domain.Permission>();

        _ = CreateMap<Domain.Permission,
            Infraestructure.SQL.Entities.PermissionDB>()
            .ReverseMap();

        _ = CreateMap<Domain.Permission,
            Responses.Permission.PermissionResponse>();
    }
}
