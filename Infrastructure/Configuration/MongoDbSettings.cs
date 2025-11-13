namespace RealEstateAPI.Infrastructure.Configuration;

/// <summary>
/// Configuration settings for MongoDB connection.
/// </summary>
public sealed class MongoDbSettings
{
    public const string SectionName = "MongoDbSettings";

    public required string ConnectionString { get; init; }
    public required string DatabaseName { get; init; }
    public required string CollectionName { get; init; }
}
