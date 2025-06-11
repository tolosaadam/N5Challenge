using AutoMapper;
using N5Challenge.Api.Infraestructure.Entities;
using N5Challenge.Common.Infraestructure.Indexables;

namespace N5Challenge.Api.AutoMapperProfiles;

public class PermissionProfile : Profile
{
    public PermissionProfile()
    {
        #region Create

        _ = CreateMap<Requests.Permission.PermissionCreateRequest,
            Application.Permission.Commands.Create.CreatePermissionCommand>()
            .ConstructUsing(src =>
            new Application.Permission.Commands.Create.CreatePermissionCommand(
                src.EmployeeFirstName,
                src.EmployeeLastName,
                src.PermissionTypeId));

        _ = CreateMap<Application.Permission.Commands.Create.CreatePermissionCommand,
            Domain.Permission>()
            .ForMember(dest => dest.Type, opt => opt.Ignore());

        #endregion

        #region Update

        _ = CreateMap<(Requests.Permission.PermissionUpdateRequest request, int id),
            Application.Permission.Commands.Update.UpdatePermissionCommand>()
            .ConstructUsing(src =>
            new Application.Permission.Commands.Update.UpdatePermissionCommand(
                src.id,
                src.request.EmployeeFirstName,
                src.request.EmployeeLastName,
                src.request.PermissionTypeId,
                src.request.Date));

        _ = CreateMap<Application.Permission.Commands.Update.UpdatePermissionCommand,
            Domain.Permission>()
            .ForMember(dest => dest.Type, opt => opt.Ignore());

        #endregion

        #region UpdatePartial

        _ = CreateMap<(Requests.Permission.PermissionUpdatePartialRequest request, int id),
            Application.Permission.Commands.UpdatePartial.UpdatePartialPermissionCommand>()
            .ConstructUsing(src =>
            new Application.Permission.Commands.UpdatePartial.UpdatePartialPermissionCommand(
                src.id,
                src.request.EmployeeFirstName,
                src.request.EmployeeLastName,
                src.request.PermissionTypeId,
                src.request.Date));

        _ = CreateMap<Application.Permission.Commands.UpdatePartial.UpdatePartialPermissionCommand,
            Domain.Permission>()
            .ForMember(dest => dest.EmployeeFirstName,
                opt => opt.Condition((src, dest, srcMember, destMember) => !string.IsNullOrEmpty(srcMember)))
            .ForMember(dest => dest.EmployeeLastName,
                opt => opt.Condition((src, dest, srcMember, destMember) => !string.IsNullOrEmpty(srcMember)))
            .ForMember(dest => dest.Date,
                opt => opt.Condition((src, dest, srcMember, destMember) => srcMember != default(DateTime)))
            .ForMember(dest => dest.PermissionTypeId,
                opt => opt.Condition((src, dest, srcMember, destMember) => srcMember != 0))
            .ForMember(dest => dest.Type, opt => opt.Ignore());

        #endregion

        #region Repository

        _ = CreateMap<Domain.Permission,
            PermissionDB>()
            .ForMember(dest => dest.Type, opt => opt.Ignore());

        _ = CreateMap<PermissionDB,
           Domain.Permission>();

        #endregion

        #region ElasticSearch

        _ = CreateMap<Domain.Permission, IndexablePermission>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.EmployeeFirstName, opt => opt.MapFrom(src => src.EmployeeFirstName))
            .ForMember(dest => dest.EmployeeLastName, opt => opt.MapFrom(src => src.EmployeeLastName))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.PermissionTypeId, opt => opt.MapFrom(src => src.PermissionTypeId));

        //_ = CreateMap<(Domain.Permission permission, int id), IndexablePermission>()
        //    .ConstructUsing(src => new IndexablePermission(src.id.ToString())
        //    {
        //        EmployeeFirstName = src.permission.EmployeeFirstName,
        //        EmployeeLastName = src.permission.EmployeeLastName,
        //        Date = src.permission.Date,
        //        PermissionTypeId = src.permission.PermissionTypeId
        //    });

        #endregion

        #region Responses

        _ = CreateMap<Domain.Permission,
            Responses.Permission.PermissionResponse>();

        #endregion
    }
}
