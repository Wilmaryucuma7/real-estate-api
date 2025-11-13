using Microsoft.AspNetCore.Mvc;
using RealEstateAPI.Application.DTOs;
using RealEstateAPI.Application.Interfaces;

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

    public PropertiesController(
        IPropertyService propertyService, 
        ILogger<PropertiesController> logger)
    {
        _propertyService = propertyService;
        _logger = logger;
    }

    /// <summary>
    /// Get all properties or filter by criteria.
    /// </summary>
    /// <param name="name">Filter by property name (optional)</param>
    /// <param name="address">Filter by property address (optional)</param>
    /// <param name="minPrice">Minimum price range (optional)</param>
    /// <param name="maxPrice">Maximum price range (optional)</param>
    /// <returns>List of properties matching the criteria</returns>
    [HttpGet]
    [ProducesResponseType<IEnumerable<PropertyDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<PropertyDto>>> GetProperties(
        [FromQuery] string? name = null,
        [FromQuery] string? address = null,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null)
    {
        try
        {
            var hasFilters = !string.IsNullOrWhiteSpace(name) || 
                           !string.IsNullOrWhiteSpace(address) || 
                           minPrice.HasValue || 
                           maxPrice.HasValue;

            if (hasFilters)
            {
                var filter = new PropertyFilterDto
                {
                    Name = name,
                    Address = address,
                    MinPrice = minPrice,
                    MaxPrice = maxPrice
                };

                var properties = await _propertyService.GetFilteredPropertiesAsync(filter);
                return Ok(properties);
            }

            var allProperties = await _propertyService.GetAllPropertiesAsync();
            return Ok(allProperties);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving properties");
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { message = "An error occurred while retrieving properties" });
        }
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
        try
        {
            var property = await _propertyService.GetPropertyByIdAsync(id);

            if (property is null)
            {
                return NotFound(new { message = $"Property with ID '{id}' not found" });
            }

            return Ok(property);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving property with ID: {PropertyId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { message = "An error occurred while retrieving the property" });
        }
    }
}
