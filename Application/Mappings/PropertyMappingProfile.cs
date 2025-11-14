using AutoMapper;
using RealEstateAPI.Application.DTOs;
using RealEstateAPI.Domain.Entities;

namespace RealEstateAPI.Application.Mappings;

/// <summary>
/// AutoMapper profile for property-related mappings.
/// </summary>
public sealed class PropertyMappingProfile : Profile
{
    public PropertyMappingProfile()
    {
        // Property -> PropertyDto (for list views - without Id)
        CreateMap<Property, PropertyDto>()
            .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Slug))
            .ForMember(dest => dest.IdOwner, opt => opt.MapFrom(src => src.OwnerId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src =>
                src.Images != null && src.Images.Any(img => img.Enabled)
                    ? src.Images.First(img => img.Enabled).File
                    : string.Empty));

        // Property -> PropertyDetailDto (for detail views with Id)
        CreateMap<Property, PropertyDetailDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Slug))
            .ForMember(dest => dest.IdOwner, opt => opt.MapFrom(src => src.OwnerId));

        // Owner -> OwnerDto
        CreateMap<Owner, OwnerDto>()
            .ForMember(dest => dest.IdOwner, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo))
            .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.Birthday));

        // PropertyImage -> PropertyImageDto
        CreateMap<PropertyImage, PropertyImageDto>()
            .ForMember(dest => dest.IdPropertyImage, opt => opt.MapFrom(src => src.IdPropertyImage))
            .ForMember(dest => dest.File, opt => opt.MapFrom(src => src.File))
            .ForMember(dest => dest.Enabled, opt => opt.MapFrom(src => src.Enabled));

        // PropertyTrace -> PropertyTraceDto
        CreateMap<PropertyTrace, PropertyTraceDto>()
            .ForMember(dest => dest.IdPropertyTrace, opt => opt.MapFrom(src => src.IdPropertyTrace))
            .ForMember(dest => dest.DateSale, opt => opt.MapFrom(src => src.DateSale))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value))
            .ForMember(dest => dest.Tax, opt => opt.MapFrom(src => src.Tax));
    }
}
