using RealEstateAPI.Application.DTOs;

namespace RealEstateAPI.Application.Interfaces;

/// <summary>
/// Service contract for property business logic operations.
/// </summary>
public interface IPropertyService
{
    Task<IEnumerable<PropertyDto>> GetAllPropertiesAsync();
    Task<PropertyDetailDto?> GetPropertyByIdAsync(string id);
    Task<PagedResponse<PropertyDto>> GetFilteredPropertiesAsync(PropertyFilterDto filter);
    Task<PropertyDetailDto?> GetPropertyBySlugAsync(string slug);
}
