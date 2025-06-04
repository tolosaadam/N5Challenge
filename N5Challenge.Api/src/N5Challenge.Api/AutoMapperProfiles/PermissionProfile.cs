using AutoMapper;
using N5Challenge.Api.Application.Models;

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

        _ = CreateMap<Application.Permission.Commands.Update.UpdatePermissionCommand, Domain.Permission>()
            .ForMember(dest => dest.EmployeeFirstName,
                opt => opt.Condition((src, dest, srcMember, destMember) => !string.IsNullOrEmpty(srcMember)))
            .ForMember(dest => dest.EmployeeLastName,
                opt => opt.Condition((src, dest, srcMember, destMember) => !string.IsNullOrEmpty(srcMember)))
            .ForMember(dest => dest.Date,
                opt => opt.Condition((src, dest, srcMember, destMember) => srcMember != default(DateTime)))
            .ForMember(dest => dest.PermissionTypeId,
                opt => opt.Condition((src, dest, srcMember, destMember) => srcMember != 0));

        _ = CreateMap<Domain.Permission,
            Infraestructure.SQL.Entities.PermissionDB>()
            .ReverseMap();

        _ = CreateMap<Domain.Permission,
            IndexablePermission>()
            .ForMember(src => src.Id, opt => opt.MapFrom(opt => opt.Id.ToString()));

        _ = CreateMap<(Domain.Permission permission, int id), IndexablePermission>()
            .ConstructUsing(src => new IndexablePermission(src.id.ToString())
            {
                EmployeeFirstName = src.permission.EmployeeFirstName,
                EmployeeLastName = src.permission.EmployeeLastName,
                Date = src.permission.Date,
                PermissionTypeId = src.permission.PermissionTypeId
            });

        _ = CreateMap<Domain.Permission,
            Responses.Permission.PermissionResponse>();
    }
}
