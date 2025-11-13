using AutoMapper;
using Microsoft.Extensions.Logging;
using RealEstateAPI.Application.DTOs;
using RealEstateAPI.Application.Exceptions;
using RealEstateAPI.Application.Interfaces;

namespace RealEstateAPI.Application.Services;

/// <summary>
/// Service implementation for property business logic with owner relationship.
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
        
        // Get all unique owner IDs
        var ownerIds = properties.Select(p => p.OwnerId).Distinct();
        var owners = await _ownerRepository.GetByIdsAsync(ownerIds);
        var ownerDictionary = owners.ToDictionary(o => o.Id);

        var propertyDtos = properties.Select(p =>
        {
            var dto = _mapper.Map<PropertyDto>(p);
            if (ownerDictionary.TryGetValue(p.OwnerId, out var owner))
            {
                dto.IdOwner = owner.Id;
            }
            return dto;
        });

        return propertyDtos;
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

        // Load related owner
        var owner = await _ownerRepository.GetByIdAsync(property.OwnerId);
        
        var propertyDto = _mapper.Map<PropertyDetailDto>(property);
        if (owner is not null)
        {
            propertyDto.Owner = _mapper.Map<OwnerDto>(owner);
        }

        return propertyDto;
    }

    public async Task<PagedResponse<PropertyDto>> GetFilteredPropertiesAsync(PropertyFilterDto filter)
    {
        _logger.LogInformation("Filtering properties with criteria: {@Filter}", filter);
        
        var (properties, totalCount) = await _propertyRepository.GetFilteredAsync(filter);
        
        // Get all unique owner IDs
        var ownerIds = properties.Select(p => p.OwnerId).Distinct();
        var owners = await _ownerRepository.GetByIdsAsync(ownerIds);
        var ownerDictionary = owners.ToDictionary(o => o.Id);

        var propertyDtos = properties.Select(p =>
        {
            var dto = _mapper.Map<PropertyDto>(p);
            if (ownerDictionary.TryGetValue(p.OwnerId, out var owner))
            {
                dto.IdOwner = owner.Id;
            }
            return dto;
        });

        return PagedResponse<PropertyDto>.Create(
            propertyDtos,
            filter.Page ?? 1,
            filter.PageSize ?? 10,
            totalCount
        );
    }
}
