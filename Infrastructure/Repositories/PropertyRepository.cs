using MongoDB.Bson;
using MongoDB.Driver;
using RealEstateAPI.Application.DTOs;
using RealEstateAPI.Application.Interfaces;
using RealEstateAPI.Domain.Entities;
using RealEstateAPI.Infrastructure.Data;

namespace RealEstateAPI.Infrastructure.Repositories;

/// <summary>
/// MongoDB implementation of property repository with pagination support.
/// </summary>
public sealed class PropertyRepository : IPropertyRepository
{
    private readonly IMongoCollection<Property> _collection;

    public PropertyRepository(MongoDbContext context)
    {
        _collection = context.Properties;
    }

    public async Task<IEnumerable<Property>> GetAllAsync()
    {
        return await _collection
            .Find(FilterDefinition<Property>.Empty)
            .ToListAsync();
    }

    public async Task<Property?> GetByIdAsync(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return null;

        return await _collection
            .Find(p => p.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Property>> GetFilteredAsync(PropertyFilterDto filter)
    {
        var filterBuilder = Builders<Property>.Filter;
        var filters = new List<FilterDefinition<Property>>();

        // Name filter (case-insensitive partial match)
        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            filters.Add(filterBuilder.Regex(
                p => p.Name, 
                new BsonRegularExpression(filter.Name, "i")));
        }

        // Address filter (case-insensitive partial match)
        if (!string.IsNullOrWhiteSpace(filter.Address))
        {
            filters.Add(filterBuilder.Regex(
                p => p.Address, 
                new BsonRegularExpression(filter.Address, "i")));
        }

        // Price range filter
        if (filter.MinPrice.HasValue)
        {
            filters.Add(filterBuilder.Gte(p => p.Price, filter.MinPrice.Value));
        }

        if (filter.MaxPrice.HasValue)
        {
            filters.Add(filterBuilder.Lte(p => p.Price, filter.MaxPrice.Value));
        }

        // Combine all filters
        var combinedFilter = filters.Count > 0
            ? filterBuilder.And(filters)
            : FilterDefinition<Property>.Empty;

        // Apply pagination
        var page = filter.Page ?? 1;
        var pageSize = filter.PageSize ?? 10;
        var skip = (page - 1) * pageSize;

        return await _collection
            .Find(combinedFilter)
            .Skip(skip)
            .Limit(pageSize)
            .ToListAsync();
    }
}
