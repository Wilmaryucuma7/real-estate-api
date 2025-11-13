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
    /// <returns>List of properties matching the criteria</returns>
    [HttpGet]
    [ProducesResponseType<IEnumerable<PropertyDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<PropertyDto>>> GetProperties(
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

        var hasFilters = !string.IsNullOrWhiteSpace(name) || 
                       !string.IsNullOrWhiteSpace(address) || 
                       minPrice.HasValue || 
                       maxPrice.HasValue;

        if (hasFilters)
        {
            var properties = await _propertyService.GetFilteredPropertiesAsync(filter);
            return Ok(properties);
        }

        var allProperties = await _propertyService.GetAllPropertiesAsync();
        return Ok(allProperties);
    }

    /// <summary>
    /// Get detailed information about a specific property by ID.
    /// </summary>
    /// <param name="id">Property unique identifier</param>
    /// <returns>Detailed property information</returns>
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
}
