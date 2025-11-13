using RealEstateAPI.Application.DTOs;

namespace RealEstateAPI.Application.Interfaces;

/// <summary>
/// Service contract for owner business logic operations.
/// </summary>
public interface IOwnerService
{
    Task<PagedResponse<OwnerDto>> GetAllOwnersAsync(int page, int pageSize);
    Task<OwnerDto?> GetOwnerByIdAsync(string id);
    Task<PagedResponse<PropertyDto>> GetPropertiesByOwnerIdAsync(string ownerId, int page, int pageSize);
}
