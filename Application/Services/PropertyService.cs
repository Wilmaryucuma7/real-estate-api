using AutoMapper;
using Microsoft.Extensions.Logging;
using RealEstateAPI.Application.DTOs;
using RealEstateAPI.Application.Interfaces;

namespace RealEstateAPI.Application.Services;

/// <summary>
/// Service implementation for property business logic.
/// </summary>
public sealed class PropertyService : IPropertyService
{
    private readonly IPropertyRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<PropertyService> _logger;

    public PropertyService(
        IPropertyRepository repository,
        IMapper mapper,
        ILogger<PropertyService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<PropertyDto>> GetAllPropertiesAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving all properties");
            var properties = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<PropertyDto>>(properties);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all properties");
            throw;
        }
    }

    public async Task<PropertyDetailDto?> GetPropertyByIdAsync(string id)
    {
        try
        {
            _logger.LogInformation("Retrieving property with ID: {PropertyId}", id);
            var property = await _repository.GetByIdAsync(id);
            
            if (property is null)
            {
                _logger.LogWarning("Property with ID: {PropertyId} not found", id);
                return null;
            }

            return _mapper.Map<PropertyDetailDto>(property);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving property with ID: {PropertyId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<PropertyDto>> GetFilteredPropertiesAsync(PropertyFilterDto filter)
    {
        try
        {
            _logger.LogInformation("Filtering properties with criteria: {@Filter}", filter);
            var properties = await _repository.GetFilteredAsync(filter);
            return _mapper.Map<IEnumerable<PropertyDto>>(properties);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error filtering properties");
            throw;
        }
    }
}
