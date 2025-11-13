using AutoMapper;
using Microsoft.Extensions.Logging;
using RealEstateAPI.Application.DTOs;
using RealEstateAPI.Application.Exceptions;
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
        _logger.LogInformation("Retrieving all properties");
        var properties = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<PropertyDto>>(properties);
    }

    public async Task<PropertyDetailDto?> GetPropertyByIdAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Property ID cannot be null or empty", nameof(id));

        _logger.LogInformation("Retrieving property with ID: {PropertyId}", id);
        var property = await _repository.GetByIdAsync(id);
        
        if (property is null)
        {
            _logger.LogWarning("Property with ID: {PropertyId} not found", id);
            throw new PropertyNotFoundException(id);
        }

        return _mapper.Map<PropertyDetailDto>(property);
    }

    public async Task<PagedResponse<PropertyDto>> GetFilteredPropertiesAsync(PropertyFilterDto filter)
    {
        _logger.LogInformation("Filtering properties with criteria: {@Filter}", filter);
        
        var (properties, totalCount) = await _repository.GetFilteredAsync(filter);
        var propertyDtos = _mapper.Map<IEnumerable<PropertyDto>>(properties);

        return PagedResponse<PropertyDto>.Create(
            propertyDtos,
            filter.Page ?? 1,
            filter.PageSize ?? 10,
            totalCount
        );
    }
}
