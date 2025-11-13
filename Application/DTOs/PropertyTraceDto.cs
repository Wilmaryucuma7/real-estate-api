namespace RealEstateAPI.Application.DTOs;

/// <summary>
/// Data transfer object for property transaction trace information.
/// </summary>
public sealed record PropertyTraceDto
{
    public required string IdPropertyTrace { get; init; }
    public required DateTime DateSale { get; init; }
    public required string Name { get; init; }
    public required decimal Value { get; init; }
    public required decimal Tax { get; init; }
}
