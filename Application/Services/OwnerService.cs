using AutoMapper;
using Microsoft.Extensions.Logging;
using RealEstateAPI.Application.DTOs;
using RealEstateAPI.Application.Interfaces;

namespace RealEstateAPI.Application.Services;

/// <summary>
/// Service implementation for owner business logic.
/// </summary>
public sealed class OwnerService : IOwnerService
{
    private readonly IOwnerRepository _ownerRepository;
    private readonly IPropertyRepository _propertyRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<OwnerService> _logger;

    public OwnerService(
        IOwnerRepository ownerRepository,
        IPropertyRepository propertyRepository,
        IMapper mapper,
        ILogger<OwnerService> logger)
    {
        _ownerRepository = ownerRepository;
        _propertyRepository = propertyRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedResponse<OwnerDto>> GetAllOwnersAsync(int page, int pageSize)
    {
        _logger.LogInformation("Retrieving all owners - Page: {Page}, PageSize: {PageSize}", page, pageSize);

        // Validate and sanitize pagination parameters
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var (owners, totalCount) = await _ownerRepository.GetAllAsync(page, pageSize);
        var ownerDtos = _mapper.Map<IEnumerable<OwnerDto>>(owners);

        return PagedResponse<OwnerDto>.Create(ownerDtos, page, pageSize, totalCount);
    }

    public async Task<OwnerDto?> GetOwnerByIdAsync(string id)
    {
        _logger.LogInformation("Retrieving owner with ID: {OwnerId}", id);
        
        var owner = await _ownerRepository.GetByIdAsync(id);
        
        if (owner is null)
        {
            _logger.LogWarning("Owner with ID: {OwnerId} not found", id);
            return null;
        }

        return _mapper.Map<OwnerDto>(owner);
    }

    public async Task<PagedResponse<PropertyDto>> GetPropertiesByOwnerIdAsync(string ownerId, int page, int pageSize)
    {
        _logger.LogInformation("Retrieving properties for owner: {OwnerId}", ownerId);

        // Validate and sanitize pagination parameters
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var (properties, totalCount) = await _propertyRepository.GetByOwnerIdAsync(ownerId, page, pageSize);
        
        // Map properties and include owner info
        var owner = await _ownerRepository.GetByIdAsync(ownerId);
        var propertyDtos = properties.Select(p =>
        {
            var dto = _mapper.Map<PropertyDto>(p);
            if (owner is not null)
            {
                dto.IdOwner = owner.Id;
            }
            return dto;
        });

        return PagedResponse<PropertyDto>.Create(propertyDtos, page, pageSize, totalCount);
    }
}
