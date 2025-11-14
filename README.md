# Real Estate API

A high-performance RESTful API for managing real estate properties, built with .NET 9, MongoDB, and Clean Architecture.

## ?? Quick Start

### Option 1: Docker (Recommended) ??

**Everything in one command:**
```bash
docker-compose up -d
```

**Access:**
- API: http://localhost:5000
- Swagger: http://localhost:5000/swagger
- MongoDB: mongodb://localhost:27017

Sample data (10 owners + 10 properties) loads automatically!

See [DOCKER.md](DOCKER.md) for details.

---

### Option 2: Local Development

#### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [MongoDB](https://www.mongodb.com/try/download/community) running on `localhost:27017`

#### Installation

```bash
# 1. Clone repository
git clone <repository-url>
cd RealEstateAPI

# 2. Restore packages
dotnet restore

# 3. Configure MongoDB (optional - already configured for local development)
# Option A: Use default configuration in appsettings.Development.json
# Option B: Override with User Secrets
dotnet user-secrets set "MongoDb:ConnectionString" "mongodb://localhost:27017"

# 4. Seed database
mongosh < Database/seed-data.js

# 5. Run
dotnet run
```

**API available at:** `https://localhost:7193`

---

## ?? API Endpoints

### Properties

```http
# List properties with filters and pagination
GET /api/properties?page=1&pageSize=10&name=beach&minPrice=500000&maxPrice=2000000

# Get property by slug
GET /api/properties/{slug}

# Get property transaction history
GET /api/properties/{slug}/traces
```

### Owners

```http
# List owners
GET /api/owners?page=1&pageSize=10

# Get owner by ID
GET /api/owners/{id}

# Get owner's properties
GET /api/owners/{id}/properties
```

**Example Response:**
```json
{
  "data": [
    {
      "slug": "modern-beach-house",
      "idOwner": "OWN-001",
      "name": "Modern Beach House",
      "price": 1250000.00,
      "image": "https://images.unsplash.com/..."
    }
  ],
  "page": 1,
  "pageSize": 10,
  "totalCount": 20,
  "totalPages": 2
}
```

---

## ??? Architecture

Clean Architecture with separation of concerns:

```
Domain/              # Business entities
Application/         # Business logic, DTOs, interfaces
Infrastructure/      # Data access, external services
Controllers/         # API endpoints
Tests/              # Unit tests (49 tests)
```

---

## ?? Configuration

### Development (Default)

Uses `appsettings.Development.json` automatically:
```json
{
  "MongoDb": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "RealEstateDB"
  }
}
```

### Production

Use environment variables:
```bash
# Windows
$env:MONGODB__CONNECTIONSTRING="mongodb+srv://user:pass@cluster.mongodb.net/"
$env:MONGODB__DATABASENAME="RealEstateDB"

# Linux/Mac
export MONGODB__CONNECTIONSTRING="mongodb+srv://user:pass@cluster.mongodb.net/"
export MONGODB__DATABASENAME="RealEstateDB"
```

### User Secrets (Optional)

Override development settings:
```bash
dotnet user-secrets set "MongoDb:ConnectionString" "your-connection-string"
dotnet user-secrets set "MongoDb:DatabaseName" "your-database-name"
```

---

## ?? Testing

```bash
# Run all tests (49 tests)
dotnet test

# With verbose output
dotnet test --logger "console;verbosity=detailed"

# With Docker
docker-compose exec api dotnet test
```

---

## ?? Database

**Collections:**
- **Owners** (10 records)
- **Properties** (20 records with slugs)

**Indexes:**
- `slug` (unique) - Fast lookups
- `ownerId` - Fast joins
- `price` - Range queries
- Full-text search on `name` and `address`

**Sample data included** - Run seed script or use Docker.

---

## ?? Features

- ? Clean Architecture with DI
- ? Repository Pattern
- ? Slug-based SEO URLs (94% faster)
- ? Advanced filtering & pagination
- ? FluentValidation (XSS/SQLi prevention)
- ? Global exception handling
- ? MongoDB optimizations
- ? User Secrets support
- ? Docker support with auto-seeding
- ? Swagger documentation
- ? 49 unit tests (100% passing)

---

## ?? Troubleshooting

**MongoDB connection fails:**
```bash
# Check MongoDB is running
mongosh mongodb://localhost:27017

# With Docker
docker-compose ps
docker-compose logs mongodb

# Verify configuration
dotnet user-secrets list
```

**Missing collections:**
```bash
# Local
mongosh < Database/seed-data.js

# Docker (automatic on first run)
docker-compose down -v
docker-compose up -d
```

---

## ?? Contact

**Email:** wilkrack7@gmail.com


