using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RealEstateAPI.Domain.Entities;
using RealEstateAPI.Infrastructure.Configuration;

namespace RealEstateAPI.Infrastructure.Data;

/// <summary>
/// MongoDB database context for managing database connections.
/// </summary>
public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        // Priority: Environment Variable > appsettings.json
        var connectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING") 
                             ?? settings.Value.ConnectionString;
        
        var databaseName = Environment.GetEnvironmentVariable("MONGODB_DATABASE_NAME") 
                         ?? settings.Value.DatabaseName;

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "MongoDB connection string is not configured. " +
                "Set MONGODB_CONNECTION_STRING environment variable or configure in appsettings.json");
        }

        if (string.IsNullOrWhiteSpace(databaseName))
        {
            throw new InvalidOperationException(
                "MongoDB database name is not configured. " +
                "Set MONGODB_DATABASE_NAME environment variable or configure in appsettings.json");
        }

        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    /// <summary>
    /// Properties collection
    /// </summary>
    public IMongoCollection<Property> Properties =>
        _database.GetCollection<Property>("Properties");

    /// <summary>
    /// Owners collection (separate from Properties)
    /// </summary>
    public IMongoCollection<Owner> Owners =>
        _database.GetCollection<Owner>("Owners");

    public IMongoCollection<T> GetCollection<T>(string name)
    {
        return _database.GetCollection<T>(name);
    }

    public IMongoDatabase Database => _database;
}
