# Real Estate API

A RESTful API for managing real estate properties built with .NET 9, MongoDB, and Clean Architecture principles.

## ??? Architecture

This project follows **Clean Architecture** patterns with clear separation of concerns:

```
RealEstateAPI/
??? Domain/                    # Core business entities
?   ??? Entities/             # Property, Owner, PropertyImage, PropertyTrace
??? Application/               # Business logic layer
?   ??? DTOs/                 # Data Transfer Objects
?   ??? Interfaces/           # Service and Repository contracts
?   ??? Services/             # Business logic implementation
?   ??? Mappings/             # AutoMapper profiles
??? Infrastructure/            # External services and data access
?   ??? Configuration/        # Settings and configuration
?   ??? Data/                 # MongoDB context
?   ??? Repositories/         # Data access implementation
?   ??? Middleware/           # Global exception handler
??? Controllers/               # API endpoints
??? Tests/                     # Unit tests with NUnit
?   ??? UnitTests/
?       ??? Services/
?       ??? Repositories/
?       ??? Mappings/
??? Database/                  # Database scripts and backup
    ??? seed-data.js          # Sample data script
    ??? backup/               # MongoDB backup
    ??? README.md             # Database setup guide
```

## ?? Technologies

- **.NET 9** - Latest .NET framework
- **MongoDB 3.5.0** - NoSQL document database
- **AutoMapper 15.1.0** - Object-to-object mapping
- **Swagger/OpenAPI** - API documentation
- **NUnit 4.4.0** - Unit testing framework
- **Moq 4.20.72** - Mocking framework for tests

## ? Features

- ? Clean Architecture with dependency injection
- ? Repository pattern for data access
- ? Global exception handling middleware
- ? AutoMapper for DTO mappings
- ? Comprehensive unit tests (15 tests, 100% passing)
- ? Swagger documentation
- ? CORS enabled for frontend integration
- ? Logging with ILogger
- ? MongoDB with embedded documents (Owner, Images, Traces)
- ? Optimized queries with indexes

## ?? Prerequisites

- **.NET 9 SDK** - [Download here](https://dotnet.microsoft.com/download/dotnet/9.0)
- **MongoDB** - [Download here](https://www.mongodb.com/try/download/community) or use [MongoDB Atlas](https://www.mongodb.com/cloud/atlas)
- **Visual Studio 2022** or **VS Code** with C# extension

## ?? Configuration

1. Update `appsettings.json` with your MongoDB connection:

```json
{
  "MongoDbSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "RealEstateDB",
    "CollectionName": "Properties"
  }
}
```

For MongoDB Atlas:
```json
{
  "MongoDbSettings": {
    "ConnectionString": "mongodb+srv://<user>:<password>@cluster.mongodb.net/",
    "DatabaseName": "RealEstateDB",
    "CollectionName": "Properties"
  }
}
```

## ?? Installation & Setup

### 1. Clone the repository
```bash
git clone <repository-url>
cd RealEstateAPI
```

### 2. Restore dependencies
```bash
dotnet restore
```

### 3. Setup MongoDB Database

#### Option A: Load sample data
```bash
# Using MongoDB Shell
mongosh mongodb://localhost:27017
load("Database/seed-data.js")
```

#### Option B: Restore from backup
```bash
mongorestore --db=RealEstateDB ./Database/backup/RealEstateDB
```

See `Database/README.md` for detailed instructions.

### 4. Run the application
```bash
dotnet run
```

The API will be available at:
- **HTTPS**: `https://localhost:5001`
- **HTTP**: `http://localhost:5000`
- **Swagger UI**: `https://localhost:5001` (root)

## ?? API Endpoints

### Get All Properties (with optional filters)
```http
GET /api/properties
```

**Query Parameters:**
- `name` (optional) - Filter by property name (case-insensitive, partial match)
- `address` (optional) - Filter by address (case-insensitive, partial match)
- `minPrice` (optional) - Minimum price range
- `maxPrice` (optional) - Maximum price range

**Example Requests:**
```bash
# Get all properties
GET /api/properties

# Filter by name
GET /api/properties?name=beach

# Filter by price range
GET /api/properties?minPrice=500000&maxPrice=2000000

# Combined filters
GET /api/properties?name=modern&address=miami&minPrice=1000000&maxPrice=3000000
```

**Response (200 OK):**
```json
[
  {
    "idOwner": "OWN-001",
    "name": "Modern Beach House",
    "address": "1250 Ocean Drive, Miami Beach, FL 33139",
    "price": 1250000.00,
    "image": "https://images.unsplash.com/photo-1613490493576-7fde63acd811?w=800"
  }
]
```

### Get Property Details by ID
```http
GET /api/properties/{id}
```

**Response (200 OK):**
```json
{
  "id": "507f1f77bcf86cd799439011",
  "name": "Modern Beach House",
  "address": "1250 Ocean Drive, Miami Beach, FL 33139",
  "price": 1250000.00,
  "codeInternal": "PROP-2024-001",
  "year": 2022,
  "owner": {
    "idOwner": "OWN-001",
    "name": "Maria Rodriguez",
    "address": "450 Brickell Ave, Miami, FL 33131",
    "photo": "https://i.pravatar.cc/150?img=47",
    "birthday": "1985-03-15T00:00:00Z"
  },
  "images": [
    {
      "idPropertyImage": "IMG-001-01",
      "file": "https://images.unsplash.com/photo-1613490493576-7fde63acd811?w=800",
      "enabled": true
    }
  ],
  "traces": [
    {
      "idPropertyTrace": "TRACE-001-01",
      "dateSale": "2022-06-15T00:00:00Z",
      "name": "Initial Purchase",
      "value": 1250000.00,
      "tax": 62500.00
    }
  ]
}
```

**Response (404 Not Found):**
```json
{
  "message": "Property with ID '...' not found"
}
```

## ?? Running Tests

### Run all tests
```bash
dotnet test
```

### Run with detailed output
```bash
dotnet test --logger "console;verbosity=detailed"
```

### Test Coverage
- **15 unit tests** covering:
  - ? Service layer business logic
  - ? Repository filter building
  - ? AutoMapper configurations
  - ? DTOs and mappings

**Test Results:**
```
Total: 15 | Passed: 15 | Failed: 0 | Skipped: 0
```

## ?? Sample Data

The seed data includes **10 diverse properties**:
- Price range: $385,000 - $3,800,000
- Locations: Miami Beach, Coral Gables, Key Biscayne, Coconut Grove, Doral, Pinecrest, Homestead
- Each property includes:
  - Complete owner information
  - 1-3 property images
  - 1-2 transaction traces

## ??? Database Backup

MongoDB backup is located in `/Database/backup/RealEstateDB/`

**To restore:**
```bash
mongorestore --db=RealEstateDB ./Database/backup/RealEstateDB
```

**To create a new backup:**
```bash
mongodump --db=RealEstateDB --out=./Database/backup
```

## ????? Development

This project follows **Git Flow** and **Conventional Commits**:

### Branch naming
- `feature/feature-name` - New features
- `fix/bug-description` - Bug fixes
- `refactor/description` - Code refactoring
- `test/description` - Adding tests

### Commit format
```
type(scope): message

Examples:
feat(api): add property filtering endpoint
fix(repository): correct price range query
test(service): add unit tests for property service
refactor(entities): optimize domain models
docs(readme): update installation instructions
```

### Git Workflow
```bash
# Create feature branch
git checkout -b feature/new-feature

# Make changes and commit
git add .
git commit -m "feat(scope): description"

# Push to remote
git push origin feature/new-feature
```

## ??? Clean Architecture Principles

1. **Domain Layer** - No dependencies, pure business entities
2. **Application Layer** - Depends only on Domain, contains business logic
3. **Infrastructure Layer** - Implements Application interfaces, depends on external services
4. **API Layer** - Entry point, depends on Application and Infrastructure

**Dependency Rule:** Dependencies point inward. Inner layers know nothing about outer layers.

## ?? Error Handling

Global exception handling middleware provides consistent error responses:

```json
{
  "statusCode": 500,
  "message": "An internal server error occurred",
  "details": "Error details here"
}
```

## ?? API Documentation

Interactive API documentation available via Swagger UI:
- Navigate to the root URL when running in development mode
- Test endpoints directly from the browser
- View request/response schemas

## ?? CORS Configuration

CORS is enabled for all origins in development. For production, update the CORS policy in `Program.cs`:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("Production", policy =>
    {
        policy.WithOrigins("https://yourdomain.com")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
```

## ?? Project Requirements Checklist

- ? Backend API with .NET 9 and C#
- ? MongoDB database integration
- ? Property filtering (name, address, price range)
- ? DTOs with required fields (IdOwner, Name, Address, Price, Image)
- ? Clean Architecture implementation
- ? Global error handling
- ? Optimized database queries with indexes
- ? Unit tests with NUnit
- ? Clean, maintainable code
- ? Comprehensive documentation
- ? Database backup included
- ? Setup instructions

## ?? Contact

For questions or issues, contact: **crios@millionluxury.com**

## ?? License

MIT License - Free to use for development and testing purposes.

---

**Built with ?? using .NET 9, MongoDB, and Clean Architecture**
