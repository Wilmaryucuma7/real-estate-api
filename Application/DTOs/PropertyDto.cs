namespace RealEstateAPI.Application.DTOs;

/// <summary>
/// Data transfer object for property list view (basic information with one image).
/// </summary>
public sealed record PropertyDto
{
    public string IdOwner { get; set; } = string.Empty;
    public required string Name { get; init; }
    public required string Address { get; init; }
    public required decimal Price { get; init; }
    public required string Image { get; init; }
    public required string Id { get; set; }
    public required string Slug { get; set; }
}
