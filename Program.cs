using RealEstateAPI.Application.Interfaces;
using RealEstateAPI.Application.Mappings;
using RealEstateAPI.Application.Services;
using RealEstateAPI.Application.Validators;
using RealEstateAPI.Infrastructure.Configuration;
using RealEstateAPI.Infrastructure.Data;
using RealEstateAPI.Infrastructure.HealthChecks;
using RealEstateAPI.Infrastructure.Middleware;
using RealEstateAPI.Infrastructure.Repositories;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Configure MongoDB settings
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection(MongoDbSettings.SectionName));

// Register MongoDB context
builder.Services.AddSingleton<MongoDbContext>();

// Register repositories
builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();

// Register services
builder.Services.AddScoped<IPropertyService, PropertyService>();

// Register validators
builder.Services.AddScoped<PropertyFilterValidator>();

// Register health check as a service
builder.Services.AddScoped<MongoDbCollectionHealthCheck>();

// Register AutoMapper compatible with 15.x
builder.Services.AddSingleton<IMapper>(sp =>
{
    var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
    var configExpression = new MapperConfigurationExpression();
    configExpression.AddProfile<PropertyMappingProfile>();
    
    var config = new MapperConfiguration(configExpression, loggerFactory);
    return config.CreateMapper();
});

// Add Health Checks with custom MongoDB collection validator
builder.Services.AddHealthChecks()
    .AddCheck<MongoDbCollectionHealthCheck>(
        name: "mongodb_collections",
        tags: new[] { "db", "mongodb", "ready" },
        timeout: TimeSpan.FromSeconds(5));

// Add controllers
builder.Services.AddControllers();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Real Estate API",
        Version = "v1",
        Description = "RESTful API for managing real estate properties with validation and pagination",
        Contact = new()
        {
            Name = "Real Estate API",
            Email = "crios@millionluxury.com"
        }
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Validate database setup on startup
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Checking MongoDB database setup...");

using (var scope = app.Services.CreateScope())
{
    var healthCheck = scope.ServiceProvider.GetRequiredService<MongoDbCollectionHealthCheck>();
    var result = await healthCheck.CheckHealthAsync(new Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckContext());
    
    if (result.Status == Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy)
    {
        logger.LogWarning("??  Database setup incomplete: {Description}", result.Description);
        logger.LogWarning("??  The API will start but may not function properly until the database is seeded.");
        logger.LogWarning("??  Run: mongosh < Database/seed-data.js");
    }
    else
    {
        logger.LogInformation("? MongoDB database is properly configured");
    }
}

// Global exception handler middleware
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Real Estate API v1");
        options.RoutePrefix = string.Empty;
    });
}

// Health check endpoints
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = System.Text.Json.JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description,
                duration = e.Value.Duration.TotalMilliseconds,
                data = e.Value.Data
            }),
            totalDuration = report.TotalDuration.TotalMilliseconds
        });
        await context.Response.WriteAsync(result);
    }
});

// Simple liveness endpoint
app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = _ => false // Don't check anything, just return 200 if app is running
});

// Readiness endpoint (checks database)
app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = async (context, report) =>
    {
        if (report.Status != Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy)
        {
            context.Response.ContentType = "application/json";
            
            var firstEntry = report.Entries.Values.FirstOrDefault();
            var errorDescription = firstEntry.Description ?? "Database not ready";
            
            var result = System.Text.Json.JsonSerializer.Serialize(new
            {
                status = report.Status.ToString(),
                error = errorDescription,
                instructions = "Please run the seed script: mongosh < Database/seed-data.js"
            });
            await context.Response.WriteAsync(result);
        }
        else
        {
            await context.Response.WriteAsync("Ready");
        }
    }
});

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
