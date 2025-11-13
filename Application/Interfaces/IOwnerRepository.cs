using RealEstateAPI.Domain.Entities;

namespace RealEstateAPI.Application.Interfaces;

/// <summary>
/// Repository contract for owner data access operations.
/// </summary>
public interface IOwnerRepository
{
    Task<Owner?> GetByIdAsync(string id);
    Task<IEnumerable<Owner>> GetByIdsAsync(IEnumerable<string> ids);
}
