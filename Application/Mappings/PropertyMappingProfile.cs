using AutoMapper;
using RealEstateAPI.Application.DTOs;
using RealEstateAPI.Domain.Entities;

namespace RealEstateAPI.Application.Mappings;

/// <summary>
/// AutoMapper profile for property-related mappings with owner reference.
/// </summary>
public sealed class PropertyMappingProfile : Profile
{
    public PropertyMappingProfile()
    {
        // Property -> PropertyDto (summary view)
        CreateMap<Property, PropertyDto>()
            .ForMember(dest => dest.IdOwner, opt => opt.MapFrom(src => src.OwnerId))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src =>
                src.Images.FirstOrDefault(img => img.Enabled)!.File ?? string.Empty));

        // Property -> PropertyDetailDto (detailed view)
        CreateMap<Property, PropertyDetailDto>()
            .ForMember(dest => dest.Owner, opt => opt.Ignore()); // Owner will be loaded separately

        // Owner -> OwnerDto
        CreateMap<Owner, OwnerDto>()
            .ForMember(dest => dest.IdOwner, opt => opt.MapFrom(src => src.Id));

        // PropertyImage -> PropertyImageDto
        CreateMap<PropertyImage, PropertyImageDto>();

        // PropertyTrace -> PropertyTraceDto
        CreateMap<PropertyTrace, PropertyTraceDto>();
    }
}
