namespace RealEstateAPI.Application.DTOs;

/// <summary>
/// Paginated response wrapper with metadata.
/// </summary>
/// <typeparam name="T">Type of items in the collection</typeparam>
public sealed record PagedResponse<T>
{
    /// <summary>
    /// Collection of items for the current page
    /// </summary>
    public required IEnumerable<T> Data { get; init; }

    /// <summary>
    /// Current page number
    /// </summary>
    public required int Page { get; init; }
    public required int PageSize { get; init; }

    /// <summary>
    /// Total number of items across all pages
    /// </summary>
    public required int TotalCount { get; init; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;

    /// <summary>
    /// Creates a paginated response from a collection
    /// </summary>
    public static PagedResponse<T> Create(IEnumerable<T> data, int page, int pageSize, int totalCount)
    {
        return new PagedResponse<T>
        {
            Data = data,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}
