using AutoMapper;

namespace N5Challenge.Api.AutoMapperProfiles;

public class PermissionTypeProfile : Profile
{
    public PermissionTypeProfile()
    {
        _ = CreateMap<Requests.Permission.PermissionRequest, Domain.PermissionType>();
        _ = CreateMap<Domain.PermissionType, Responses.Permission.PermissionTypeResponse>();
    }
}
