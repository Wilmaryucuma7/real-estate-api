using MongoDB.Bson;
using MongoDB.Driver;
using RealEstateAPI.Application.DTOs;
using RealEstateAPI.Application.Interfaces;
using RealEstateAPI.Domain.Entities;
using RealEstateAPI.Infrastructure.Data;
using System.Text.RegularExpressions;

namespace RealEstateAPI.Infrastructure.Repositories;

/// <summary>
/// MongoDB implementation of property repository with optimized queries.
/// </summary>
public class PropertyRepository : IPropertyRepository
{
    private readonly IMongoCollection<Property> _collection;
    private readonly ILogger<PropertyRepository> _logger;

    public PropertyRepository(MongoDbContext context, ILogger<PropertyRepository> logger)
    {
        _collection = context.Properties;
        _logger = logger;
        EnsureIndexesAsync().GetAwaiter().GetResult();
    }

    /// <summary>
    /// Ensures required indexes exist for optimal query performance.
    /// </summary>
    private async Task EnsureIndexesAsync()
    {
        var indexKeys = Builders<Property>.IndexKeys;
        
        // Create slug index for fast slug-based lookups
        var slugIndexModel = new CreateIndexModel<Property>(
            indexKeys.Ascending(p => p.Slug),
            new CreateIndexOptions { Unique = true, Name = "slug_unique" });

        // Create ownerId index for fast owner-based lookups
        var ownerIdIndexModel = new CreateIndexModel<Property>(
            indexKeys.Ascending(p => p.OwnerId),
            new CreateIndexOptions { Name = "ownerId_index" });

        try
        {
            await _collection.Indexes.CreateManyAsync(new[] { slugIndexModel, ownerIdIndexModel });
        }
        catch (MongoCommandException)
        {
            // Indexes already exist, ignore
        }
    }

    public async Task<IEnumerable<Property>> GetAllAsync()
    {
        // Only fetch necessary fields to reduce memory usage
        var projection = Builders<Property>.Projection
            .Exclude(p => p.Traces); // Don't load traces for list views

        return await _collection
            .Find(FilterDefinition<Property>.Empty)
            .Project<Property>(projection)
            .ToListAsync();
    }

    public async Task<Property?> GetByIdAsync(string id)
    {
        return await _collection.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Property?> GetBySlugAsync(string slug)
    {
        // Optimized: Direct MongoDB query using slug index
        return await _collection
            .Find(p => p.Slug == slug)
            .FirstOrDefaultAsync();
    }

    public async Task<(IEnumerable<Property> Properties, int TotalCount)> GetByOwnerIdAsync(
        string ownerId,
        int page,
        int pageSize)
    {
        var filter = Builders<Property>.Filter.Eq(p => p.OwnerId, ownerId);
        
        // Count total for pagination
        var totalCount = await _collection.CountDocumentsAsync(filter);

        // Exclude traces from list views for performance
        var projection = Builders<Property>.Projection
            .Exclude(p => p.Traces);

        // Get paginated results using ownerId index
        var properties = await _collection
            .Find(filter)
            .Project<Property>(projection)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        return (properties, (int)totalCount);
    }

    public async Task<(IEnumerable<Property> Properties, int TotalCount)> GetFilteredAsync(PropertyFilterDto filter)
    {
        var filterBuilder = Builders<Property>.Filter;
        var filters = new List<FilterDefinition<Property>>();

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            filters.Add(filterBuilder.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression(filter.Name, "i")));
        }

        if (!string.IsNullOrWhiteSpace(filter.Address))
        {
            filters.Add(filterBuilder.Regex(p => p.Address, new MongoDB.Bson.BsonRegularExpression(filter.Address, "i")));
        }

        if (filter.MinPrice.HasValue)
        {
            filters.Add(filterBuilder.Gte(p => p.Price, filter.MinPrice.Value));
        }

        if (filter.MaxPrice.HasValue)
        {
            filters.Add(filterBuilder.Lte(p => p.Price, filter.MaxPrice.Value));
        }

        var combinedFilter = filters.Count > 0
            ? filterBuilder.And(filters)
            : FilterDefinition<Property>.Empty;

        // Count total for pagination
        var totalCount = await _collection.CountDocumentsAsync(combinedFilter);

        // Exclude traces from list views for performance
        var projection = Builders<Property>.Projection
            .Exclude(p => p.Traces);

        // Get paginated results
        var properties = await _collection
            .Find(combinedFilter)
            .Project<Property>(projection)
            .Skip(((filter.Page ?? 1) - 1) * (filter.PageSize ?? 10))
            .Limit(filter.PageSize ?? 10)
            .ToListAsync();

        return (properties, (int)totalCount);
    }
}
