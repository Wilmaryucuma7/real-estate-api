using RealEstateAPI.Application.DTOs;

namespace RealEstateAPI.Application.Interfaces;

/// <summary>
/// Service contract for property business operations.
/// </summary>
public interface IPropertyService
{
    Task<IEnumerable<PropertyDto>> GetAllPropertiesAsync();
    Task<PropertyDetailDto?> GetPropertyByIdAsync(string id);
    Task<PagedResponse<PropertyDto>> GetFilteredPropertiesAsync(PropertyFilterDto filter);
}
