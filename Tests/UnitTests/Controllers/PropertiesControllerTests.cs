using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RealEstateAPI.Application.DTOs;
using RealEstateAPI.Application.Exceptions;
using RealEstateAPI.Application.Interfaces;
using RealEstateAPI.Application.Validators;
using RealEstateAPI.Controllers;

namespace RealEstateAPI.Tests.UnitTests.Controllers;

/// <summary>
/// Unit tests for PropertiesController.
/// </summary>
[TestFixture]
public class PropertiesControllerTests
{
    private Mock<IPropertyService> _serviceMock = null!;
    private Mock<ILogger<PropertiesController>> _loggerMock = null!;
    private PropertyFilterValidator _validator = null!;
    private PropertiesController _controller = null!;

    [SetUp]
    public void Setup()
    {
        _serviceMock = new Mock<IPropertyService>();
        _loggerMock = new Mock<ILogger<PropertiesController>>();
        _validator = new PropertyFilterValidator();
        
        _controller = new PropertiesController(
            _serviceMock.Object,
            _loggerMock.Object,
            _validator);
    }

    [Test]
    public async Task GetProperties_WithoutFilters_ReturnsPagedProperties()
    {
        // Arrange
        var properties = new List<PropertyDto>
        {
            new() { IdOwner = "OWN-1", Name = "Property 1", Address = "Address 1", Price = 100000, Image = "img1.jpg" },
            new() { IdOwner = "OWN-2", Name = "Property 2", Address = "Address 2", Price = 200000, Image = "img2.jpg" }
        };

        var pagedResponse = PagedResponse<PropertyDto>.Create(properties, 1, 10, 2);

        _serviceMock.Setup(s => s.GetFilteredPropertiesAsync(It.IsAny<PropertyFilterDto>()))
            .ReturnsAsync(pagedResponse);

        // Act
        var result = await _controller.GetProperties();

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        var returnedResponse = okResult!.Value as PagedResponse<PropertyDto>;
        Assert.That(returnedResponse, Is.Not.Null);
        Assert.That(returnedResponse!.Data.Count(), Is.EqualTo(2));
        Assert.That(returnedResponse.TotalCount, Is.EqualTo(2));
        _serviceMock.Verify(s => s.GetFilteredPropertiesAsync(It.IsAny<PropertyFilterDto>()), Times.Once);
    }

    [Test]
    public async Task GetProperties_WithValidNameFilter_ReturnsPagedFilteredProperties()
    {
        // Arrange
        var properties = new List<PropertyDto>
        {
            new() { IdOwner = "OWN-1", Name = "Beach House", Address = "Ocean Drive", Price = 500000, Image = "img1.jpg" }
        };

        var pagedResponse = PagedResponse<PropertyDto>.Create(properties, 1, 10, 1);

        _serviceMock.Setup(s => s.GetFilteredPropertiesAsync(It.IsAny<PropertyFilterDto>()))
            .ReturnsAsync(pagedResponse);

        // Act
        var result = await _controller.GetProperties(name: "Beach");

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        var returnedResponse = okResult!.Value as PagedResponse<PropertyDto>;
        Assert.That(returnedResponse, Is.Not.Null);
        Assert.That(returnedResponse!.Data.Count(), Is.EqualTo(1));
        Assert.That(returnedResponse.TotalCount, Is.EqualTo(1));
        Assert.That(returnedResponse.TotalPages, Is.EqualTo(1));
    }

    [Test]
    public void GetProperties_WithInvalidCharactersInName_ThrowsValidationException()
    {
        // Arrange
        var invalidName = "Property<script>alert('xss')</script>";

        // Act & Assert
        Assert.ThrowsAsync<ValidationException>(async () =>
            await _controller.GetProperties(name: invalidName));
    }

    [Test]
    public void GetProperties_WithNegativeMinPrice_ThrowsValidationException()
    {
        // Arrange & Act & Assert
        Assert.ThrowsAsync<ValidationException>(async () =>
            await _controller.GetProperties(minPrice: -100));
    }

    [Test]
    public void GetProperties_WithMaxPriceLessThanMinPrice_ThrowsValidationException()
    {
        // Arrange & Act & Assert
        Assert.ThrowsAsync<ValidationException>(async () =>
            await _controller.GetProperties(minPrice: 500000, maxPrice: 100000));
    }

    [Test]
    public async Task GetProperties_WithValidPriceRange_ReturnsPagedFilteredProperties()
    {
        // Arrange
        var properties = new List<PropertyDto>
        {
            new() { IdOwner = "OWN-1", Name = "Property", Address = "Address", Price = 300000, Image = "img.jpg" }
        };

        var pagedResponse = PagedResponse<PropertyDto>.Create(properties, 1, 10, 1);

        _serviceMock.Setup(s => s.GetFilteredPropertiesAsync(It.IsAny<PropertyFilterDto>()))
            .ReturnsAsync(pagedResponse);

        // Act
        var result = await _controller.GetProperties(minPrice: 200000, maxPrice: 400000);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task GetProperties_WithPagination_ReturnsCorrectPageMetadata()
    {
        // Arrange
        var properties = new List<PropertyDto>
        {
            new() { IdOwner = "OWN-1", Name = "Property 1", Address = "Address 1", Price = 100000, Image = "img1.jpg" }
        };

        var pagedResponse = PagedResponse<PropertyDto>.Create(properties, 1, 10, 1);

        _serviceMock.Setup(s => s.GetFilteredPropertiesAsync(It.IsAny<PropertyFilterDto>()))
            .ReturnsAsync(pagedResponse);

        // Act
        var result = await _controller.GetProperties(page: 1, pageSize: 10);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        var returnedResponse = okResult!.Value as PagedResponse<PropertyDto>;
        Assert.That(returnedResponse!.Page, Is.EqualTo(1));
        Assert.That(returnedResponse.PageSize, Is.EqualTo(10));
    }

    [Test]
    public void GetProperties_WithInvalidPageSize_ThrowsValidationException()
    {
        // Arrange & Act & Assert
        Assert.ThrowsAsync<ValidationException>(async () =>
            await _controller.GetProperties(pageSize: 150)); // Max is 100
    }

    [Test]
    public async Task GetPropertyById_WithValidId_ReturnsProperty()
    {
        // Arrange
        var propertyId = "507f1f77bcf86cd799439011";
        var property = new PropertyDetailDto
        {
            Id = propertyId,
            Name = "Test Property",
            Address = "Test Address",
            Price = 250000,
            CodeInternal = "PROP-001",
            Year = 2020,
            Owner = new OwnerDto 
            { 
                IdOwner = "OWN-1", 
                Name = "Owner", 
                Address = "Owner Address" 
            },
            Images = new List<PropertyImageDto>(),
            Traces = new List<PropertyTraceDto>()
        };

        _serviceMock.Setup(s => s.GetPropertyByIdAsync(propertyId))
            .ReturnsAsync(property);

        // Act
        var result = await _controller.GetPropertyById(propertyId);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        var returnedProperty = okResult!.Value as PropertyDetailDto;
        Assert.That(returnedProperty, Is.Not.Null);
        Assert.That(returnedProperty!.Id, Is.EqualTo(propertyId));
        _serviceMock.Verify(s => s.GetPropertyByIdAsync(propertyId), Times.Once);
    }

    [Test]
    public void GetPropertyById_WithInvalidId_ThrowsPropertyNotFoundException()
    {
        // Arrange
        var invalidId = "invalid-id";

        _serviceMock.Setup(s => s.GetPropertyByIdAsync(invalidId))
            .ThrowsAsync(new PropertyNotFoundException(invalidId));

        // Act & Assert
        Assert.ThrowsAsync<PropertyNotFoundException>(async () =>
            await _controller.GetPropertyById(invalidId));
    }

    [Test]
    public async Task GetProperties_WithAccentedCharacters_ReturnsResults()
    {
        // Arrange
        var properties = new List<PropertyDto>
        {
            new() { IdOwner = "OWN-1", Name = "Casa en Bogotá", Address = "Calle José María", Price = 300000, Image = "img.jpg" }
        };

        var pagedResponse = PagedResponse<PropertyDto>.Create(properties, 1, 10, 1);

        _serviceMock.Setup(s => s.GetFilteredPropertiesAsync(It.IsAny<PropertyFilterDto>()))
            .ReturnsAsync(pagedResponse);

        // Act
        var result = await _controller.GetProperties(name: "Bogotá", address: "José María");

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult!.Value, Is.Not.Null);
    }

    [Test]
    public async Task GetProperties_WithMultiplePages_ReturnsCorrectMetadata()
    {
        // Arrange
        var properties = new List<PropertyDto>
        {
            new() { IdOwner = "OWN-1", Name = "Property", Address = "Address", Price = 100000, Image = "img.jpg" }
        };

        var pagedResponse = PagedResponse<PropertyDto>.Create(properties, 2, 10, 25); // Page 2 of 3

        _serviceMock.Setup(s => s.GetFilteredPropertiesAsync(It.IsAny<PropertyFilterDto>()))
            .ReturnsAsync(pagedResponse);

        // Act
        var result = await _controller.GetProperties(name: "test", page: 2, pageSize: 10);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        var returnedResponse = okResult!.Value as PagedResponse<PropertyDto>;
        Assert.That(returnedResponse!.TotalPages, Is.EqualTo(3));
        Assert.That(returnedResponse.HasPreviousPage, Is.True);
        Assert.That(returnedResponse.HasNextPage, Is.True);
    }
}
