using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RealEstateAPI.Application.DTOs;
using RealEstateAPI.Application.Exceptions;
using RealEstateAPI.Application.Interfaces;
using RealEstateAPI.Application.Services;
using RealEstateAPI.Domain.Entities;

namespace RealEstateAPI.Tests.UnitTests.Services;

/// <summary>
/// Unit tests for PropertyService business logic.
/// </summary>
[TestFixture]
public class PropertyServiceTests
{
    private Mock<IPropertyRepository> _repositoryMock = null!;
    private Mock<IMapper> _mapperMock = null!;
    private Mock<ILogger<PropertyService>> _loggerMock = null!;
    private PropertyService _service = null!;

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<IPropertyRepository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<PropertyService>>();
        
        _service = new PropertyService(
            _repositoryMock.Object,
            _mapperMock.Object,
            _loggerMock.Object);
    }

    [Test]
    public async Task GetAllPropertiesAsync_ShouldReturnMappedProperties()
    {
        // Arrange
        var properties = new List<Property>
        {
            CreateTestProperty("1"),
            CreateTestProperty("2")
        };

        var propertyDtos = new List<PropertyDto>
        {
            CreateTestPropertyDto("OWN-1"),
            CreateTestPropertyDto("OWN-2")
        };

        _repositoryMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(properties);

        _mapperMock.Setup(m => m.Map<IEnumerable<PropertyDto>>(properties))
            .Returns(propertyDtos);

        // Act
        var result = await _service.GetAllPropertiesAsync();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(2));
        _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Test]
    public async Task GetPropertyByIdAsync_WithValidId_ShouldReturnProperty()
    {
        // Arrange
        var propertyId = "507f1f77bcf86cd799439011";
        var property = CreateTestProperty(propertyId);
        var propertyDetailDto = CreateTestPropertyDetailDto(propertyId);

        _repositoryMock.Setup(r => r.GetByIdAsync(propertyId))
            .ReturnsAsync(property);

        _mapperMock.Setup(m => m.Map<PropertyDetailDto>(property))
            .Returns(propertyDetailDto);

        // Act
        var result = await _service.GetPropertyByIdAsync(propertyId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(propertyId));
        _repositoryMock.Verify(r => r.GetByIdAsync(propertyId), Times.Once);
    }

    [Test]
    public void GetPropertyByIdAsync_WithInvalidId_ShouldThrowPropertyNotFoundException()
    {
        // Arrange
        var invalidId = "invalid-id";

        _repositoryMock.Setup(r => r.GetByIdAsync(invalidId))
            .ReturnsAsync((Property?)null);

        // Act & Assert
        Assert.ThrowsAsync<PropertyNotFoundException>(async () =>
            await _service.GetPropertyByIdAsync(invalidId));
        _repositoryMock.Verify(r => r.GetByIdAsync(invalidId), Times.Once);
    }

    [Test]
    public void GetPropertyByIdAsync_WithNullOrEmptyId_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(async () =>
            await _service.GetPropertyByIdAsync(string.Empty));
    }

    [Test]
    public async Task GetFilteredPropertiesAsync_WithNameFilter_ShouldReturnFilteredProperties()
    {
        // Arrange
        var filter = new PropertyFilterDto { Name = "Beach House" };
        var properties = new List<Property> { CreateTestProperty("1") };
        var propertyDtos = new List<PropertyDto> { CreateTestPropertyDto("OWN-1") };

        _repositoryMock.Setup(r => r.GetFilteredAsync(filter))
            .ReturnsAsync(properties);

        _mapperMock.Setup(m => m.Map<IEnumerable<PropertyDto>>(properties))
            .Returns(propertyDtos);

        // Act
        var result = await _service.GetFilteredPropertiesAsync(filter);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(1));
        _repositoryMock.Verify(r => r.GetFilteredAsync(filter), Times.Once);
    }

    [Test]
    public async Task GetFilteredPropertiesAsync_WithPriceRange_ShouldReturnFilteredProperties()
    {
        // Arrange
        var filter = new PropertyFilterDto { MinPrice = 100000, MaxPrice = 500000 };
        var properties = new List<Property> { CreateTestProperty("1") };
        var propertyDtos = new List<PropertyDto> { CreateTestPropertyDto("OWN-1") };

        _repositoryMock.Setup(r => r.GetFilteredAsync(filter))
            .ReturnsAsync(properties);

        _mapperMock.Setup(m => m.Map<IEnumerable<PropertyDto>>(properties))
            .Returns(propertyDtos);

        // Act
        var result = await _service.GetFilteredPropertiesAsync(filter);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(1));
        _repositoryMock.Verify(r => r.GetFilteredAsync(filter), Times.Once);
    }

    private static Property CreateTestProperty(string id)
    {
        return new Property
        {
            Id = id,
            Name = "Test Property",
            Address = "123 Test St",
            Price = 250000,
            CodeInternal = "PROP-001",
            Year = 2020,
            Owner = new Owner
            {
                IdOwner = "OWN-001",
                Name = "John Doe",
                Address = "456 Owner St",
                Photo = null,
                Birthday = null
            },
            Images = new List<PropertyImage>
            {
                new()
                {
                    IdPropertyImage = "IMG-001",
                    File = "https://example.com/image.jpg",
                    Enabled = true
                }
            },
            Traces = new List<PropertyTrace>()
        };
    }

    private static PropertyDto CreateTestPropertyDto(string ownerId)
    {
        return new PropertyDto
        {
            IdOwner = ownerId,
            Name = "Test Property",
            Address = "123 Test St",
            Price = 250000,
            Image = "https://example.com/image.jpg"
        };
    }

    private static PropertyDetailDto CreateTestPropertyDetailDto(string id)
    {
        return new PropertyDetailDto
        {
            Id = id,
            Name = "Test Property",
            Address = "123 Test St",
            Price = 250000,
            CodeInternal = "PROP-001",
            Year = 2020,
            Owner = new OwnerDto
            {
                IdOwner = "OWN-001",
                Name = "John Doe",
                Address = "456 Owner St"
            },
            Images = new List<PropertyImageDto>(),
            Traces = new List<PropertyTraceDto>()
        };
    }
}
