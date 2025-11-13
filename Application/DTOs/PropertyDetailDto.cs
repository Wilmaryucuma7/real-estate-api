namespace RealEstateAPI.Application.DTOs;

/// <summary>
/// Data transfer object for detailed property view with complete information.
/// </summary>
public sealed record PropertyDetailDto
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Address { get; init; }
    public required decimal Price { get; init; }
    public required string CodeInternal { get; init; }
    public required int Year { get; init; }
    public OwnerDto? Owner { get; set; }
    public IReadOnlyCollection<PropertyImageDto> Images { get; init; } = [];
    public IReadOnlyCollection<PropertyTraceDto> Traces { get; init; } = [];
}
