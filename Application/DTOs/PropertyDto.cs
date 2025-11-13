namespace RealEstateAPI.Application.DTOs;

/// <summary>
/// Data transfer object for property list view (basic information with one image).
/// </summary>
public sealed record PropertyDto
{
    public required string IdOwner { get; init; }
    public required string Name { get; init; }
    public required string Address { get; init; }
    public required decimal Price { get; init; }
    public string? Image { get; init; }
}
