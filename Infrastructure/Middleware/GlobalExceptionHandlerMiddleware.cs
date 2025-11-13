using System.Net;
using System.Text.Json;
using RealEstateAPI.Application.Exceptions;

namespace RealEstateAPI.Infrastructure.Middleware;

/// <summary>
/// Global exception handling middleware for consistent error responses including database errors.
/// </summary>
public sealed class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;

    public GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlerMiddleware> logger,
        IWebHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message, errors) = exception switch
        {
            PropertyNotFoundException notFound => (
                HttpStatusCode.NotFound,
                notFound.Message,
                (IDictionary<string, string[]>?)null
            ),

            Application.Exceptions.ValidationException validation => (
                HttpStatusCode.BadRequest,
                "One or more validation errors occurred",
                validation.Errors
            ),

            ArgumentException => (
                HttpStatusCode.BadRequest,
                "Invalid request parameters",
                (IDictionary<string, string[]>?)null
            ),

            InvalidOperationException invalidOp when invalidOp.Message.Contains("Database") => (
                HttpStatusCode.ServiceUnavailable,
                "Database service is currently unavailable",
                (IDictionary<string, string[]>?)null
            ),

            _ => (
                HttpStatusCode.InternalServerError,
                "An internal server error occurred",
                (IDictionary<string, string[]>?)null
            )
        };

        context.Response.StatusCode = (int)statusCode;

        var response = new ErrorResponse
        {
            StatusCode = (int)statusCode,
            Message = message,
            Details = _environment.IsDevelopment() ? exception.Message : null,
            Errors = errors
        };

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        var json = JsonSerializer.Serialize(response, options);
        await context.Response.WriteAsync(json);
    }

    private sealed record ErrorResponse
    {
        public required int StatusCode { get; init; }
        public required string Message { get; init; }
        public string? Details { get; init; }
        public IDictionary<string, string[]>? Errors { get; init; }
    }
}
