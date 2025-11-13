using MongoDB.Bson.Serialization.Attributes;

namespace RealEstateAPI.Domain.Entities;

/// <summary>
/// Represents an image associated with a property.
/// </summary>
public class PropertyImage
{
    [BsonElement("idPropertyImage")]
    [BsonRequired]
    public required string IdPropertyImage { get; set; }

    [BsonElement("file")]
    [BsonRequired]
    public required string File { get; set; }

    [BsonElement("enabled")]
    [BsonRequired]
    public required bool Enabled { get; set; }
}
