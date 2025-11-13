using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RealEstateAPI.Domain.Entities;

/// <summary>
/// Represents a historical trace of a property transaction.
/// </summary>
public class PropertyTrace
{
    [BsonElement("idPropertyTrace")]
    [BsonRequired]
    public required string IdPropertyTrace { get; set; }

    [BsonElement("dateSale")]
    [BsonRequired]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public required DateTime DateSale { get; set; }

    [BsonElement("name")]
    [BsonRequired]
    public required string Name { get; set; }

    [BsonElement("value")]
    [BsonRequired]
    [BsonRepresentation(BsonType.Decimal128)]
    public required decimal Value { get; set; }

    [BsonElement("tax")]
    [BsonRequired]
    [BsonRepresentation(BsonType.Decimal128)]
    public required decimal Tax { get; set; }
}
