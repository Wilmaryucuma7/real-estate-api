using AutoMapper;
using Microsoft.Extensions.Logging;
using RealEstateAPI.Application.DTOs;
using RealEstateAPI.Application.Exceptions;
using RealEstateAPI.Application.Interfaces;
using RealEstateAPI.Application.Constants;

namespace RealEstateAPI.Application.Services;

/// <summary>
/// Service implementation for property business logic with optimized queries.
/// </summary>
public sealed class PropertyService : IPropertyService
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IOwnerRepository _ownerRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<PropertyService> _logger;

    public PropertyService(
        IPropertyRepository propertyRepository,
        IOwnerRepository ownerRepository,
        IMapper mapper,
        ILogger<PropertyService> logger)
    {
        _propertyRepository = propertyRepository;
        _ownerRepository = ownerRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<PropertyDto>> GetAllPropertiesAsync()
    {
        _logger.LogInformation("Retrieving all properties");
        var properties = await _propertyRepository.GetAllAsync();
        
        // Map directly without loading owners - DTO only needs IdOwner
        return _mapper.Map<IEnumerable<PropertyDto>>(properties);
    }

    public async Task<PropertyDetailDto?> GetPropertyByIdAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Property ID cannot be null or empty", nameof(id));

        _logger.LogInformation("Retrieving property with ID: {PropertyId}", id);
        var property = await _propertyRepository.GetByIdAsync(id);
        
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
        
        var (properties, totalCount) = await _propertyRepository.GetFilteredAsync(filter);
        
        // Map directly without loading owners - performance optimization
        var propertyDtos = _mapper.Map<IEnumerable<PropertyDto>>(properties);

        return PagedResponse<PropertyDto>.Create(
            propertyDtos,
            filter.Page ?? PaginationConstants.DefaultPage,
            filter.PageSize ?? PaginationConstants.DefaultPageSize,
            totalCount
        );
    }

    public async Task<PropertyDetailDto?> GetPropertyBySlugAsync(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
            throw new ArgumentException("Property slug cannot be null or empty", nameof(slug));

        _logger.LogInformation("Retrieving property by slug: {Slug}", slug);
        
        // Optimized: Uses slug index in MongoDB
        var property = await _propertyRepository.GetBySlugAsync(slug);
        
        if (property is null)
        {
            _logger.LogWarning("Property with slug: {Slug} not found", slug);
            return null;
        }

        return _mapper.Map<PropertyDetailDto>(property);
    }

    public async Task<IEnumerable<PropertyTraceDto>> GetPropertyTracesBySlugAsync(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
            throw new ArgumentException("Property slug cannot be null or empty", nameof(slug));

        _logger.LogInformation("Retrieving traces for property slug: {Slug}", slug);
        
        // Optimized: Uses slug index
        var property = await _propertyRepository.GetBySlugAsync(slug);
        
        if (property is null)
        {
            _logger.LogWarning("Property with slug: {Slug} not found", slug);
            throw new PropertyNotFoundException(slug);
        }

        return _mapper.Map<IEnumerable<PropertyTraceDto>>(property.Traces ?? Enumerable.Empty<Domain.Entities.PropertyTrace>());
    }
}
