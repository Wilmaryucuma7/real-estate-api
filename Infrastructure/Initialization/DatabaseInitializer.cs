using MongoDB.Driver;
using RealEstateAPI.Infrastructure.Data;

namespace RealEstateAPI.Infrastructure.Initialization;

/// <summary>
/// Service to validate and initialize MongoDB database on startup.
/// </summary>
public sealed class DatabaseInitializer
{
    private readonly MongoDbContext _context;
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(MongoDbContext context, ILogger<DatabaseInitializer> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Validates that required collections exist and are accessible.
    /// Throws exception if database is not properly configured.
    /// </summary>
    public async Task ValidateAndInitializeAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("?? Validating MongoDB database configuration...");

        try
        {
            var database = _context.Properties.Database;
            
            // List all collections
            var collectionNames = await (await database.ListCollectionNamesAsync(cancellationToken: cancellationToken))
                .ToListAsync(cancellationToken);

            var propertiesExists = collectionNames.Contains("Properties");
            var ownersExists = collectionNames.Contains("Owners");

            if (!propertiesExists || !ownersExists)
            {
                var missingCollections = new List<string>();
                if (!propertiesExists) missingCollections.Add("Properties");
                if (!ownersExists) missingCollections.Add("Owners");

                _logger.LogError(
                    "? Missing required collections: {MissingCollections}",
                    string.Join(", ", missingCollections));

                _logger.LogError("?? To fix this issue, run the seed script:");
                _logger.LogError("   mongosh < Database/seed-data.js");
                _logger.LogError("   OR in MongoDB Compass MONGOSH tab:");
                _logger.LogError("   use RealEstateDB");
                _logger.LogError("   load('Database/seed-data.js')");

                throw new InvalidOperationException(
                    $"Database initialization failed: Missing collections [{string.Join(", ", missingCollections)}]. " +
                    "Please run the seed script: mongosh < Database/seed-data.js");
            }

            // Check if collections have data
            var propertiesCount = await _context.Properties.CountDocumentsAsync(
                FilterDefinition<Domain.Entities.Property>.Empty,
                cancellationToken: cancellationToken);

            var ownersCount = await _context.Owners.CountDocumentsAsync(
                FilterDefinition<Domain.Entities.Owner>.Empty,
                cancellationToken: cancellationToken);

            if (propertiesCount == 0 || ownersCount == 0)
            {
                _logger.LogWarning(
                    "??  Collections exist but are empty - Properties: {PropertiesCount}, Owners: {OwnersCount}",
                    propertiesCount,
                    ownersCount);
                
                _logger.LogWarning("?? Consider running the seed script to populate data:");
                _logger.LogWarning("   mongosh < Database/seed-data.js");
            }
            else
            {
                _logger.LogInformation(
                    "? Database validation successful - Properties: {PropertiesCount}, Owners: {OwnersCount}",
                    propertiesCount,
                    ownersCount);
            }
        }
        catch (MongoException ex)
        {
            _logger.LogError(ex, "? Cannot connect to MongoDB");
            _logger.LogError("?? Ensure MongoDB is running:");
            _logger.LogError("   - Local: mongodb://localhost:27017");
            _logger.LogError("   - Or check your connection string in appsettings.json");

            throw new InvalidOperationException(
                "Database connection failed. Please ensure MongoDB is running and accessible.", ex);
        }
        catch (InvalidOperationException)
        {
            // Re-throw validation errors
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "? Unexpected error during database validation");
            throw new InvalidOperationException("Database validation failed due to unexpected error.", ex);
        }
    }
}
