using RealEstateAPI.Application.DTOs;
using RealEstateAPI.Domain.Entities;

namespace RealEstateAPI.Application.Interfaces;

/// <summary>
/// Repository contract for property data access operations.
/// </summary>
public interface IPropertyRepository
{
    Task<IEnumerable<Property>> GetAllAsync();
    Task<Property?> GetByIdAsync(string id);
    Task<(IEnumerable<Property> Properties, int TotalCount)> GetFilteredAsync(PropertyFilterDto filter);
}
