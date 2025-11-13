using RealEstateAPI.Application.DTOs;

namespace RealEstateAPI.Application.Interfaces;

/// <summary>
/// Service interface for property business operations.
/// </summary>
public interface IPropertyService
{
    Task<IEnumerable<PropertyDto>> GetAllPropertiesAsync();
    Task<PropertyDetailDto?> GetPropertyByIdAsync(string id);
    Task<IEnumerable<PropertyDto>> GetFilteredPropertiesAsync(PropertyFilterDto filter);
}
