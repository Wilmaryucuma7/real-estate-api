# Real Estate API

A RESTful API for managing real estate properties built with .NET 9, MongoDB, and Clean Architecture principles.

## ?? Architecture

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
?   ??? Validators/           # FluentValidation rules
?   ??? Exceptions/           # Custom exceptions
??? Infrastructure/            # External services and data access
?   ??? Configuration/        # Settings and configuration
?   ??? Data/                 # MongoDB context
?   ??? Repositories/         # Data access implementation
?   ??? Middleware/           # Global exception handler
??? Controllers/               # API endpoints
??? Tests/                     # Unit tests with NUnit
?   ??? UnitTests/
?       ??? Services/
?       ??? Controllers/
?       ??? Middleware/
?       ??? Validators/
?       ??? Repositories/
?       ??? Mappings/
??? Database/                  # Database scripts and backup
    ??? seed-data.js          # Sample data script
    ??? backup/               # MongoDB backup
    ??? README.md             # Database setup guide
```

## ??? Database Design

This API uses **MongoDB** with a hybrid approach: **separate collections** for reusable entities and **embedded documents** for dependent data.

| Entity | MongoDB Strategy | Reason |
|--------|------------------|--------|
| **Owner** | ??? Separate Collection | Can own multiple properties (reusable) |
| **Property** | ??? Main Collection | Root entity |
| **PropertyImage** | ?? Embedded in Property | Always depends on a property |
| **PropertyTrace** | ?? Embedded in Property | Always depends on a property |

### Collections Structure

**Owners Collection:**
```json
{
  "_id": "OWN-001",
  "name": "Maria Rodriguez",
  "address": "450 Brickell Ave, Miami, FL 33131",
  "photo": "https://i.pravatar.cc/150?img=47",
  "birthday": "1985-03-15T00:00:00Z"
}
```

**Properties Collection** (with Owner reference):
```json
{
  "_id": ObjectId("..."),
  "name": "Modern Beach House",
  "address": "1250 Ocean Drive, Miami Beach, FL 33139",
  "price": 1250000.00,
  "codeInternal": "PROP-2024-001",
  "year": 2022,
  "ownerId": "OWN-001",  // Foreign key reference
  "images": [
    {
      "idPropertyImage": "IMG-001-01",
      "file": "https://images.unsplash.com/...",
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

### Relationship Benefits
- ? **One owner ? Many properties** relationship
- ? Data normalization and integrity
- ? Reusable owner information
- ? Optimized queries with indexes on `ownerId`
- ? Efficient joins in application layer

## ?? Technologies

- **.NET 9** - Latest .NET framework
- **MongoDB 3.5.0** - NoSQL document database
- **AutoMapper 15.1.0** - Object-to-object mapping
- **FluentValidation 12.1.0** - Input validation
- **Swagger/OpenAPI** - API documentation
- **NUnit 4.4.0** - Unit testing framework
- **Moq 4.20.72** - Mocking framework for tests

## ? Features

- ? Clean Architecture with dependency injection
- ? Repository pattern for data access
- ? **Separate Owner collection** with foreign key relationships
- ? Global exception handling middleware
- ? Custom exceptions (PropertyNotFoundException, ValidationException)
- ? FluentValidation with security constraints (XSS/SQL injection prevention)
- ? AutoMapper for DTO mappings
- ? **Pagination with metadata** for frontend integration
- ? Comprehensive unit tests (49 tests, 100% passing)
- ? Swagger documentation
- ? CORS enabled for frontend integration
- ? Structured logging with ILogger
- ? MongoDB with hybrid approach (references + embedded documents)
- ? Optimized queries with indexes
- ? Support for accented characters (á, é, í, ó, ú, ñ)

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

#### Option A: Load sample data (creates both collections)
```bash
# Using MongoDB Shell
mongosh mongodb://localhost:27017
use RealEstateDB
load("Database/seed-data.js")
```

The seed script creates:
- ? **Owners collection** with 10 owners
- ? **Properties collection** with 10 properties (referencing owners)
- ? Indexes on both collections for optimization

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
- **HTTPS**: `https://localhost:7026`
- **HTTP**: `http://localhost:5202`
- **Swagger UI**: `https://localhost:7026` (root)

## ?? API Endpoints

### Get All Properties (with pagination and filters)
```http
GET /api/properties?page=1&pageSize=10
```

**Query Parameters:**
- `name` (optional) - Filter by property name (case-insensitive, partial match)
- `address` (optional) - Filter by address (case-insensitive, partial match)
- `minPrice` (optional) - Minimum price range
- `maxPrice` (optional) - Maximum price range
- `page` (optional, default: 1) - Page number
- `pageSize` (optional, default: 10, max: 100) - Items per page

**Example Requests:**
```bash
# Get first page (10 items)
GET /api/properties?page=1&pageSize=10

# Filter by name with pagination
GET /api/properties?name=beach&page=1&pageSize=5

# Filter by price range
GET /api/properties?minPrice=500000&maxPrice=2000000&page=1&pageSize=10

# Combined filters
GET /api/properties?name=modern&address=miami&minPrice=1000000&page=2&pageSize=10
```

**Response (200 OK) - Paginated:**
```json
{
  "data": [
    {
      "idOwner": "OWN-001",
      "name": "Modern Beach House",
      "address": "1250 Ocean Drive, Miami Beach, FL 33139",
      "price": 1250000.00,
      "image": "https://images.unsplash.com/photo-1613490493576-7fde63acd811?w=800"
    }
  ],
  "page": 1,
  "pageSize": 10,
  "totalCount": 45,
  "totalPages": 5,
  "hasPreviousPage": false,
  "hasNextPage": true
}
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
  "statusCode": 404,
  "message": "Property with ID '...' was not found.",
  "details": null
}
```

## ?? Pagination Guide for Frontend

### Response Structure

All paginated endpoints return a `PagedResponse` with the following metadata:

| Property | Type | Description | Example |
|----------|------|-------------|---------|
| `data` | Array | Items for current page | `[{...}, {...}]` |
| `page` | number | Current page number (1-indexed) | `2` |
| `pageSize` | number | Items per page | `10` |
| `totalCount` | number | Total items across all pages | `45` |
| `totalPages` | number | Total number of pages | `5` |
| `hasPreviousPage` | boolean | Can navigate backward? | `true` |
| `hasNextPage` | boolean | Can navigate forward? | `true` |

### Frontend Implementation Example (React)

```tsx
import { useState, useEffect } from 'react';

interface PagedResponse<T> {
  data: T[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

interface Property {
  idOwner: string;
  name: string;
  address: string;
  price: number;
  image: string;
}

function PropertyList() {
  const [properties, setProperties] = useState<PagedResponse<Property> | null>(null);
  const [currentPage, setCurrentPage] = useState(1);

  const fetchProperties = async (page: number) => {
    const response = await fetch(
      `https://localhost:7026/api/properties?page=${page}&pageSize=10`
    );
    const data = await response.json();
    setProperties(data);
  };

  useEffect(() => {
    fetchProperties(currentPage);
  }, [currentPage]);

  return (
    <div>
      {properties?.data.map(property => (
        <div key={property.idOwner}>{property.name}</div>
      ))}

      <div className="pagination">
        <button
          disabled={!properties?.hasPreviousPage}
          onClick={() => setCurrentPage(prev => prev - 1)}
        >
          Previous
        </button>

        <span>
          Page {properties?.page} of {properties?.totalPages}
        </span>

        <button
          disabled={!properties?.hasNextPage}
          onClick={() => setCurrentPage(prev => prev + 1)}
        >
          Next
        </button>
      </div>
    </div>
  );
}
```

### Usage Tips

1. **Use `totalCount`** to display "Showing X results"
2. **Use `totalPages`** to render page number buttons
3. **Check `hasPreviousPage` and `hasNextPage`** before enabling navigation
4. **Preserve filters** when changing pages
5. **Page size limit**: Max 100 items per page (enforced by API)

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
- **49 unit tests** covering:
  - ? Service layer with owner relationships (6 tests)
  - ? Controller endpoints (15 tests)
  - ? Middleware exception handling (9 tests)
  - ? FluentValidation rules (15 tests)
  - ? AutoMapper configurations (4 tests)

**Test Results:**
```
Total: 49 | Passed: 49 | Failed: 0 | Skipped: 0
Duration: ~2 seconds
```

## ??? Sample Data

The seed data includes:
- **10 owners** (in separate Owners collection)
- **10 properties** (referencing owners via `ownerId`)
- Price range: $385,000 - $3,800,000
- Locations: Miami Beach, Coral Gables, Key Biscayne, Coconut Grove, Doral, Pinecrest, Homestead
- Each property includes:
  - Owner reference (`ownerId`)
  - 1-3 property images (embedded)
  - 1-2 transaction traces (embedded)

## ?? Database Backup

MongoDB backup is located in `/Database/backup/RealEstateDB/`

**To restore:**
```bash
mongorestore --db=RealEstateDB ./Database/backup/RealEstateAPI
```

**To create a new backup:**
```bash
mongodump --db=RealEstateDB --out=./Database/backup
```

## ?? Development

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
refactor(domain): separate owners into independent collection
test(service): add unit tests for property service
docs(readme): update installation instructions
```

### Git Workflow
```bash
# Create feature branch
git checkout develop
git checkout -b feature/new-feature

# Make changes and commit
git add .
git commit -m "feat(scope): description"

# Push to remote
git push origin feature/new-feature

# Merge to develop
git checkout develop
git merge feature/new-feature --no-ff
```

## ??? Clean Architecture Principles

1. **Domain Layer** - No dependencies, pure business entities
2. **Application Layer** - Depends only on Domain, contains business logic
3. **Infrastructure Layer** - Implements Application interfaces, depends on external services
4. **API Layer** - Entry point, depends on Application and Infrastructure

**Dependency Rule:** Dependencies point inward. Inner layers know nothing about outer layers.

## ?? Error Handling

Global exception handling middleware provides consistent error responses:

**Validation Error (400):**
```json
{
  "statusCode": 400,
  "message": "One or more validation errors occurred",
  "errors": {
    "Name": ["Property name contains invalid characters"]
  }
}
```

**Not Found (404):**
```json
{
  "statusCode": 404,
  "message": "Property with ID 'xyz' was not found."
}
```

**Server Error (500):**
```json
{
  "statusCode": 500,
  "message": "An internal server error occurred",
  "details": "Stack trace here (only in Development)"
}
```

### Security Features

- ? **No sensitive data exposure** in production (details hidden)
- ? **Input validation** with FluentValidation
- ? **XSS prevention** - Special characters blocked
- ? **SQL injection prevention** - Parameterized queries
- ? **Regex validation** for text inputs

## ?? API Documentation

Interactive API documentation available via Swagger UI:
- Navigate to the root URL when running in development mode
- Test endpoints directly from the browser
- View request/response schemas
- See validation rules

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

## ? Project Requirements Checklist

- ? Backend API with .NET 9 and C#
- ? MongoDB database integration with normalized data model
- ? **Separate Owner collection** with foreign key relationships
- ? Property filtering (name, address, price range)
- ? **Pagination with metadata**
- ? DTOs with required fields (IdOwner, Name, Address, Price, Image)
- ? Clean Architecture implementation
- ? **Custom exception handling** (PropertyNotFoundException, ValidationException)
- ? **FluentValidation** with security rules
- ? Optimized database queries with indexes
- ? **49 comprehensive unit tests** with NUnit
- ? Clean, maintainable code following SOLID principles
- ? Comprehensive documentation
- ? Database backup included
- ? Setup instructions

## ?? Contact

For questions or issues, contact: **crios@millionluxury.com**

## ?? License

MIT License - Free to use for development and testing purposes.

---

**Built with ?? using .NET 9, MongoDB, Clean Architecture, and FluentValidation**
