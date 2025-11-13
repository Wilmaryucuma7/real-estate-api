using Microsoft.AspNetCore.Mvc;
using RealEstateAPI.Application.DTOs;
using RealEstateAPI.Application.Interfaces;
using RealEstateAPI.Application.Validators;

namespace RealEstateAPI.Controllers;

/// <summary>
/// API controller for managing property operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class PropertiesController : ControllerBase
{
    private readonly IPropertyService _propertyService;
    private readonly ILogger<PropertiesController> _logger;
    private readonly PropertyFilterValidator _filterValidator;

    public PropertiesController(
        IPropertyService propertyService, 
        ILogger<PropertiesController> logger,
        PropertyFilterValidator filterValidator)
    {
        _propertyService = propertyService;
        _logger = logger;
        _filterValidator = filterValidator;
    }

    /// <summary>
    /// Get all properties or filter by criteria with pagination.
    /// </summary>
    /// <param name="name">Filter by property name (optional)</param>
    /// <param name="address">Filter by property address (optional)</param>
    /// <param name="minPrice">Minimum price range (optional)</param>
    /// <param name="maxPrice">Maximum price range (optional)</param>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="pageSize">Items per page (default: 10, max: 100)</param>
    /// <returns>Paginated list of properties with metadata</returns>
    /// <response code="200">Returns paginated properties with total count and page info</response>
    /// <response code="400">Invalid filter parameters</response>
    [HttpGet]
    [ProducesResponseType<PagedResponse<PropertyDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PagedResponse<PropertyDto>>> GetProperties(
        [FromQuery] string? name = null,
        [FromQuery] string? address = null,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null,
        [FromQuery] int? page = 1,
        [FromQuery] int? pageSize = 10)
    {
        var filter = new PropertyFilterDto
        {
            Name = name,
            Address = address,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            Page = page,
            PageSize = pageSize
        };

        // Validate filter
        var validationResult = await _filterValidator.ValidateAsync(filter);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            throw new Application.Exceptions.ValidationException(
                "Validation failed for property filter",
                errors
            );
        }

        // Always use GetFilteredPropertiesAsync - it handles pagination correctly
        // even when there are no filters (empty filter = get all with pagination)
        var pagedResult = await _propertyService.GetFilteredPropertiesAsync(filter);
        return Ok(pagedResult);
    }

    /// <summary>
    /// Get detailed information about a specific property by ID.
    /// </summary>
    /// <param name="id">Property unique identifier</param>
    /// <returns>Detailed property information</returns>
    /// <response code="200">Returns property details</response>
    /// <response code="404">Property not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType<PropertyDetailDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PropertyDetailDto>> GetPropertyById(string id)
    {
        _logger.LogInformation("Requesting property details for ID: {PropertyId}", id);
        var property = await _propertyService.GetPropertyByIdAsync(id);
        return Ok(property);
    }

    /// <summary>
    /// Get property by slug-friendly name (SEO-friendly URL).
    /// </summary>
    /// <param name="slug">Property name in slug format (e.g., modern-beach-house)</param>
    /// <returns>Detailed property information</returns>
    /// <response code="200">Returns property details</response>
    /// <response code="404">Property not found</response>
    /// <example>
    /// GET /api/properties/by-name/modern-beach-house
    /// GET /api/properties/by-name/downtown-luxury-penthouse
    /// </example>
    [HttpGet("by-name/{slug}")]
    [ProducesResponseType<PropertyDetailDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PropertyDetailDto>> GetPropertyByName(string slug)
    {
        _logger.LogInformation("Requesting property by slug: {Slug}", slug);
        var property = await _propertyService.GetPropertyBySlugAsync(slug);
        
        if (property is null)
        {
            return NotFound(new { message = $"Property with name '{slug}' was not found." });
        }

        return Ok(property);
    }

    /// <summary>
    /// Get property traces (transaction history) by slug name.
    /// </summary>
    /// <param name="slug">Property name in slug format (e.g., modern-beach-house)</param>
    /// <returns>List of property traces</returns>
    /// <response code="200">Returns property traces</response>
    /// <response code="404">Property not found</response>
    /// <example>
    /// GET /api/properties/modern-beach-house/traces
    /// </example>
    [HttpGet("{slug}/traces")]
    [ProducesResponseType<IEnumerable<PropertyTraceDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<PropertyTraceDto>>> GetPropertyTraces(string slug)
    {
        _logger.LogInformation("Requesting traces for property slug: {Slug}", slug);
        var traces = await _propertyService.GetPropertyTracesBySlugAsync(slug);
        return Ok(traces);
    }
}
