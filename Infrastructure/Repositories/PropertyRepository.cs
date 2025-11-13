using MongoDB.Bson;
using MongoDB.Driver;
using RealEstateAPI.Application.DTOs;
using RealEstateAPI.Application.Interfaces;
using RealEstateAPI.Domain.Entities;
using RealEstateAPI.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace RealEstateAPI.Infrastructure.Repositories;

/// <summary>
/// MongoDB implementation of property repository with pagination and connection error handling.
/// </summary>
public sealed class PropertyRepository : IPropertyRepository
{
    private readonly IMongoCollection<Property> _collection;
    private readonly ILogger<PropertyRepository> _logger;

    public PropertyRepository(MongoDbContext context, ILogger<PropertyRepository> logger)
    {
        _collection = context.Properties;
        _logger = logger;
    }

    public async Task<IEnumerable<Property>> GetAllAsync()
    {
        try
        {
            return await _collection
                .Find(FilterDefinition<Property>.Empty)
                .ToListAsync();
        }
        catch (MongoException ex)
        {
            _logger.LogError(ex, "MongoDB connection error while retrieving all properties");
            throw new InvalidOperationException("Database connection failed. Please ensure MongoDB is running.", ex);
        }
        catch (TimeoutException ex)
        {
            _logger.LogError(ex, "MongoDB timeout while retrieving all properties");
            throw new InvalidOperationException("Database operation timed out. Please try again later.", ex);
        }
    }

    public async Task<Property?> GetByIdAsync(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return null;

        try
        {
            return await _collection
                .Find(p => p.Id == id)
                .FirstOrDefaultAsync();
        }
        catch (MongoException ex)
        {
            _logger.LogError(ex, "MongoDB connection error while retrieving property {PropertyId}", id);
            throw new InvalidOperationException("Database connection failed. Please ensure MongoDB is running.", ex);
        }
        catch (TimeoutException ex)
        {
            _logger.LogError(ex, "MongoDB timeout while retrieving property {PropertyId}", id);
            throw new InvalidOperationException("Database operation timed out. Please try again later.", ex);
        }
    }

    public async Task<(IEnumerable<Property> Properties, int TotalCount)> GetFilteredAsync(PropertyFilterDto filter)
    {
        try
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

            // Get total count BEFORE pagination
            var totalCount = (int)await _collection.CountDocumentsAsync(combinedFilter);

            // Apply pagination
            var page = filter.Page ?? 1;
            var pageSize = filter.PageSize ?? 10;
            var skip = (page - 1) * pageSize;

            var properties = await _collection
                .Find(combinedFilter)
                .Skip(skip)
                .Limit(pageSize)
                .ToListAsync();

            return (properties, totalCount);
        }
        catch (MongoException ex)
        {
            _logger.LogError(ex, "MongoDB connection error while filtering properties");
            throw new InvalidOperationException("Database connection failed. Please ensure MongoDB is running.", ex);
        }
        catch (TimeoutException ex)
        {
            _logger.LogError(ex, "MongoDB timeout while filtering properties");
            throw new InvalidOperationException("Database operation timed out. Please try again later.", ex);
        }
    }
}
