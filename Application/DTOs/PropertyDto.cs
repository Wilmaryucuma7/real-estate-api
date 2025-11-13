namespace RealEstateAPI.Application.DTOs;

/// <summary>
/// Data transfer object for property list view (basic information with one image).
/// </summary>
public sealed class PropertyDto
{
    public required string Slug { get; set; }
    public required string IdOwner { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required decimal Price { get; set; }
    public required string Image { get; set; }
}
