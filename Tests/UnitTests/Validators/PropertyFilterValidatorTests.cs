using NUnit.Framework;
using RealEstateAPI.Application.DTOs;
using RealEstateAPI.Application.Validators;

namespace RealEstateAPI.Tests.UnitTests.Validators;

/// <summary>
/// Unit tests for PropertyFilterValidator.
/// </summary>
[TestFixture]
public class PropertyFilterValidatorTests
{
    private PropertyFilterValidator _validator = null!;

    [SetUp]
    public void Setup()
    {
        _validator = new PropertyFilterValidator();
    }

    [Test]
    public async Task Validate_WithValidFilter_PassesValidation()
    {
        // Arrange
        var filter = new PropertyFilterDto
        {
            Name = "Beach House",
            Address = "Ocean Drive 123",
            MinPrice = 100000,
            MaxPrice = 500000,
            Page = 1,
            PageSize = 10
        };

        // Act
        var result = await _validator.ValidateAsync(filter);

        // Assert
        Assert.That(result.IsValid, Is.True);
    }

    [Test]
    public async Task Validate_WithAccentedCharacters_PassesValidation()
    {
        // Arrange
        var filter = new PropertyFilterDto
        {
            Name = "Casa en Bogotá",
            Address = "Calle José María número 5"
        };

        // Act
        var result = await _validator.ValidateAsync(filter);

        // Assert
        Assert.That(result.IsValid, Is.True);
    }

    [Test]
    public async Task Validate_WithSpecialCharacters_FailsValidation()
    {
        // Arrange
        var filter = new PropertyFilterDto
        {
            Name = "Property<script>alert('xss')</script>"
        };

        // Act
        var result = await _validator.ValidateAsync(filter);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Any(e => e.PropertyName == "Name"), Is.True);
    }

    [Test]
    public async Task Validate_WithSQLInjection_FailsValidation()
    {
        // Arrange
        var filter = new PropertyFilterDto
        {
            Address = "123 Main St'; DROP TABLE Properties;--"
        };

        // Act
        var result = await _validator.ValidateAsync(filter);

        // Assert
        Assert.That(result.IsValid, Is.False);
    }

    [Test]
    public async Task Validate_WithTooLongName_FailsValidation()
    {
        // Arrange
        var filter = new PropertyFilterDto
        {
            Name = new string('a', 101) // 101 characters
        };

        // Act
        var result = await _validator.ValidateAsync(filter);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Any(e => e.ErrorMessage.Contains("100 characters")), Is.True);
    }

    [Test]
    public async Task Validate_WithNegativeMinPrice_FailsValidation()
    {
        // Arrange
        var filter = new PropertyFilterDto
        {
            MinPrice = -100
        };

        // Act
        var result = await _validator.ValidateAsync(filter);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Any(e => e.PropertyName == "MinPrice"), Is.True);
    }

    [Test]
    public async Task Validate_WithNegativeMaxPrice_FailsValidation()
    {
        // Arrange
        var filter = new PropertyFilterDto
        {
            MaxPrice = -500
        };

        // Act
        var result = await _validator.ValidateAsync(filter);

        // Assert
        Assert.That(result.IsValid, Is.False);
    }

    [Test]
    public async Task Validate_WithMaxPriceLessThanMinPrice_FailsValidation()
    {
        // Arrange
        var filter = new PropertyFilterDto
        {
            MinPrice = 500000,
            MaxPrice = 100000
        };

        // Act
        var result = await _validator.ValidateAsync(filter);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Any(e => e.ErrorMessage.Contains("greater than or equal to minimum")), Is.True);
    }

    [Test]
    public async Task Validate_WithZeroPage_FailsValidation()
    {
        // Arrange
        var filter = new PropertyFilterDto
        {
            Page = 0
        };

        // Act
        var result = await _validator.ValidateAsync(filter);

        // Assert
        Assert.That(result.IsValid, Is.False);
    }

    [Test]
    public async Task Validate_WithPageSizeTooLarge_FailsValidation()
    {
        // Arrange
        var filter = new PropertyFilterDto
        {
            PageSize = 150 // Max is 100
        };

        // Act
        var result = await _validator.ValidateAsync(filter);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Any(e => e.ErrorMessage.Contains("between 1 and 100")), Is.True);
    }

    [Test]
    public async Task Validate_WithEmptyFilter_PassesValidation()
    {
        // Arrange
        var filter = new PropertyFilterDto();

        // Act
        var result = await _validator.ValidateAsync(filter);

        // Assert
        Assert.That(result.IsValid, Is.True);
    }

    [Test]
    public async Task Validate_WithNumbersInName_PassesValidation()
    {
        // Arrange
        var filter = new PropertyFilterDto
        {
            Name = "Property 123",
            Address = "Street 45B"
        };

        // Act
        var result = await _validator.ValidateAsync(filter);

        // Assert
        Assert.That(result.IsValid, Is.True);
    }

    [Test]
    public async Task Validate_WithHyphensAndDots_PassesValidation()
    {
        // Arrange
        var filter = new PropertyFilterDto
        {
            Name = "Modern-Style Property",
            Address = "Ave. Principal No. 123-B"
        };

        // Act
        var result = await _validator.ValidateAsync(filter);

        // Assert
        Assert.That(result.IsValid, Is.True);
    }
}
