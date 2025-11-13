using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RealEstateAPI.Domain.Entities;

/// <summary>
/// Represents the owner of properties (standalone collection).
/// </summary>
public class Owner
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public required string Id { get; set; } // e.g., "OWN-001"

    [BsonElement("name")]
    [BsonRequired]
    public required string Name { get; set; }

    [BsonElement("address")]
    [BsonRequired]
    public required string Address { get; set; }

    [BsonElement("photo")]
    public string? Photo { get; set; }

    [BsonElement("birthday")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime? Birthday { get; set; }
}
