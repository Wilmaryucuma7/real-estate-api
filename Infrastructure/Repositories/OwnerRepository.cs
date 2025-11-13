using MongoDB.Driver;
using RealEstateAPI.Application.Interfaces;
using RealEstateAPI.Domain.Entities;
using RealEstateAPI.Infrastructure.Data;

namespace RealEstateAPI.Infrastructure.Repositories;

/// <summary>
/// MongoDB implementation of owner repository.
/// </summary>
public sealed class OwnerRepository : IOwnerRepository
{
    private readonly IMongoCollection<Owner> _collection;

    public OwnerRepository(MongoDbContext context)
    {
        _collection = context.Owners;
    }

    public async Task<Owner?> GetByIdAsync(string id)
    {
        return await _collection
            .Find(o => o.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Owner>> GetByIdsAsync(IEnumerable<string> ids)
    {
        var filter = Builders<Owner>.Filter.In(o => o.Id, ids);
        return await _collection
            .Find(filter)
            .ToListAsync();
    }
}
