using Microsoft.AspNetCore.Mvc;
using RealEstateAPI.Application.DTOs;
using RealEstateAPI.Application.Interfaces;

namespace RealEstateAPI.Controllers;

/// <summary>
/// API controller for managing owner operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class OwnersController : ControllerBase
{
    private readonly IOwnerService _ownerService;
    private readonly ILogger<OwnersController> _logger;

    public OwnersController(
        IOwnerService ownerService,
        ILogger<OwnersController> logger)
    {
        _ownerService = ownerService;
        _logger = logger;
    }

    /// <summary>
    /// Get all owners with pagination.
    /// </summary>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="pageSize">Items per page (default: 10, max: 100)</param>
    /// <returns>Paginated list of owners</returns>
    /// <response code="200">Returns paginated owners</response>
    [HttpGet]
    [ProducesResponseType<PagedResponse<OwnerDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PagedResponse<OwnerDto>>> GetAllOwners(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("Retrieving all owners - Page: {Page}, PageSize: {PageSize}", page, pageSize);
        var pagedOwners = await _ownerService.GetAllOwnersAsync(page, pageSize);
        return Ok(pagedOwners);
    }

    /// <summary>
    /// Get owner by ID.
    /// </summary>
    /// <param name="id">Owner unique identifier</param>
    /// <returns>Owner details</returns>
    /// <response code="200">Returns owner details</response>
    /// <response code="404">Owner not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType<OwnerDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<OwnerDto>> GetOwnerById(string id)
    {
        _logger.LogInformation("Retrieving owner with ID: {OwnerId}", id);
        var owner = await _ownerService.GetOwnerByIdAsync(id);
        
        if (owner is null)
        {
            return NotFound(new { message = $"Owner with ID '{id}' was not found." });
        }

        return Ok(owner);
    }

    /// <summary>
    /// Get all properties owned by a specific owner.
    /// </summary>
    /// <param name="id">Owner unique identifier</param>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="pageSize">Items per page (default: 10, max: 100)</param>
    /// <returns>Paginated list of properties for the owner</returns>
    /// <response code="200">Returns paginated properties</response>
    /// <response code="404">Owner not found</response>
    [HttpGet("{id}/properties")]
    [ProducesResponseType<PagedResponse<PropertyDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PagedResponse<PropertyDto>>> GetPropertiesByOwnerId(
        string id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("Retrieving properties for owner: {OwnerId}", id);
        
        // First check if owner exists
        var owner = await _ownerService.GetOwnerByIdAsync(id);
        if (owner is null)
        {
            return NotFound(new { message = $"Owner with ID '{id}' was not found." });
        }

        var properties = await _ownerService.GetPropertiesByOwnerIdAsync(id, page, pageSize);
        return Ok(properties);
    }
}
