# ?? Docker Setup Guide

## Quick Start

### Prerequisites
- Docker Desktop ([Download](https://www.docker.com/products/docker-desktop/))
- Docker Compose (included with Docker Desktop)

### Run Everything with One Command

```bash
docker-compose up -d
```

**That's it!** The API and MongoDB will start with sample data automatically loaded. ??

---

## Access Points

| Service | URL | Description |
|---------|-----|-------------|
| **API** | http://localhost:5000 | REST API endpoints |
| **Swagger** | http://localhost:5000/swagger | Interactive API documentation |
| **MongoDB** | mongodb://localhost:27017 | Database connection |

### Quick Test

```bash
# List all properties
curl http://localhost:5000/api/properties

# Get specific property
curl http://localhost:5000/api/properties/modern-beach-house

# List owners
curl http://localhost:5000/api/owners
```

---

## Sample Data Included

MongoDB automatically loads on first start:
- **10 owners** (OWN-001 to OWN-010)
- **10 properties** with unique slugs
- **Optimized indexes** for performance
- **Price range**: $385K - $3.8M

**No manual seeding required!**

---

## Essential Commands

### Start/Stop

```bash
# Start containers (detached mode)
docker-compose up -d

# Stop containers
docker-compose down

# Stop and remove all data
docker-compose down -v
```

### Rebuild After Code Changes

```bash
docker-compose up -d --build api
```

### View Logs

```bash
# All services
docker-compose logs -f

# API only
docker-compose logs -f api

# MongoDB only
docker-compose logs -f mongodb
```

### Container Status

```bash
docker-compose ps
```

---

## Helper Scripts

For easier management, use the included helper scripts:

### Windows
```bash
docker-helper.bat
```

### Linux/Mac
```bash
chmod +x docker-helper.sh
./docker-helper.sh
```

**Menu options:**
1. Start containers
2. Stop containers
3. View API logs
4. View MongoDB logs
5. Restart all
6. Clean everything
7. Open Swagger
8. Check status

---

## Database Management

### Connect to MongoDB Shell

```bash
docker exec -it realestate-mongodb mongosh RealEstateDB
```

### Check Data

```bash
# Count properties
docker exec -it realestate-mongodb mongosh RealEstateDB --eval "db.Properties.countDocuments()"

# Count owners
docker exec -it realestate-mongodb mongosh RealEstateDB --eval "db.Owners.countDocuments()"
```

### Reset Database

```bash
# Remove volumes and restart (fresh data)
docker-compose down -v
docker-compose up -d
```

---

## Architecture

```
???????????????????????????????????????????
?          Docker Compose                  ?
???????????????????????????????????????????
?                                          ?
?  ????????????????    ????????????????  ?
?  ?              ?    ?              ?  ?
?  ?  Real Estate ??????   MongoDB    ?  ?
?  ?     API      ?    ?    7.0       ?  ?
?  ?  (.NET 9)    ?    ?              ?  ?
?  ?              ?    ? + Sample     ?  ?
?  ?  Port: 5000  ?    ?   Data       ?  ?
?  ?              ?    ?              ?  ?
?  ????????????????    ????????????????  ?
?         ?                    ?          ?
???????????????????????????????????????????
          ?                    ?
      Port 5000            Port 27017
          ?                    ?
          ?                    ?
      Swagger UI        MongoDB Compass
```

---

## Configuration Details

### Files Structure

```
RealEstateAPI/
??? Dockerfile                 # API container image
??? docker-compose.yml         # Services orchestration
??? .dockerignore             # Build optimization
??? docker-helper.bat         # Windows helper
??? docker-helper.sh          # Linux/Mac helper
??? Database/
    ??? init-mongo.js         # Auto-seed script
```

### Networks & Volumes

**Network:** `realestate-network` (bridge)
- API ? MongoDB: `mongodb://mongodb:27017`
- Host ? MongoDB: `mongodb://localhost:27017`

**Volume:** `mongodb_data`
- Persists database data between restarts
- Located in Docker volumes directory

### Environment Variables

The API container uses:
```yaml
ASPNETCORE_ENVIRONMENT: Development
ASPNETCORE_URLS: http://+:8080
MONGODB__CONNECTIONSTRING: mongodb://mongodb:27017
MONGODB__DATABASENAME: RealEstateDB
```

---

## Troubleshooting

### Port Already in Use

**Windows:**
```bash
netstat -ano | findstr :5000
```

**Linux/Mac:**
```bash
lsof -i :5000
```

**Solution:** Change port in `docker-compose.yml`
```yaml
ports:
  - "5050:8080"  # Use 5050 instead of 5000
```

### MongoDB Won't Start

```bash
# Check logs
docker-compose logs mongodb

# Check health
docker-compose ps

# Restart
docker-compose restart mongodb
```

### API Can't Connect to MongoDB

```bash
# Verify MongoDB is healthy
docker-compose ps

# Check if healthcheck passed
docker-compose logs mongodb | grep "healthy"

# Restart API
docker-compose restart api
```

### Database is Empty

```bash
# Check if init script ran
docker-compose logs mongodb | grep "initialization"

# Force re-initialization
docker-compose down -v
docker-compose up -d
```

### Rebuild Not Working

```bash
# Clean build
docker-compose down
docker-compose build --no-cache api
docker-compose up -d
```

---

## Testing in Docker

### Run Unit Tests

```bash
docker-compose exec api dotnet test
```

### Run with Verbose Output

```bash
docker-compose exec api dotnet test --logger "console;verbosity=detailed"
```

---

## Production Deployment

For production, update `docker-compose.yml`:

```yaml
services:
  mongodb:
    environment:
      MONGO_INITDB_ROOT_USERNAME: admin
      MONGO_INITDB_ROOT_PASSWORD: ${MONGO_PASSWORD}
    volumes:
      - /path/to/backup:/backup  # Backup location
    
  api:
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ASPNETCORE_URLS=https://+:8081;http://+:8080
      - MONGODB__CONNECTIONSTRING=mongodb://admin:${MONGO_PASSWORD}@mongodb:27017
    deploy:
      replicas: 2  # Scale API
      restart_policy:
        condition: on-failure
```

### Security Considerations

1. **Use secrets** for passwords
2. **Enable authentication** on MongoDB
3. **Use HTTPS** for API
4. **Limit exposed ports**
5. **Regular backups** of mongodb_data volume

---

## CI/CD Integration

### GitHub Actions

```yaml
name: Docker Build & Test

on:
  push:
    branches: [ main, develop ]

jobs:
  build-and-test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      
      - name: Start containers
        run: docker-compose up -d
        
      - name: Wait for API
        run: |
          timeout 60 bash -c 'until curl -f http://localhost:5000/health; do sleep 5; done'
      
      - name: Run tests
        run: docker-compose exec -T api dotnet test
        
      - name: Cleanup
        run: docker-compose down -v
```

### GitLab CI

```yaml
test:
  image: docker:latest
  services:
    - docker:dind
  script:
    - docker-compose up -d
    - docker-compose exec -T api dotnet test
    - docker-compose down -v
```

---

## Performance Tips

### Build Optimization

1. **Multi-stage builds** (already configured in Dockerfile)
2. **Layer caching** - Order commands from least to most changing
3. **Use .dockerignore** to exclude unnecessary files

### Runtime Optimization

```yaml
# Add resource limits in docker-compose.yml
services:
  api:
    deploy:
      resources:
        limits:
          cpus: '1'
          memory: 512M
        reservations:
          cpus: '0.5'
          memory: 256M
```

### MongoDB Optimization

```yaml
services:
  mongodb:
    command: mongod --wiredTigerCacheSizeGB 1.5
```

---

## Backup & Restore

### Backup Database

```bash
# Create backup
docker exec realestate-mongodb mongodump --db RealEstateDB --out /backup

# Copy to host
docker cp realestate-mongodb:/backup ./backup
```

### Restore Database

```bash
# Copy backup to container
docker cp ./backup realestate-mongodb:/backup

# Restore
docker exec realestate-mongodb mongorestore --db RealEstateDB /backup/RealEstateDB
```

---

## Sample Data Details

### Owners (10 records)
- OWN-001: Maria Rodriguez (Miami, FL)
- OWN-002: John Anderson (Miami, FL)
- OWN-003: Sarah Johnson (Coral Gables, FL)
- OWN-004: Robert Chen (Key Biscayne, FL)
- OWN-005: Isabella Martinez (Miami Beach, FL)
- OWN-006: Michael Thompson (Miami, FL)
- OWN-007: Emily Davis (Coconut Grove, FL)
- OWN-008: David Kim (Pinecrest, FL)
- OWN-009: Jennifer Wilson (Doral, FL)
- OWN-010: Carlos Ramirez (Homestead, FL)

### Properties (10 records)
1. Modern Beach House - $1,250,000
2. Downtown Luxury Penthouse - $2,500,000
3. Cozy Suburban Home - $650,000
4. Waterfront Villa - $3,800,000
5. Historic Art Deco Building - $850,000
6. Modern Condo with City View - $475,000
7. Tropical Paradise Estate - $1,950,000
8. Eco-Friendly Smart Home - $725,000
9. Golf Course Mansion - $1,575,000
10. Charming Family Home - $385,000

---

## Additional Resources

### Documentation
- **README.md** - General project setup
- **Database/README.md** - Database details
- **appsettings.json** - Configuration reference

### Tools
- **MongoDB Compass** - GUI for MongoDB ([Download](https://www.mongodb.com/products/compass))
- **Postman** - API testing ([Download](https://www.postman.com/downloads/))
- **Docker Desktop** - Container management

---

## Support

**Issues or questions:**
- Email: wilkrack7@gmail.com
- Check logs: `docker-compose logs -f`
- GitHub Issues (if available)

---

## Common Workflows

### Development Workflow

```bash
# 1. Start containers
docker-compose up -d

# 2. Make code changes
# ... edit files ...

# 3. Rebuild and restart API
docker-compose up -d --build api

# 4. View logs
docker-compose logs -f api

# 5. Test
curl http://localhost:5000/api/properties
```

### Testing Workflow

```bash
# 1. Start fresh
docker-compose down -v
docker-compose up -d

# 2. Wait for startup
sleep 10

# 3. Run tests
docker-compose exec api dotnet test

# 4. Test API endpoints
curl http://localhost:5000/api/properties
```

### Cleanup Workflow

```bash
# Stop everything
docker-compose down

# Remove volumes (data)
docker-compose down -v

# Remove images
docker rmi realestate-api

# Complete cleanup
docker system prune -a --volumes
```

---

**Built with ?? Docker for easy deployment and development**
