using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RealEstateAPI.Domain.Entities;

/// <summary>
/// Represents a real estate property with owner reference.
/// </summary>
public class Property
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name")]
    [BsonRequired]
    public required string Name { get; set; }

    [BsonElement("address")]
    [BsonRequired]
    public required string Address { get; set; }

    [BsonElement("price")]
    [BsonRequired]
    [BsonRepresentation(BsonType.Decimal128)]
    public required decimal Price { get; set; }

    [BsonElement("codeInternal")]
    [BsonRequired]
    public required string CodeInternal { get; set; }

    [BsonElement("year")]
    [BsonRequired]
    public required int Year { get; set; }

    /// <summary>
    /// Reference to the owner's ID (foreign key pattern)
    /// </summary>
    [BsonElement("ownerId")]
    [BsonRequired]
    public required string OwnerId { get; set; }

    [BsonElement("images")]
    public List<PropertyImage> Images { get; set; } = [];

    [BsonElement("traces")]
    public List<PropertyTrace> Traces { get; set; } = [];
}
