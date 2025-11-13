using MongoDB.Bson.Serialization.Attributes;

namespace RealEstateAPI.Domain.Entities;

/// <summary>
/// Represents the owner of a property.
/// </summary>
public class Owner
{
    [BsonElement("idOwner")]
    [BsonRequired]
    public required string IdOwner { get; set; }

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
