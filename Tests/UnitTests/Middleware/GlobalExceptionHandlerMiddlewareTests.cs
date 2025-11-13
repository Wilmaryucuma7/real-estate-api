using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RealEstateAPI.Application.Exceptions;
using RealEstateAPI.Infrastructure.Middleware;
using System.Text.Json;

namespace RealEstateAPI.Tests.UnitTests.Middleware;

/// <summary>
/// Unit tests for GlobalExceptionHandlerMiddleware.
/// </summary>
[TestFixture]
public class GlobalExceptionHandlerMiddlewareTests
{
    private Mock<ILogger<GlobalExceptionHandlerMiddleware>> _loggerMock = null!;
    private Mock<IWebHostEnvironment> _environmentMock = null!;
    private DefaultHttpContext _context = null!;

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<GlobalExceptionHandlerMiddleware>>();
        _environmentMock = new Mock<IWebHostEnvironment>();
        _context = new DefaultHttpContext();
        _context.Response.Body = new MemoryStream();
    }

    [Test]
    public async Task InvokeAsync_WithNoException_CallsNextMiddleware()
    {
        // Arrange
        var nextCalled = false;
        RequestDelegate next = (HttpContext ctx) =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        };

        _environmentMock.Setup(e => e.EnvironmentName).Returns("Development");

        var middleware = new GlobalExceptionHandlerMiddleware(
            next,
            _loggerMock.Object,
            _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(_context);

        // Assert
        Assert.That(nextCalled, Is.True);
        Assert.That(_context.Response.StatusCode, Is.EqualTo(200));
    }

    [Test]
    public async Task InvokeAsync_WithPropertyNotFoundException_Returns404()
    {
        // Arrange
        var propertyId = "test-id";
        RequestDelegate next = (HttpContext ctx) =>
        {
            throw new PropertyNotFoundException(propertyId);
        };

        _environmentMock.Setup(e => e.EnvironmentName).Returns("Development");

        var middleware = new GlobalExceptionHandlerMiddleware(
            next,
            _loggerMock.Object,
            _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(_context);

        // Assert
        Assert.That(_context.Response.StatusCode, Is.EqualTo(404));
        Assert.That(_context.Response.ContentType, Is.EqualTo("application/json"));

        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(_context.Response.Body).ReadToEndAsync();
        Assert.That(responseBody, Does.Contain(propertyId));
    }

    [Test]
    public async Task InvokeAsync_WithValidationException_Returns400WithErrors()
    {
        // Arrange
        var errors = new Dictionary<string, string[]>
        {
            { "Name", new[] { "Name is required" } },
            { "Price", new[] { "Price must be positive" } }
        };

        RequestDelegate next = (HttpContext ctx) =>
        {
            throw new ValidationException("Validation failed", errors);
        };

        _environmentMock.Setup(e => e.EnvironmentName).Returns("Development");

        var middleware = new GlobalExceptionHandlerMiddleware(
            next,
            _loggerMock.Object,
            _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(_context);

        // Assert
        Assert.That(_context.Response.StatusCode, Is.EqualTo(400));

        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(_context.Response.Body).ReadToEndAsync();
        var response = JsonSerializer.Deserialize<JsonElement>(responseBody);
        
        Assert.That(response.GetProperty("errors").ValueKind, Is.EqualTo(JsonValueKind.Object));
    }

    [Test]
    public async Task InvokeAsync_WithArgumentException_Returns400()
    {
        // Arrange
        RequestDelegate next = (HttpContext ctx) =>
        {
            throw new ArgumentException("Invalid argument");
        };

        _environmentMock.Setup(e => e.EnvironmentName).Returns("Development");

        var middleware = new GlobalExceptionHandlerMiddleware(
            next,
            _loggerMock.Object,
            _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(_context);

        // Assert
        Assert.That(_context.Response.StatusCode, Is.EqualTo(400));
    }

    [Test]
    public async Task InvokeAsync_WithGenericException_Returns500()
    {
        // Arrange
        RequestDelegate next = (HttpContext ctx) =>
        {
            throw new Exception("Something went wrong");
        };

        _environmentMock.Setup(e => e.EnvironmentName).Returns("Development");

        var middleware = new GlobalExceptionHandlerMiddleware(
            next,
            _loggerMock.Object,
            _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(_context);

        // Assert
        Assert.That(_context.Response.StatusCode, Is.EqualTo(500));
    }

    [Test]
    public async Task InvokeAsync_InProduction_DoesNotExposeExceptionDetails()
    {
        // Arrange
        RequestDelegate next = (HttpContext ctx) =>
        {
            throw new Exception("Sensitive internal error");
        };

        _environmentMock.Setup(e => e.EnvironmentName).Returns("Production");

        var middleware = new GlobalExceptionHandlerMiddleware(
            next,
            _loggerMock.Object,
            _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(_context);

        // Assert
        Assert.That(_context.Response.StatusCode, Is.EqualTo(500));

        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(_context.Response.Body).ReadToEndAsync();
        
        // Should not contain the actual exception message
        Assert.That(responseBody, Does.Not.Contain("Sensitive internal error"));
        Assert.That(responseBody, Does.Contain("internal server error"));
    }

    [Test]
    public async Task InvokeAsync_InDevelopment_ExposesExceptionDetails()
    {
        // Arrange
        var exceptionMessage = "Detailed error for debugging";
        RequestDelegate next = (HttpContext ctx) =>
        {
            throw new Exception(exceptionMessage);
        };

        _environmentMock.Setup(e => e.EnvironmentName).Returns("Development");

        var middleware = new GlobalExceptionHandlerMiddleware(
            next,
            _loggerMock.Object,
            _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(_context);

        // Assert
        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(_context.Response.Body).ReadToEndAsync();
        
        // Should contain the actual exception message in development
        Assert.That(responseBody, Does.Contain(exceptionMessage));
    }

    [Test]
    public async Task InvokeAsync_LogsException()
    {
        // Arrange
        RequestDelegate next = (HttpContext ctx) =>
        {
            throw new Exception("Test exception");
        };

        _environmentMock.Setup(e => e.EnvironmentName).Returns("Development");

        var middleware = new GlobalExceptionHandlerMiddleware(
            next,
            _loggerMock.Object,
            _environmentMock.Object);

        // Act
        await middleware.InvokeAsync(_context);

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
