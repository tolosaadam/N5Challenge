using AutoMapper;

namespace N5Challenge.Api.AutoMapperProfiles;

public class PermissionProfile : Profile
{
    public PermissionProfile()
    {
        _ = CreateMap<Requests.Permission.PermissionRequest, Domain.Permission > ();
        _ = CreateMap<Domain.Permission, Responses.Permission.PermissionResponse>();
    }
}
