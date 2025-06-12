using AutoMapper;
using N5Challenge.Api.Infraestructure.Entities;
using N5Challenge.Api.Infraestructure.Indexables;

namespace N5Challenge.Api.AutoMapperProfiles;

public class PermissionTypeProfile : Profile
{
    public PermissionTypeProfile()
    {
        _ = CreateMap<Domain.PermissionType, PermissionTypeDB>()
            .ReverseMap();

        _ = CreateMap<Domain.PermissionType,
            Responses.PermissionType.PermissionTypeResponse>();

        _ = CreateMap<Domain.PermissionType, IndexablePermissionType>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

        _ = CreateMap<IndexablePermissionType, Domain.PermissionType>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Convert.ToInt32(src.Id)))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
    }
}
