using RealEstateAPI.Application.Interfaces;
using RealEstateAPI.Application.Mappings;
using RealEstateAPI.Application.Services;
using RealEstateAPI.Application.Validators;
using RealEstateAPI.Infrastructure.Configuration;
using RealEstateAPI.Infrastructure.Data;
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

// Register services
builder.Services.AddScoped<IPropertyService, PropertyService>();

// Register validators
builder.Services.AddScoped<PropertyFilterValidator>();

// Register AutoMapper compatible with 15.x
builder.Services.AddSingleton<IMapper>(sp =>
{
    var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
    var configExpression = new MapperConfigurationExpression();
    configExpression.AddProfile<PropertyMappingProfile>();
    
    var config = new MapperConfiguration(configExpression, loggerFactory);
    return config.CreateMapper();
});

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

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
