namespace RealEstateAPI.Application.DTOs;

/// <summary>
/// Data transfer object for property owner information.
/// </summary>
public sealed record OwnerDto
{
    public required string IdOwner { get; init; }
    public required string Name { get; init; }
    public required string Address { get; init; }
    public string? Photo { get; init; }
    public DateTime? Birthday { get; init; }
}
