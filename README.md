# Real Estate API

A RESTful API for managing real estate properties built with .NET 9, MongoDB, and Clean Architecture principles.

## ??? Architecture

This project follows Clean Architecture patterns with the following layers:
- **Domain**: Core business entities
- **Application**: Business logic, DTOs, and service interfaces
- **Infrastructure**: Data access and external services
- **API**: Controllers and HTTP endpoints

## ?? Technologies

- .NET 9
- MongoDB 3.5.0
- AutoMapper 15.1.0
- Swagger/OpenAPI
- NUnit for testing

## ?? Prerequisites

- .NET 9 SDK
- MongoDB (local or Atlas)
- Visual Studio 2022 or VS Code

## ?? Configuration

Update `appsettings.json` with your MongoDB connection:

```json
{
  "MongoDbSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "RealEstateDB",
    "CollectionName": "Properties"
  }
}
```

## ?? Installation

1. Clone the repository
2. Install dependencies: `dotnet restore`
3. Update MongoDB connection in `appsettings.json`
4. Run the application: `dotnet run`

## ?? API Endpoints

- `GET /api/properties` - Get all properties with optional filters
- `GET /api/properties/{id}` - Get property details by ID

### Query Parameters for Filtering
- `name` - Filter by property name
- `address` - Filter by property address
- `minPrice` - Minimum price range
- `maxPrice` - Maximum price range

## ?? Running Tests

```bash
dotnet test
```

## ?? Database Backup

MongoDB backup is included in the `/backup` directory.

To restore:
```bash
mongorestore --db=RealEstateDB ./backup/RealEstateDB
```

## ????? Development

This project follows Git Flow and Conventional Commits:
- Feature branches: `feature/feature-name`
- Commit format: `type(scope): message`

## ?? License

MIT
