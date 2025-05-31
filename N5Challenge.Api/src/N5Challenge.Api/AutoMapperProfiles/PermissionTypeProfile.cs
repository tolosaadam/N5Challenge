using AutoMapper;

namespace N5Challenge.Api.AutoMapperProfiles;

public class PermissionTypeProfile : Profile
{
    public PermissionTypeProfile()
    {
        _ = CreateMap<Requests.Permission.PermissionCreateRequest, Domain.PermissionType>();

        _ = CreateMap<Domain.PermissionType, Infraestructure.SQL.Entities.PermissionTypeDB>()
            .ReverseMap();

        _ = CreateMap<Domain.PermissionType, Responses.Permission.PermissionTypeResponse>();
    }
}
