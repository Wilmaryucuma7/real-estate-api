using RealEstateAPI.Application.DTOs;
using RealEstateAPI.Domain.Entities;

namespace RealEstateAPI.Application.Interfaces;

/// <summary>
/// Repository interface for property data access operations.
/// </summary>
public interface IPropertyRepository
{
    Task<IEnumerable<Property>> GetAllAsync();
    Task<Property?> GetByIdAsync(string id);
    Task<IEnumerable<Property>> GetFilteredAsync(PropertyFilterDto filter);
}
