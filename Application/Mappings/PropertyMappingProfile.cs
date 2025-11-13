using AutoMapper;
using RealEstateAPI.Application.DTOs;
using RealEstateAPI.Domain.Entities;

namespace RealEstateAPI.Application.Mappings;

/// <summary>
/// AutoMapper profile for mapping between domain entities and DTOs.
/// </summary>
public class PropertyMappingProfile : Profile
{
    public PropertyMappingProfile()
    {
        // Property -> PropertyDto (with only first enabled image)
        CreateMap<Property, PropertyDto>()
            .ForMember(dest => dest.IdOwner, opt => opt.MapFrom(src => src.Owner.IdOwner))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => 
                src.Images.FirstOrDefault(img => img.Enabled)!.File ?? null));

        // Property -> PropertyDetailDto
        CreateMap<Property, PropertyDetailDto>();

        // Owner -> OwnerDto
        CreateMap<Owner, OwnerDto>();

        // PropertyImage -> PropertyImageDto
        CreateMap<PropertyImage, PropertyImageDto>();

        // PropertyTrace -> PropertyTraceDto
        CreateMap<PropertyTrace, PropertyTraceDto>();
    }
}
