using AutoMapper;
using N5Challenge.Api.Infraestructure.Entities;

namespace N5Challenge.Api.AutoMapperProfiles;

public class PermissionTypeProfile : Profile
{
    public PermissionTypeProfile()
    {
        _ = CreateMap<Domain.PermissionType, PermissionTypeDB>()
            .ReverseMap();

        _ = CreateMap<Domain.PermissionType,
            Responses.PermissionType.PermissionTypeResponse>();
    }
}
