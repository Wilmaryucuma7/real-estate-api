# Database Setup Guide

## Prerequisites
- MongoDB installed locally or MongoDB Atlas account
- MongoDB Shell (`mongosh`) or MongoDB Compass

## Option 1: Load Seed Data

### Using MongoDB Shell
```bash
# Connect to MongoDB
mongosh mongodb://localhost:27017

# Run the seed script
load("Database/seed-data.js")
```

### Using MongoDB Compass
1. Open MongoDB Compass
2. Connect to `mongodb://localhost:27017`
3. Create database `RealEstateDB`
4. Open MongoSH tab at the bottom
5. Copy and paste the content of `seed-data.js`
6. Press Enter to execute

## Option 2: Restore from Backup

### Using mongorestore command
```bash
# Restore the entire database
mongorestore --db=RealEstateDB ./Database/backup/RealEstateDB

# Or restore specific collection
mongorestore --db=RealEstateDB --collection=Properties ./Database/backup/RealEstateDB/Properties.bson
```

## Verify Data

After loading data, verify with:
```javascript
use RealEstateDB
db.Properties.countDocuments()  // Should return 10
db.Properties.findOne()  // View first document
```

## Create Backup

To create a new backup:
```bash
mongodump --db=RealEstateDB --out=./Database/backup
```

## Sample Data Summary

The seed data includes:
- **10 properties** with diverse characteristics
- Price range: $385,000 - $3,800,000
- Locations: Miami Beach, Coral Gables, Key Biscayne, Coconut Grove, etc.
- Each property has:
  - Owner information (name, address, photo, birthday)
  - 1-3 images (some enabled, some disabled)
  - 1-2 transaction traces (purchase, refinance, revaluation)

## Indexes Created

The following indexes are created for query optimization:
- Text index on `name` and `address` fields
- Single field index on `price`
- Single field index on `owner.idOwner`
- Compound index on `name`, `address`, and `price`

## Connection String

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

For MongoDB Atlas, use:
```json
{
  "MongoDbSettings": {
    "ConnectionString": "mongodb+srv://<username>:<password>@<cluster>.mongodb.net/?retryWrites=true&w=majority",
    "DatabaseName": "RealEstateDB",
    "CollectionName": "Properties"
  }
}
```
