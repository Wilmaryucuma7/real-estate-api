using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;
using RealEstateAPI.Infrastructure.Data;

namespace RealEstateAPI.Infrastructure.HealthChecks;

/// <summary>
/// Health check that verifies MongoDB collections exist and are accessible.
/// </summary>
public sealed class MongoDbCollectionHealthCheck : IHealthCheck
{
    private readonly MongoDbContext _context;
    private readonly ILogger<MongoDbCollectionHealthCheck> _logger;

    public MongoDbCollectionHealthCheck(
        MongoDbContext context,
        ILogger<MongoDbCollectionHealthCheck> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if Properties collection exists
            var propertiesExists = await CollectionExistsAsync("Properties", cancellationToken);
            
            // Check if Owners collection exists
            var ownersExists = await CollectionExistsAsync("Owners", cancellationToken);

            if (!propertiesExists || !ownersExists)
            {
                var missingCollections = new List<string>();
                if (!propertiesExists) missingCollections.Add("Properties");
                if (!ownersExists) missingCollections.Add("Owners");

                var errorMessage = $"Missing required collections: {string.Join(", ", missingCollections)}. " +
                                 "Please run the seed script: Database/seed-data.js";

                _logger.LogWarning("MongoDB health check failed: {ErrorMessage}", errorMessage);

                return HealthCheckResult.Unhealthy(
                    description: errorMessage,
                    data: new Dictionary<string, object>
                    {
                        { "propertiesExists", propertiesExists },
                        { "ownersExists", ownersExists },
                        { "missingCollections", missingCollections }
                    });
            }

            // Optionally check if collections have data
            var propertiesCount = await _context.Properties.CountDocumentsAsync(
                FilterDefinition<Domain.Entities.Property>.Empty,
                cancellationToken: cancellationToken);

            var ownersCount = await _context.Owners.CountDocumentsAsync(
                FilterDefinition<Domain.Entities.Owner>.Empty,
                cancellationToken: cancellationToken);

            _logger.LogInformation(
                "MongoDB health check passed. Properties: {PropertiesCount}, Owners: {OwnersCount}",
                propertiesCount,
                ownersCount);

            return HealthCheckResult.Healthy(
                description: "MongoDB is healthy with all required collections",
                data: new Dictionary<string, object>
                {
                    { "propertiesCount", propertiesCount },
                    { "ownersCount", ownersCount }
                });
        }
        catch (TimeoutException ex)
        {
            _logger.LogError(ex, "MongoDB health check timed out");
            return HealthCheckResult.Unhealthy(
                description: "MongoDB connection timed out",
                exception: ex);
        }
        catch (MongoException ex)
        {
            _logger.LogError(ex, "MongoDB health check failed due to connection error");
            return HealthCheckResult.Unhealthy(
                description: "Cannot connect to MongoDB. Ensure MongoDB is running.",
                exception: ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during MongoDB health check");
            return HealthCheckResult.Unhealthy(
                description: "Unexpected error checking MongoDB health",
                exception: ex);
        }
    }

    private async Task<bool> CollectionExistsAsync(string collectionName, CancellationToken cancellationToken)
    {
        try
        {
            var database = _context.Properties.Database;
            var collectionNames = await (await database.ListCollectionNamesAsync(cancellationToken: cancellationToken))
                .ToListAsync(cancellationToken);

            return collectionNames.Contains(collectionName);
        }
        catch
        {
            return false;
        }
    }
}
