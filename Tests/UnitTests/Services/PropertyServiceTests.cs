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
/// Unit tests for PropertyService business logic with owner relationships.
/// </summary>
[TestFixture]
public class PropertyServiceTests
{
    private Mock<IPropertyRepository> _propertyRepositoryMock = null!;
    private Mock<IOwnerRepository> _ownerRepositoryMock = null!;
    private Mock<IMapper> _mapperMock = null!;
    private Mock<ILogger<PropertyService>> _loggerMock = null!;
    private PropertyService _service = null!;

    [SetUp]
    public void Setup()
    {
        _propertyRepositoryMock = new Mock<IPropertyRepository>();
        _ownerRepositoryMock = new Mock<IOwnerRepository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<PropertyService>>();
        
        _service = new PropertyService(
            _propertyRepositoryMock.Object,
            _ownerRepositoryMock.Object,
            _mapperMock.Object,
            _loggerMock.Object);
    }

    [Test]
    public async Task GetAllPropertiesAsync_ShouldReturnMappedProperties()
    {
        // Arrange
        var properties = new List<Property>
        {
            CreateTestProperty("1", "OWN-001"),
            CreateTestProperty("2", "OWN-002")
        };

        var owners = new List<Owner>
        {
            CreateTestOwner("OWN-001"),
            CreateTestOwner("OWN-002")
        };

        _propertyRepositoryMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(properties);

        _ownerRepositoryMock.Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(owners);

        _mapperMock.Setup(m => m.Map<PropertyDto>(It.IsAny<Property>()))
            .Returns((Property p) => new PropertyDto
            {
                Slug = "test-property",
                IdOwner = p.OwnerId,
                Name = p.Name,
                Address = p.Address,
                Price = p.Price,
                Image = "https://example.com/image.jpg"
            });

        // Act
        var result = await _service.GetAllPropertiesAsync();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(2));
        _propertyRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        _ownerRepositoryMock.Verify(r => r.GetByIdsAsync(It.IsAny<IEnumerable<string>>()), Times.Once);
    }

    [Test]
    public async Task GetPropertyByIdAsync_WithValidId_ShouldReturnPropertyWithOwnerId()
    {
        // Arrange
        var propertyId = "507f1f77bcf86cd799439011";
        var property = CreateTestProperty(propertyId, "OWN-001");
        
        _propertyRepositoryMock.Setup(r => r.GetByIdAsync(propertyId))
            .ReturnsAsync(property);

        _mapperMock.Setup(m => m.Map<PropertyDetailDto>(property))
            .Returns(new PropertyDetailDto
            {
                Id = property.Id!,
                Slug = "test-property",
                IdOwner = property.OwnerId,
                Name = property.Name,
                Address = property.Address,
                Price = property.Price,
                CodeInternal = property.CodeInternal,
                Year = property.Year,
                Images = new List<PropertyImageDto>()
            });

        // Act
        var result = await _service.GetPropertyByIdAsync(propertyId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(propertyId));
        Assert.That(result.IdOwner, Is.EqualTo("OWN-001"));
        _propertyRepositoryMock.Verify(r => r.GetByIdAsync(propertyId), Times.Once);
    }

    [Test]
    public void GetPropertyByIdAsync_WithInvalidId_ShouldThrowPropertyNotFoundException()
    {
        // Arrange
        var invalidId = "invalid-id";

        _propertyRepositoryMock.Setup(r => r.GetByIdAsync(invalidId))
            .ReturnsAsync((Property?)null);

        // Act & Assert
        Assert.ThrowsAsync<PropertyNotFoundException>(async () =>
            await _service.GetPropertyByIdAsync(invalidId));
        _propertyRepositoryMock.Verify(r => r.GetByIdAsync(invalidId), Times.Once);
    }

    [Test]
    public void GetPropertyByIdAsync_WithNullOrEmptyId_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(async () =>
            await _service.GetPropertyByIdAsync(string.Empty));
    }

    [Test]
    public async Task GetFilteredPropertiesAsync_WithNameFilter_ShouldReturnPagedProperties()
    {
        // Arrange
        var filter = new PropertyFilterDto { Name = "Beach House", Page = 1, PageSize = 10 };
        var properties = new List<Property> { CreateTestProperty("1", "OWN-001") };
        var owners = new List<Owner> { CreateTestOwner("OWN-001") };

        _propertyRepositoryMock.Setup(r => r.GetFilteredAsync(filter))
            .ReturnsAsync((properties, 1));

        _ownerRepositoryMock.Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(owners);

        _mapperMock.Setup(m => m.Map<PropertyDto>(It.IsAny<Property>()))
            .Returns((Property p) => new PropertyDto
            {
                Slug = "beach-house",
                IdOwner = p.OwnerId,
                Name = p.Name,
                Address = p.Address,
                Price = p.Price,
                Image = "https://example.com/image.jpg"
            });

        // Act
        var result = await _service.GetFilteredPropertiesAsync(filter);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Data.Count(), Is.EqualTo(1));
        Assert.That(result.TotalCount, Is.EqualTo(1));
        Assert.That(result.Page, Is.EqualTo(1));
        Assert.That(result.PageSize, Is.EqualTo(10));
        Assert.That(result.TotalPages, Is.EqualTo(1));
        _propertyRepositoryMock.Verify(r => r.GetFilteredAsync(filter), Times.Once);
    }

    [Test]
    public async Task GetFilteredPropertiesAsync_WithMultiplePages_ShouldCalculateTotalPagesCorrectly()
    {
        // Arrange
        var filter = new PropertyFilterDto { Page = 2, PageSize = 10 };
        var properties = new List<Property> { CreateTestProperty("1", "OWN-001") };
        var owners = new List<Owner> { CreateTestOwner("OWN-001") };

        _propertyRepositoryMock.Setup(r => r.GetFilteredAsync(filter))
            .ReturnsAsync((properties, 25)); // 25 total items = 3 pages

        _ownerRepositoryMock.Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(owners);

        _mapperMock.Setup(m => m.Map<PropertyDto>(It.IsAny<Property>()))
            .Returns((Property p) => new PropertyDto
            {
                Slug = "test-property",
                IdOwner = p.OwnerId,
                Name = p.Name,
                Address = p.Address,
                Price = p.Price,
                Image = "https://example.com/image.jpg"
            });

        // Act
        var result = await _service.GetFilteredPropertiesAsync(filter);

        // Assert
        Assert.That(result.TotalCount, Is.EqualTo(25));
        Assert.That(result.TotalPages, Is.EqualTo(3));
        Assert.That(result.HasPreviousPage, Is.True);
        Assert.That(result.HasNextPage, Is.True);
    }

    private static Property CreateTestProperty(string id, string ownerId)
    {
        return new Property
        {
            Id = id,
            Name = "Test Property",
            Address = "123 Test St",
            Price = 250000,
            CodeInternal = "PROP-001",
            Year = 2020,
            OwnerId = ownerId,
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

    private static Owner CreateTestOwner(string id)
    {
        return new Owner
        {
            Id = id,
            Name = "John Doe",
            Address = "456 Owner St",
            Photo = null,
            Birthday = null
        };
    }
}
