namespace RealEstateAPI.Application.DTOs;

/// <summary>
/// Data transfer object for filtering properties with pagination support.
/// </summary>
public sealed record PropertyFilterDto
{
    public string? Name { get; init; }
    public string? Address { get; init; }
    public decimal? MinPrice { get; init; }
    public decimal? MaxPrice { get; init; }
    public int? Page { get; init; } = 1;
    public int? PageSize { get; init; } = 10;
}
