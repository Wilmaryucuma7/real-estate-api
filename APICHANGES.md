# ?? API CHANGES - Slug Support & Separated Traces

## ? NEW FEATURES

### 1?? **PropertyDto now includes ID and Slug**

**Before:**
```json
{
  "idOwner": "OWN-001",
  "name": "Modern Beach House",
  "address": "1250 Ocean Drive...",
  "price": 1250000,
  "image": "https://..."
}
```

**After:**
```json
{
  "id": "673ab123cde456...",
  "slug": "modern-beach-house",
  "idOwner": "OWN-001",
  "name": "Modern Beach House",
  "address": "1250 Ocean Drive...",
  "price": 1250000,
  "image": "https://..."
}
```

### 2?? **Traces moved to separate endpoint**

**Before:** Traces included in PropertyDetailDto  
**After:** Fetch traces separately

```
GET /api/properties/modern-beach-house/traces
```

**Response:**
```json
[
  {
    "idPropertyTrace": "TRACE-001-01",
    "dateSale": "2022-06-15T00:00:00Z",
    "name": "Initial Purchase",
    "value": 1250000.00,
    "tax": 62500.00
  }
]
```

---

## ?? API ENDPOINTS SUMMARY

| Endpoint | Description | Returns |
|----------|-------------|---------|
| `GET /api/properties` | List all properties | PropertyDto[] with id & slug |
| `GET /api/properties/{id}` | Get by MongoDB ID | PropertyDetailDto (no traces) |
| `GET /api/properties/by-name/{slug}` | Get by slug name | PropertyDetailDto (no traces) |
| `GET /api/properties/{slug}/traces` | ? **NEW** - Get traces | PropertyTraceDto[] |
| `GET /api/owners` | List all owners | OwnerDto[] |
| `GET /api/owners/{id}` | Get owner | OwnerDto |
| `GET /api/owners/{id}/properties` | Owner's properties | PropertyDto[] with slug |

---

## ?? FRONTEND USAGE EXAMPLES

### React/Next.js Example

```typescript
// List properties with routing
const properties = await fetch('/api/properties?page=1&pageSize=10');
properties.data.forEach(prop => {
  // Use slug for SEO-friendly URLs
  const url = `/properties/${prop.slug}`; // /properties/modern-beach-house
  const detailUrl = `/api/properties/${prop.id}`; // By ID
  const slugUrl = `/api/properties/by-name/${prop.slug}`; // By slug
});

// Get property details
const property = await fetch('/api/properties/by-name/modern-beach-house');

// Get traces separately (lazy load)
const traces = await fetch('/api/properties/modern-beach-house/traces');
```

---

## ?? BREAKING CHANGES

### PropertyDto
- **Added:** `id` (string, required)
- **Added:** `slug` (string, required)

### PropertyDetailDto
- **Added:** `id` (string, required)
- **Added:** `slug` (string, required)
- **Removed:** `traces` - Use `/api/properties/{slug}/traces` instead

---

## ?? TESTS UPDATE REQUIRED

### Update PropertyDto Tests

**Before:**
```csharp
new PropertyDto 
{ 
    IdOwner = "OWN-1", 
    Name = "Property", 
    Address = "Address", 
    Price = 100000, 
    Image = "img.jpg" 
}
```

**After:**
```csharp
new PropertyDto 
{ 
    Id = "673ab123...",
    Slug = "property",
    IdOwner = "OWN-1", 
    Name = "Property", 
    Address = "Address", 
    Price = 100000, 
    Image = "img.jpg" 
}
```

### Update PropertyDetailDto Tests

**Before:**
```csharp
new PropertyDetailDto
{
    Id = "123",
    Name = "Property",
    // ...
    Traces = new List<PropertyTraceDto>() // ? REMOVED
}
```

**After:**
```csharp
new PropertyDetailDto
{
    Id = "673ab123...",
    Slug = "property", // ? ADDED
    Name = "Property",
    Owner = new OwnerDto { ... }, // ? REQUIRED
    Images = new List<PropertyImageDto>(), // ? REQUIRED
    // Traces removed - fetch separately
}
```

---

## ?? SLUG GENERATION

Slugs are automatically generated from property names:

| Property Name | Generated Slug |
|---------------|----------------|
| "Modern Beach House" | `modern-beach-house` |
| "Downtown Luxury Penthouse" | `downtown-luxury-penthouse` |
| "Casa en Bogotá" | `casa-en-bogota` (removes accents) |
| "Property #123 & More!" | `property-123-more` (removes special chars) |

---

## ?? NEW FILES

- `Application/Helpers/SlugHelper.cs` - Utility to generate URL-friendly slugs
- This README for documentation

---

## ? BENEFITS

1. **SEO-Friendly URLs**: `/properties/modern-beach-house` instead of `/properties/673ab...`
2. **Better Performance**: Traces loaded on-demand, not with every property request
3. **Frontend Flexibility**: Easy routing with predictable slug URLs
4. **Cleaner API**: Separation of concerns - details vs. history

---

## ?? NEXT STEPS

1. Run tests and update failing ones with new structure
2. Update frontend to use new `id` and `slug` properties
3. Implement lazy loading for traces in UI
4. Consider caching slug?id mapping for faster lookups

---

**For questions or issues, check the commit: `feat: add slug support and separate traces endpoint`**
