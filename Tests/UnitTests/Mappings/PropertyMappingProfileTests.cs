using AutoMapper;
using NUnit.Framework;
using RealEstateAPI.Application.DTOs;
using RealEstateAPI.Application.Mappings;
using RealEstateAPI.Domain.Entities;

namespace RealEstateAPI.Tests.UnitTests.Mappings;

/// <summary>
/// Unit tests for AutoMapper configuration and mappings with owner references.
/// </summary>
[TestFixture]
public class PropertyMappingProfileTests
{
    private IMapper _mapper = null!;

    [SetUp]
    public void Setup()
    {
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var configExpression = new MapperConfigurationExpression();
        configExpression.AddProfile<PropertyMappingProfile>();
        
        var config = new MapperConfiguration(configExpression, loggerFactory);
        _mapper = config.CreateMapper();
    }

    [Test]
    public void MappingProfile_Configuration_ShouldBeValid()
    {
        // Arrange
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var configExpression = new MapperConfigurationExpression();
        configExpression.AddProfile<PropertyMappingProfile>();
        
        var config = new MapperConfiguration(configExpression, loggerFactory);

        // Act & Assert
        config.AssertConfigurationIsValid();
    }

    [Test]
    public void Map_Property_To_PropertyDto_ShouldMapCorrectly()
    {
        // Arrange
        var property = new Property
        {
            Id = "507f1f77bcf86cd799439011",
            Name = "Beach House",
            Address = "123 Ocean Drive",
            Price = 450000,
            CodeInternal = "PROP-001",
            Year = 2022,
            OwnerId = "OWN-001",
            Images = new List<PropertyImage>
            {
                new()
                {
                    IdPropertyImage = "IMG-001",
                    File = "https://example.com/image1.jpg",
                    Enabled = true
                },
                new()
                {
                    IdPropertyImage = "IMG-002",
                    File = "https://example.com/image2.jpg",
                    Enabled = false
                }
            }
        };

        // Act
        var result = _mapper.Map<PropertyDto>(property);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Slug, Is.EqualTo("beach-house"));
        Assert.That(result.IdOwner, Is.EqualTo("OWN-001"));
        Assert.That(result.Name, Is.EqualTo("Beach House"));
        Assert.That(result.Address, Is.EqualTo("123 Ocean Drive"));
        Assert.That(result.Price, Is.EqualTo(450000));
        Assert.That(result.Image, Is.EqualTo("https://example.com/image1.jpg"));
    }

    [Test]
    public void Map_Property_To_PropertyDetailDto_ShouldMapCorrectly()
    {
        // Arrange
        var property = new Property
        {
            Id = "507f1f77bcf86cd799439011",
            Name = "Beach House",
            Address = "123 Ocean Drive",
            Price = 450000,
            CodeInternal = "PROP-001",
            Year = 2022,
            OwnerId = "OWN-001",
            Images = new List<PropertyImage>
            {
                new()
                {
                    IdPropertyImage = "IMG-001",
                    File = "https://example.com/image1.jpg",
                    Enabled = true
                }
            },
            Traces = new List<PropertyTrace>
            {
                new()
                {
                    IdPropertyTrace = "TRACE-001",
                    DateSale = new DateTime(2022, 6, 15),
                    Name = "Initial Sale",
                    Value = 450000,
                    Tax = 22500
                }
            }
        };

        // Act
        var result = _mapper.Map<PropertyDetailDto>(property);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo("507f1f77bcf86cd799439011"));
        Assert.That(result.Slug, Is.EqualTo("beach-house"));
        Assert.That(result.IdOwner, Is.EqualTo("OWN-001"));
        Assert.That(result.Name, Is.EqualTo("Beach House"));
        Assert.That(result.CodeInternal, Is.EqualTo("PROP-001"));
        Assert.That(result.Year, Is.EqualTo(2022));
        Assert.That(result.Images.Count, Is.EqualTo(1));
        // Traces are no longer part of PropertyDetailDto - they're fetched separately via /api/properties/{slug}/traces
    }

    [Test]
    public void Map_Owner_To_OwnerDto_ShouldMapCorrectly()
    {
        // Arrange
        var owner = new Owner
        {
            Id = "OWN-001",
            Name = "Jane Smith",
            Address = "789 Park Ave",
            Photo = "https://example.com/photo.jpg",
            Birthday = new DateTime(1985, 3, 20)
        };

        // Act
        var result = _mapper.Map<OwnerDto>(owner);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.IdOwner, Is.EqualTo("OWN-001"));
        Assert.That(result.Name, Is.EqualTo("Jane Smith"));
        Assert.That(result.Address, Is.EqualTo("789 Park Ave"));
        Assert.That(result.Photo, Is.EqualTo("https://example.com/photo.jpg"));
        Assert.That(result.Birthday, Is.EqualTo(new DateTime(1985, 3, 20)));
    }
}
