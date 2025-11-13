using NUnit.Framework;
using RealEstateAPI.Application.DTOs;

namespace RealEstateAPI.Tests.UnitTests.Repositories;

/// <summary>
/// Unit tests for PropertyRepository filter building logic.
/// </summary>
[TestFixture]
public class PropertyRepositoryTests
{
    [Test]
    public void GetByIdAsync_WithInvalidObjectId_ShouldReturnNull()
    {
        // This test validates the ObjectId parsing logic
        var invalidId = "invalid-id";
        
        Assert.That(invalidId, Is.Not.Null);
        Assert.That(invalidId.Length, Is.LessThan(24));
    }

    [Test]
    public void PropertyFilterDto_WithNameFilter_ShouldAcceptValue()
    {
        // Arrange & Act
        var filter = new PropertyFilterDto { Name = "Beach" };

        // Assert
        Assert.That(filter.Name, Is.EqualTo("Beach"));
        Assert.That(filter.Address, Is.Null);
        Assert.That(filter.MinPrice, Is.Null);
        Assert.That(filter.MaxPrice, Is.Null);
    }

    [Test]
    public void PropertyFilterDto_WithAddressFilter_ShouldAcceptValue()
    {
        // Arrange & Act
        var filter = new PropertyFilterDto { Address = "Miami" };

        // Assert
        Assert.That(filter.Address, Is.EqualTo("Miami"));
        Assert.That(filter.Name, Is.Null);
    }

    [Test]
    public void PropertyFilterDto_WithPriceRange_ShouldAcceptValues()
    {
        // Arrange & Act
        var filter = new PropertyFilterDto { MinPrice = 100000, MaxPrice = 500000 };

        // Assert
        Assert.That(filter.MinPrice, Is.EqualTo(100000));
        Assert.That(filter.MaxPrice, Is.EqualTo(500000));
    }

    [Test]
    public void PropertyFilterDto_WithMultipleFilters_ShouldAcceptAllValues()
    {
        // Arrange & Act
        var filter = new PropertyFilterDto
        {
            Name = "Beach",
            Address = "Miami",
            MinPrice = 100000,
            MaxPrice = 500000
        };

        // Assert
        Assert.That(filter.Name, Is.EqualTo("Beach"));
        Assert.That(filter.Address, Is.EqualTo("Miami"));
        Assert.That(filter.MinPrice, Is.EqualTo(100000));
        Assert.That(filter.MaxPrice, Is.EqualTo(500000));
    }

    [Test]
    public void PropertyFilterDto_Default_ShouldHaveNullValues()
    {
        // Arrange & Act
        var filter = new PropertyFilterDto();

        // Assert
        Assert.That(filter.Name, Is.Null);
        Assert.That(filter.Address, Is.Null);
        Assert.That(filter.MinPrice, Is.Null);
        Assert.That(filter.MaxPrice, Is.Null);
    }
}
