namespace RealEstateAPI.Application.DTOs;

/// <summary>
/// Data transfer object for property image information.
/// </summary>
public sealed record PropertyImageDto
{
    public required string IdPropertyImage { get; init; }
    public required string File { get; init; }
    public required bool Enabled { get; init; }
}
