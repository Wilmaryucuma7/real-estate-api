namespace RealEstateAPI.Application.DTOs;

/// <summary>
/// Data transfer object for detailed property view without traces.
/// Traces should be fetched separately via /api/properties/{slug}/traces
/// </summary>
public sealed class PropertyDetailDto
{
    public required string Id { get; set; }
    public required string Slug { get; set; }
    public required string IdOwner { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required decimal Price { get; set; }
    public required string CodeInternal { get; set; }
    public required int Year { get; set; }
    public required IEnumerable<PropertyImageDto> Images { get; set; }
}
