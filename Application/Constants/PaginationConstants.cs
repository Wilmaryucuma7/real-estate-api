namespace RealEstateAPI.Application.Constants;

/// <summary>
/// Constants for pagination configuration.
/// </summary>
public static class PaginationConstants
{
    /// <summary>
    /// Maximum allowed page size for pagination.
    /// </summary>
    public const int MaxPageSize = 100;

    /// <summary>
    /// Default page size when not specified.
    /// </summary>
    public const int DefaultPageSize = 10;

    /// <summary>
    /// Default page number when not specified.
    /// </summary>
    public const int DefaultPage = 1;

    /// <summary>
    /// Minimum page number allowed.
    /// </summary>
    public const int MinPage = 1;

    /// <summary>
    /// Minimum page size allowed.
    /// </summary>
    public const int MinPageSize = 1;
}
