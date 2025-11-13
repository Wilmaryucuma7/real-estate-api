using MongoDB.Driver;
using RealEstateAPI.Application.Interfaces;
using RealEstateAPI.Domain.Entities;
using RealEstateAPI.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace RealEstateAPI.Infrastructure.Repositories;

/// <summary>
/// MongoDB implementation of owner repository with connection error handling.
/// </summary>
public sealed class OwnerRepository : IOwnerRepository
{
    private readonly IMongoCollection<Owner> _collection;
    private readonly ILogger<OwnerRepository> _logger;

    public OwnerRepository(MongoDbContext context, ILogger<OwnerRepository> logger)
    {
        _collection = context.Owners;
        _logger = logger;
    }

    public async Task<Owner?> GetByIdAsync(string id)
    {
        try
        {
            return await _collection
                .Find(o => o.Id == id)
                .FirstOrDefaultAsync();
        }
        catch (MongoException ex)
        {
            _logger.LogError(ex, "MongoDB connection error while retrieving owner {OwnerId}", id);
            throw new InvalidOperationException("Database connection failed. Please ensure MongoDB is running.", ex);
        }
        catch (TimeoutException ex)
        {
            _logger.LogError(ex, "MongoDB timeout while retrieving owner {OwnerId}", id);
            throw new InvalidOperationException("Database operation timed out. Please try again later.", ex);
        }
    }

    public async Task<IEnumerable<Owner>> GetByIdsAsync(IEnumerable<string> ids)
    {
        try
        {
            var filter = Builders<Owner>.Filter.In(o => o.Id, ids);
            return await _collection
                .Find(filter)
                .ToListAsync();
        }
        catch (MongoException ex)
        {
            _logger.LogError(ex, "MongoDB connection error while retrieving multiple owners");
            throw new InvalidOperationException("Database connection failed. Please ensure MongoDB is running.", ex);
        }
        catch (TimeoutException ex)
        {
            _logger.LogError(ex, "MongoDB timeout while retrieving multiple owners");
            throw new InvalidOperationException("Database operation timed out. Please try again later.", ex);
        }
    }

    public async Task<(IEnumerable<Owner> Owners, int TotalCount)> GetAllAsync(int page, int pageSize)
    {
        try
        {
            var filter = FilterDefinition<Owner>.Empty;

            // Get total count
            var totalCount = (int)await _collection.CountDocumentsAsync(filter);

            // Apply pagination
            var skip = (page - 1) * pageSize;
            var owners = await _collection
                .Find(filter)
                .Skip(skip)
                .Limit(pageSize)
                .ToListAsync();

            return (owners, totalCount);
        }
        catch (MongoException ex)
        {
            _logger.LogError(ex, "MongoDB connection error while retrieving all owners");
            throw new InvalidOperationException("Database connection failed. Please ensure MongoDB is running.", ex);
        }
        catch (TimeoutException ex)
        {
            _logger.LogError(ex, "MongoDB timeout while retrieving all owners");
            throw new InvalidOperationException("Database operation timed out. Please try again later.", ex);
        }
    }
}
