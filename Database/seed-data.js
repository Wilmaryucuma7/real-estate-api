// MongoDB Seed Data Script for Real Estate API
// Run this script in MongoDB Compass MONGOSH tab (select RealEstateDB first)
// Or run in mongosh CLI

// Switch to RealEstateDB database (creates it if doesn't exist)
use RealEstateDB

// Drop existing collections if you want to start fresh
db.Owners.drop()
db.Properties.drop()

// ========================================
// 1. INSERT OWNERS (Separate Collection)
// ========================================
db.Owners.insertMany([
  {
    "_id": "OWN-001",
    "name": "Maria Rodriguez",
    "address": "450 Brickell Ave, Miami, FL 33131",
    "photo": "https://i.pravatar.cc/150?img=47",
    "birthday": ISODate("1985-03-15T00:00:00Z")
  },
  {
    "_id": "OWN-002",
    "name": "John Anderson",
    "address": "200 SE 15th Rd, Miami, FL 33129",
    "photo": "https://i.pravatar.cc/150?img=12",
    "birthday": ISODate("1978-11-22T00:00:00Z")
  },
  {
    "_id": "OWN-003",
    "name": "Sarah Johnson",
    "address": "789 Maple Ave, Coral Gables, FL 33146",
    "photo": "https://i.pravatar.cc/150?img=32",
    "birthday": ISODate("1990-07-08T00:00:00Z")
  },
  {
    "_id": "OWN-004",
    "name": "Robert Chen",
    "address": "123 Harbor View, Key Biscayne, FL 33149",
    "photo": "https://i.pravatar.cc/150?img=68",
    "birthday": ISODate("1982-04-30T00:00:00Z")
  },
  {
    "_id": "OWN-005",
    "name": "Isabella Martinez",
    "address": "901 Washington Ave, Miami Beach, FL 33139",
    "photo": "https://i.pravatar.cc/150?img=45",
    "birthday": ISODate("1988-12-10T00:00:00Z")
  },
  {
    "_id": "OWN-006",
    "name": "Michael Thompson",
    "address": "333 SE 3rd Ave, Miami, FL 33131",
    "photo": "https://i.pravatar.cc/150?img=15",
    "birthday": ISODate("1992-06-18T00:00:00Z")
  },
  {
    "_id": "OWN-007",
    "name": "Emily Davis",
    "address": "456 Palmetto Bay, Coconut Grove, FL 33133",
    "photo": "https://i.pravatar.cc/150?img=28",
    "birthday": ISODate("1987-09-25T00:00:00Z")
  },
  {
    "_id": "OWN-008",
    "name": "David Kim",
    "address": "222 Eco Lane, Pinecrest, FL 33156",
    "photo": "https://i.pravatar.cc/150?img=52",
    "birthday": ISODate("1991-02-14T00:00:00Z")
  },
  {
    "_id": "OWN-009",
    "name": "Jennifer Wilson",
    "address": "777 Club House Rd, Doral, FL 33178",
    "photo": "https://i.pravatar.cc/150?img=26",
    "birthday": ISODate("1984-08-03T00:00:00Z")
  },
  {
    "_id": "OWN-010",
    "name": "Carlos Ramirez",
    "address": "155 Pine Ave, Homestead, FL 33030",
    "photo": "https://i.pravatar.cc/150?img=60",
    "birthday": ISODate("1989-05-28T00:00:00Z")
  }
]);

print("? Successfully inserted 10 owners");

// ========================================
// 2. INSERT PROPERTIES (20 properties with unique names and slugs)
// Each property has 2-3 images and 2-4 traces (transaction history)
// ========================================
db.Properties.insertMany([
  // OWN-001: Maria Rodriguez (3 properties)
  {
    "name": "Modern Beach House",
    "slug": "modern-beach-house",
    "address": "1250 Ocean Drive, Miami Beach, FL 33139",
    "price": NumberDecimal("1250000.00"),
    "codeInternal": "PROP-2024-001",
    "year": 2022,
    "ownerId": "OWN-001",
    "images": [
      {
        "idPropertyImage": "IMG-001-01",
        "file": "https://images.unsplash.com/photo-1613490493576-7fde63acd811?w=800",
        "enabled": true
      },
      {
        "idPropertyImage": "IMG-001-02",
        "file": "https://images.unsplash.com/photo-1512917774080-9991f1c4c750?w=800",
        "enabled": true
      },
      {
        "idPropertyImage": "IMG-001-03",
        "file": "https://images.unsplash.com/photo-1613977257363-707ba9348227?w=800",
        "enabled": true
      }
    ],
    "traces": [
      {
        "idPropertyTrace": "TRACE-001-01",
        "dateSale": ISODate("2019-03-10T00:00:00Z"),
        "name": "First Purchase",
        "value": NumberDecimal("950000.00"),
        "tax": NumberDecimal("47500.00")
      },
      {
        "idPropertyTrace": "TRACE-001-02",
        "dateSale": ISODate("2021-08-15T00:00:00Z"),
        "name": "Renovation Appraisal",
        "value": NumberDecimal("1100000.00"),
        "tax": NumberDecimal("55000.00")
      },
      {
        "idPropertyTrace": "TRACE-001-03",
        "dateSale": ISODate("2022-06-15T00:00:00Z"),
        "name": "Market Revaluation",
        "value": NumberDecimal("1250000.00"),
        "tax": NumberDecimal("62500.00")
      }
    ]
  },
  {
    "name": "Sunset Bay Cottage",
    "slug": "sunset-bay-cottage",
    "address": "3450 Collins Ave, Miami Beach, FL 33140",
    "price": NumberDecimal("890000.00"),
    "codeInternal": "PROP-2024-011",
    "year": 2021,
    "ownerId": "OWN-001",
    "images": [
      {
        "idPropertyImage": "IMG-011-01",
        "file": "https://images.unsplash.com/photo-1564013799919-ab600027ffc6?w=800",
        "enabled": true
      },
      {
        "idPropertyImage": "IMG-011-02",
        "file": "https://images.unsplash.com/photo-1600047509807-ba8f99d2cdde?w=800",
        "enabled": true
      }
    ],
    "traces": [
      {
        "idPropertyTrace": "TRACE-011-01",
        "dateSale": ISODate("2021-08-20T00:00:00Z"),
        "name": "Investment Property",
        "value": NumberDecimal("890000.00"),
        "tax": NumberDecimal("44500.00")
      },
      {
        "idPropertyTrace": "TRACE-011-02",
        "dateSale": ISODate("2023-02-10T00:00:00Z"),
        "name": "Insurance Valuation",
        "value": NumberDecimal("920000.00"),
        "tax": NumberDecimal("46000.00")
      }
    ]
  },
  {
    "name": "Oceanview Penthouse Suite",
    "slug": "oceanview-penthouse-suite",
    "address": "1500 Bay Rd, Miami Beach, FL 33139",
    "price": NumberDecimal("2150000.00"),
    "codeInternal": "PROP-2024-012",
    "year": 2023,
    "ownerId": "OWN-001",
    "images": [
      {
        "idPropertyImage": "IMG-012-01",
        "file": "https://images.unsplash.com/photo-1502672260066-6bc35f0fb0b4?w=800",
        "enabled": true
      },
      {
        "idPropertyImage": "IMG-012-02",
        "file": "https://images.unsplash.com/photo-1600585154526-990dced4db0d?w=800",
        "enabled": true
      },
      {
        "idPropertyImage": "IMG-012-03",
        "file": "https://images.unsplash.com/photo-1600607687644-c7171b42498f?w=800",
        "enabled": false
      }
    ],
    "traces": [
      {
        "idPropertyTrace": "TRACE-012-01",
        "dateSale": ISODate("2023-03-10T00:00:00Z"),
        "name": "New Acquisition",
        "value": NumberDecimal("2150000.00"),
        "tax": NumberDecimal("107500.00")
      }
    ]
  },

  // OWN-002: John Anderson (2 properties)
  {
    "name": "Downtown Luxury Penthouse",
    "slug": "downtown-luxury-penthouse",
    "address": "88 Brickell Avenue, Miami, FL 33131",
    "price": NumberDecimal("2500000.00"),
    "codeInternal": "PROP-2024-002",
    "year": 2023,
    "ownerId": "OWN-002",
    "images": [
      {
        "idPropertyImage": "IMG-002-01",
        "file": "https://images.unsplash.com/photo-1545324418-cc1a3fa10c00?w=800",
        "enabled": true
      },
      {
        "idPropertyImage": "IMG-002-02",
        "file": "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?w=800",
        "enabled": true
      }
    ],
    "traces": [
      {
        "idPropertyTrace": "TRACE-002-01",
        "dateSale": ISODate("2020-11-05T00:00:00Z"),
        "name": "Pre-construction Purchase",
        "value": NumberDecimal("2000000.00"),
        "tax": NumberDecimal("100000.00")
      },
      {
        "idPropertyTrace": "TRACE-002-02",
        "dateSale": ISODate("2023-01-10T00:00:00Z"),
        "name": "Completion Valuation",
        "value": NumberDecimal("2500000.00"),
        "tax": NumberDecimal("125000.00")
      }
    ]
  },
  {
    "name": "Brickell Heights Tower",
    "slug": "brickell-heights-tower",
    "address": "100 Brickell City Centre, Miami, FL 33131",
    "price": NumberDecimal("1800000.00"),
    "codeInternal": "PROP-2024-013",
    "year": 2022,
    "ownerId": "OWN-002",
    "images": [
      {
        "idPropertyImage": "IMG-013-01",
        "file": "https://images.unsplash.com/photo-1512917774080-9991f1c4c750?w=800",
        "enabled": true
      },
      {
        "idPropertyImage": "IMG-013-02",
        "file": "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?w=800",
        "enabled": true
      },
      {
        "idPropertyImage": "IMG-013-03",
        "file": "https://images.unsplash.com/photo-1600566753190-17f0baa2a6c3?w=800",
        "enabled": true
      }
    ],
    "traces": [
      {
        "idPropertyTrace": "TRACE-013-01",
        "dateSale": ISODate("2022-11-15T00:00:00Z"),
        "name": "Corporate Investment",
        "value": NumberDecimal("1800000.00"),
        "tax": NumberDecimal("90000.00")
      },
      {
        "idPropertyTrace": "TRACE-013-02",
        "dateSale": ISODate("2024-01-20T00:00:00Z"),
        "name": "Annual Assessment",
        "value": NumberDecimal("1850000.00"),
        "tax": NumberDecimal("92500.00")
      }
    ]
  },

  // OWN-003: Sarah Johnson (2 properties)
  {
    "name": "Cozy Suburban Home",
    "slug": "cozy-suburban-home",
    "address": "345 Elm Street, Coral Gables, FL 33134",
    "price": NumberDecimal("650000.00"),
    "codeInternal": "PROP-2024-003",
    "year": 2018,
    "ownerId": "OWN-003",
    "images": [
      {
        "idPropertyImage": "IMG-003-01",
        "file": "https://images.unsplash.com/photo-1600596542815-ffad4c1539a9?w=800",
        "enabled": true
      },
      {
        "idPropertyImage": "IMG-003-02",
        "file": "https://images.unsplash.com/photo-1600566753086-00f18fb6b3ea?w=800",
        "enabled": true
      }
    ],
    "traces": [
      {
        "idPropertyTrace": "TRACE-003-01",
        "dateSale": ISODate("2018-05-20T00:00:00Z"),
        "name": "Original Purchase",
        "value": NumberDecimal("550000.00"),
        "tax": NumberDecimal("27500.00")
      },
      {
        "idPropertyTrace": "TRACE-003-02",
        "dateSale": ISODate("2020-09-10T00:00:00Z"),
        "name": "Home Improvement",
        "value": NumberDecimal("600000.00"),
        "tax": NumberDecimal("30000.00")
      },
      {
        "idPropertyTrace": "TRACE-003-03",
        "dateSale": ISODate("2023-09-15T00:00:00Z"),
        "name": "Market Adjustment",
        "value": NumberDecimal("650000.00"),
        "tax": NumberDecimal("32500.00")
      }
    ]
  },
  {
    "name": "Garden Oasis Villa",
    "slug": "garden-oasis-villa",
    "address": "456 Miracle Mile, Coral Gables, FL 33134",
    "price": NumberDecimal("975000.00"),
    "codeInternal": "PROP-2024-014",
    "year": 2020,
    "ownerId": "OWN-003",
    "images": [
      {
        "idPropertyImage": "IMG-014-01",
        "file": "https://images.unsplash.com/photo-1605276374104-dee2a0ed3cd6?w=800",
        "enabled": true
      },
      {
        "idPropertyImage": "IMG-014-02",
        "file": "https://images.unsplash.com/photo-1600047509807-ba8f99d2cdde?w=800",
        "enabled": true
      },
      {
        "idPropertyImage": "IMG-014-03",
        "file": "https://images.unsplash.com/photo-1600585154526-990dced4db0d?w=800",
        "enabled": true
      }
    ],
    "traces": [
      {
        "idPropertyTrace": "TRACE-014-01",
        "dateSale": ISODate("2020-07-12T00:00:00Z"),
        "name": "Estate Purchase",
        "value": NumberDecimal("975000.00"),
        "tax": NumberDecimal("48750.00")
      },
      {
        "idPropertyTrace": "TRACE-014-02",
        "dateSale": ISODate("2022-12-05T00:00:00Z"),
        "name": "Landscaping Upgrade",
        "value": NumberDecimal("1050000.00"),
        "tax": NumberDecimal("52500.00")
      }
    ]
  },

  // OWN-004: Robert Chen (3 properties - high value portfolio)
  {
    "name": "Waterfront Villa",
    "slug": "waterfront-villa",
    "address": "567 Bay Shore Drive, Key Biscayne, FL 33149",
    "price": NumberDecimal("3800000.00"),
    "codeInternal": "PROP-2024-004",
    "year": 2021,
    "ownerId": "OWN-004",
    "images": [
      {
        "idPropertyImage": "IMG-004-01",
        "file": "https://images.unsplash.com/photo-1605276374104-dee2a0ed3cd6?w=800",
        "enabled": true
      },
      {
        "idPropertyImage": "IMG-004-02",
        "file": "https://images.unsplash.com/photo-1600607687939-ce8a6c25118c?w=800",
        "enabled": true
      },
      {
        "idPropertyImage": "IMG-004-03",
        "file": "https://images.unsplash.com/photo-1600607687644-c7171b42498f?w=800",
        "enabled": true
      }
    ],
    "traces": [
      {
        "idPropertyTrace": "TRACE-004-01",
        "dateSale": ISODate("2021-11-05T00:00:00Z"),
        "name": "Acquisition",
        "value": NumberDecimal("3800000.00"),
        "tax": NumberDecimal("190000.00")
      },
      {
        "idPropertyTrace": "TRACE-004-02",
        "dateSale": ISODate("2023-05-15T00:00:00Z"),
        "name": "Marina Upgrade",
        "value": NumberDecimal("4100000.00"),
        "tax": NumberDecimal("205000.00")
      }
    ]
  },
  {
    "name": "Seaside Luxury Estate",
    "slug": "seaside-luxury-estate",
    "address": "789 Ocean Lane, Key Biscayne, FL 33149",
    "price": NumberDecimal("4200000.00"),
    "codeInternal": "PROP-2024-015",
    "year": 2022,
    "ownerId": "OWN-004",
    "images": [
      {
        "idPropertyImage": "IMG-015-01",
        "file": "https://images.unsplash.com/photo-1613977257363-707ba9348227?w=800",
        "enabled": true
      },
      {
        "idPropertyImage": "IMG-015-02",
        "file": "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?w=800",
        "enabled": true
      }
    ],
    "traces": [
      {
        "idPropertyTrace": "TRACE-015-01",
        "dateSale": ISODate("2022-04-18T00:00:00Z"),
        "name": "Luxury Investment",
        "value": NumberDecimal("4200000.00"),
        "tax": NumberDecimal("210000.00")
      }
    ]
  },
  {
    "name": "Harbor View Mansion",
    "slug": "harbor-view-mansion",
    "address": "234 Harbor Dr, Key Biscayne, FL 33149",
    "price": NumberDecimal("5500000.00"),
    "codeInternal": "PROP-2024-016",
    "year": 2023,
    "ownerId": "OWN-004",
    "images": [
      {
        "idPropertyImage": "IMG-016-01",
        "file": "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?w=800",
        "enabled": true
      },
      {
        "idPropertyImage": "IMG-016-02",
        "file": "https://images.unsplash.com/photo-1613490493576-7fde63acd811?w=800",
        "enabled": true
      },
      {
        "idPropertyImage": "IMG-016-03",
        "file": "https://images.unsplash.com/photo-1600566753190-17f0baa2a6c3?w=800",
        "enabled": true
      }
    ],
    "traces": [
      {
        "idPropertyTrace": "TRACE-016-01",
        "dateSale": ISODate("2023-09-22T00:00:00Z"),
        "name": "Premium Acquisition",
        "value": NumberDecimal("5500000.00"),
        "tax": NumberDecimal("275000.00")
      },
      {
        "idPropertyTrace": "TRACE-016-02",
        "dateSale": ISODate("2024-03-10T00:00:00Z"),
        "name": "Smart Home Installation",
        "value": NumberDecimal("5650000.00"),
        "tax": NumberDecimal("282500.00")
      }
    ]
  },

  // OWN-005: Isabella Martinez (2 properties - historic)
  {
    "name": "Historic Art Deco Building",
    "slug": "historic-art-deco-building",
    "address": "1234 Collins Avenue, Miami Beach, FL 33139",
    "price": NumberDecimal("850000.00"),
    "codeInternal": "PROP-2024-005",
    "year": 1935,
    "ownerId": "OWN-005",
    "images": [
      {
        "idPropertyImage": "IMG-005-01",
        "file": "https://images.unsplash.com/photo-1580587771525-78b9dba3b914?w=800",
        "enabled": true
      },
      {
        "idPropertyImage": "IMG-005-02",
        "file": "https://images.unsplash.com/photo-1582268611958-ebfd161ef9cf?w=800",
        "enabled": true
      }
    ],
    "traces": [
      {
        "idPropertyTrace": "TRACE-005-01",
        "dateSale": ISODate("2015-06-10T00:00:00Z"),
        "name": "Historic Purchase",
        "value": NumberDecimal("650000.00"),
        "tax": NumberDecimal("32500.00")
      },
      {
        "idPropertyTrace": "TRACE-005-02",
        "dateSale": ISODate("2017-11-20T00:00:00Z"),
        "name": "Restoration Phase 1",
        "value": NumberDecimal("750000.00"),
        "tax": NumberDecimal("37500.00")
      },
      {
        "idPropertyTrace": "TRACE-005-03",
        "dateSale": ISODate("2019-03-22T00:00:00Z"),
        "name": "Final Restoration",
        "value": NumberDecimal("850000.00"),
        "tax": NumberDecimal("42500.00")
      }
    ]
  },
  {
    "name": "Vintage Mediterranean Villa",
    "slug": "vintage-mediterranean-villa",
    "address": "678 Espanola Way, Miami Beach, FL 33139",
    "price": NumberDecimal("1150000.00"),
    "codeInternal": "PROP-2024-017",
    "year": 1940,
    "ownerId": "OWN-005",
    "images": [
      {
        "idPropertyImage": "IMG-017-01",
        "file": "https://images.unsplash.com/photo-1582268611958-ebfd161ef9cf?w=800",
        "enabled": true
      },
      {
        "idPropertyImage": "IMG-017-02",
        "file": "https://images.unsplash.com/photo-1580587771525-78b9dba3b914?w=800",
        "enabled": true
      },
      {
        "idPropertyImage": "IMG-017-03",
        "file": "https://images.unsplash.com/photo-1600047509807-ba8f99d2cdde?w=800",
        "enabled": false
      }
    ],
    "traces": [
      {
        "idPropertyTrace": "TRACE-017-01",
        "dateSale": ISODate("2020-06-10T00:00:00Z"),
        "name": "Historic Restoration",
        "value": NumberDecimal("1150000.00"),
        "tax": NumberDecimal("57500.00")
      },
      {
        "idPropertyTrace": "TRACE-017-02",
        "dateSale": ISODate("2022-08-18T00:00:00Z"),
        "name": "Heritage Certification",
        "value": NumberDecimal("1200000.00"),
        "tax": NumberDecimal("60000.00")
      }
    ]
  },

  // OWN-006: Michael Thompson (1 property)
  {
    "name": "Modern Condo with City View",
    "slug": "modern-condo-with-city-view",
    "address": "777 Brickell Plaza, Miami, FL 33131",
    "price": NumberDecimal("475000.00"),
    "codeInternal": "PROP-2024-006",
    "year": 2020,
    "ownerId": "OWN-006",
    "images": [
      {
        "idPropertyImage": "IMG-006-01",
        "file": "https://images.unsplash.com/photo-1560448204-e02f11c3d0e2?w=800",
        "enabled": true
      },
      {
        "idPropertyImage": "IMG-006-02",
        "file": "https://images.unsplash.com/photo-1545324418-cc1a3fa10c00?w=800",
        "enabled": true
      }
    ],
    "traces": [
      {
        "idPropertyTrace": "TRACE-006-01",
        "dateSale": ISODate("2020-08-30T00:00:00Z"),
        "name": "New Construction Sale",
        "value": NumberDecimal("475000.00"),
        "tax": NumberDecimal("23750.00")
      },
      {
        "idPropertyTrace": "TRACE-006-02",
        "dateSale": ISODate("2023-04-15T00:00:00Z"),
        "name": "Market Update",
        "value": NumberDecimal("495000.00"),
        "tax": NumberDecimal("24750.00")
      }
    ]
  },

  // OWN-007: Emily Davis (2 properties)
  {
    "name": "Tropical Paradise Estate",
    "slug": "tropical-paradise-estate",
    "address": "2100 Sunset Drive, Coconut Grove, FL 33133",
    "price": NumberDecimal("1950000.00"),
    "codeInternal": "PROP-2024-007",
    "year": 2019,
    "ownerId": "OWN-007",
    "images": [
      {
        "idPropertyImage": "IMG-007-01",
        "file": "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?w=800",
        "enabled": true
      },
      {
        "idPropertyImage": "IMG-007-02",
        "file": "https://images.unsplash.com/photo-1600566753190-17f0baa2a6c3?w=800",
        "enabled": true
      },
      {
        "idPropertyImage": "IMG-007-03",
        "file": "https://images.unsplash.com/photo-1613490493576-7fde63acd811?w=800",
        "enabled": true
      }
    ],
    "traces": [
      {
        "idPropertyTrace": "TRACE-007-01",
        "dateSale": ISODate("2019-12-12T00:00:00Z"),
        "name": "Private Sale",
        "value": NumberDecimal("1950000.00"),
        "tax": NumberDecimal("97500.00")
      },
      {
        "idPropertyTrace": "TRACE-007-02",
        "dateSale": ISODate("2021-07-20T00:00:00Z"),
        "name": "Pool Addition",
        "value": NumberDecimal("2100000.00"),
        "tax": NumberDecimal("105000.00")
      },
      {
        "idPropertyTrace": "TRACE-007-03",
        "dateSale": ISODate("2023-11-10T00:00:00Z"),
        "name": "Tropical Landscaping",
        "value": NumberDecimal("2200000.00"),
        "tax": NumberDecimal("110000.00")
      }
    ]
  },
  {
    "name": "Bayfront Contemporary Home",
    "slug": "bayfront-contemporary-home",
    "address": "3500 Pan American Dr, Coconut Grove, FL 33133",
    "price": NumberDecimal("2400000.00"),
    "codeInternal": "PROP-2024-018",
    "year": 2021,
    "ownerId": "OWN-007",
    "images": [
      {
        "idPropertyImage": "IMG-018-01",
        "file": "https://images.unsplash.com/photo-1600566753190-17f0baa2a6c3?w=800",
        "enabled": true
      },
      {
        "idPropertyImage": "IMG-018-02",
        "file": "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?w=800",
        "enabled": true
      }
    ],
    "traces": [
      {
        "idPropertyTrace": "TRACE-018-01",
        "dateSale": ISODate("2021-10-05T00:00:00Z"),
        "name": "Waterfront Purchase",
        "value": NumberDecimal("2400000.00"),
        "tax": NumberDecimal("120000.00")
      },
      {
        "idPropertyTrace": "TRACE-018-02",
        "dateSale": ISODate("2023-06-18T00:00:00Z"),
        "name": "Dock Construction",
        "value": NumberDecimal("2550000.00"),
        "tax": NumberDecimal("127500.00")
      }
    ]
  },

  // OWN-008: David Kim (2 properties - eco-friendly)
  {
    "name": "Eco-Friendly Smart Home",
    "slug": "eco-friendly-smart-home",
    "address": "888 Green Boulevard, Pinecrest, FL 33156",
    "price": NumberDecimal("725000.00"),
    "codeInternal": "PROP-2024-008",
    "year": 2023,
    "ownerId": "OWN-008",
    "images": [
      {
        "idPropertyImage": "IMG-008-01",
        "file": "https://images.unsplash.com/photo-1599427303058-f04cbcf4756f?w=800",
        "enabled": true
      },
      {
        "idPropertyImage": "IMG-008-02",
        "file": "https://images.unsplash.com/photo-1600607687920-4e2a09cf159d?w=800",
        "enabled": true
      }
    ],
    "traces": [
      {
        "idPropertyTrace": "TRACE-008-01",
        "dateSale": ISODate("2023-04-05T00:00:00Z"),
        "name": "First Owner",
        "value": NumberDecimal("725000.00"),
        "tax": NumberDecimal("36250.00")
      }
    ]
  },
  {
    "name": "Solar Powered Villa",
    "slug": "solar-powered-villa",
    "address": "999 Eco Drive, Pinecrest, FL 33156",
    "price": NumberDecimal("850000.00"),
    "codeInternal": "PROP-2024-019",
    "year": 2023,
    "ownerId": "OWN-008",
    "images": [
      {
        "idPropertyImage": "IMG-019-01",
        "file": "https://images.unsplash.com/photo-1600607687920-4e2a09cf159d?w=800",
        "enabled": true
      },
      {
        "idPropertyImage": "IMG-019-02",
        "file": "https://images.unsplash.com/photo-1599427303058-f04cbcf4756f?w=800",
        "enabled": true
      },
      {
        "idPropertyImage": "IMG-019-03",
        "file": "https://images.unsplash.com/photo-1600047509807-ba8f99d2cdde?w=800",
        "enabled": true
      }
    ],
    "traces": [
      {
        "idPropertyTrace": "TRACE-019-01",
        "dateSale": ISODate("2023-07-20T00:00:00Z"),
        "name": "Green Investment",
        "value": NumberDecimal("850000.00"),
        "tax": NumberDecimal("42500.00")
      },
      {
        "idPropertyTrace": "TRACE-019-02",
        "dateSale": ISODate("2024-02-15T00:00:00Z"),
        "name": "Solar Panel Upgrade",
        "value": NumberDecimal("880000.00"),
        "tax": NumberDecimal("44000.00")
      }
    ]
  },

  // OWN-009: Jennifer Wilson (2 properties - golf community)
  {
    "name": "Golf Course Mansion",
    "slug": "golf-course-mansion",
    "address": "555 Fairway Drive, Doral, FL 33178",
    "price": NumberDecimal("1575000.00"),
    "codeInternal": "PROP-2024-009",
    "year": 2017,
    "ownerId": "OWN-009",
    "images": [
      {
        "idPropertyImage": "IMG-009-01",
        "file": "https://images.unsplash.com/photo-1600585154526-990dced4db0d?w=800",
        "enabled": true
      },
      {
        "idPropertyImage": "IMG-009-02",
        "file": "https://images.unsplash.com/photo-1600566753086-00f18fb6b3ea?w=800",
        "enabled": true
      }
    ],
    "traces": [
      {
        "idPropertyTrace": "TRACE-009-01",
        "dateSale": ISODate("2017-07-20T00:00:00Z"),
        "name": "Purchase",
        "value": NumberDecimal("1400000.00"),
        "tax": NumberDecimal("70000.00")
      },
      {
        "idPropertyTrace": "TRACE-009-02",
        "dateSale": ISODate("2020-03-12T00:00:00Z"),
        "name": "Basement Expansion",
        "value": NumberDecimal("1500000.00"),
        "tax": NumberDecimal("75000.00")
      },
      {
        "idPropertyTrace": "TRACE-009-03",
        "dateSale": ISODate("2022-10-10T00:00:00Z"),
        "name": "Market Revaluation",
        "value": NumberDecimal("1575000.00"),
        "tax": NumberDecimal("78750.00")
      }
    ]
  },
  {
    "name": "Fairway Heights Estate",
    "slug": "fairway-heights-estate",
    "address": "777 Championship Dr, Doral, FL 33178",
    "price": NumberDecimal("1890000.00"),
    "codeInternal": "PROP-2024-020",
    "year": 2019,
    "ownerId": "OWN-009",
    "images": [
      {
        "idPropertyImage": "IMG-020-01",
        "file": "https://images.unsplash.com/photo-1600566753086-00f18fb6b3ea?w=800",
        "enabled": true
      },
      {
        "idPropertyImage": "IMG-020-02",
        "file": "https://images.unsplash.com/photo-1600585154526-990dced4db0d?w=800",
        "enabled": true
      },
      {
        "idPropertyImage": "IMG-020-03",
        "file": "https://images.unsplash.com/photo-1600047509807-ba8f99d2cdde?w=800",
        "enabled": true
      }
    ],
    "traces": [
      {
        "idPropertyTrace": "TRACE-020-01",
        "dateSale": ISODate("2019-09-15T00:00:00Z"),
        "name": "Golf Community",
        "value": NumberDecimal("1890000.00"),
        "tax": NumberDecimal("94500.00")
      },
      {
        "idPropertyTrace": "TRACE-020-02",
        "dateSale": ISODate("2021-12-20T00:00:00Z"),
        "name": "Club House Access",
        "value": NumberDecimal("1950000.00"),
        "tax": NumberDecimal("97500.00")
      },
      {
        "idPropertyTrace": "TRACE-020-03",
        "dateSale": ISODate("2023-08-10T00:00:00Z"),
        "name": "Tennis Court Addition",
        "value": NumberDecimal("2050000.00"),
        "tax": NumberDecimal("102500.00")
      }
    ]
  },

  // OWN-010: Carlos Ramirez (1 property)
  {
    "name": "Charming Family Home",
    "slug": "charming-family-home",
    "address": "432 Oak Street, Homestead, FL 33030",
    "price": NumberDecimal("385000.00"),
    "codeInternal": "PROP-2024-010",
    "year": 2015,
    "ownerId": "OWN-010",
    "images": [
      {
        "idPropertyImage": "IMG-010-01",
        "file": "https://images.unsplash.com/photo-1570129477492-45c003edd2be?w=800",
        "enabled": true
      },
      {
        "idPropertyImage": "IMG-010-02",
        "file": "https://images.unsplash.com/photo-1600596542815-ffad4c1539a9?w=800",
        "enabled": true
      }
    ],
    "traces": [
      {
        "idPropertyTrace": "TRACE-010-01",
        "dateSale": ISODate("2015-02-18T00:00:00Z"),
        "name": "Initial Purchase",
        "value": NumberDecimal("320000.00"),
        "tax": NumberDecimal("16000.00")
      },
      {
        "idPropertyTrace": "TRACE-010-02",
        "dateSale": ISODate("2019-06-10T00:00:00Z"),
        "name": "Kitchen Remodel",
        "value": NumberDecimal("350000.00"),
        "tax": NumberDecimal("17500.00")
      },
      {
        "idPropertyTrace": "TRACE-010-03",
        "dateSale": ISODate("2023-11-30T00:00:00Z"),
        "name": "Home Improvement Appraisal",
        "value": NumberDecimal("385000.00"),
        "tax": NumberDecimal("19250.00")
      },
      {
        "idPropertyTrace": "TRACE-010-04",
        "dateSale": ISODate("2024-05-15T00:00:00Z"),
        "name": "Roof Replacement",
        "value": NumberDecimal("395000.00"),
        "tax": NumberDecimal("19750.00")
      }
    ]
  }
]);

print("? Successfully inserted properties with slug field");

// ========================================
// 3. CREATE INDEXES
// ========================================
// Properties collection indexes
db.Properties.createIndex({ "slug": 1 }, { unique: true }); // UNIQUE INDEX ON SLUG (NEW!)
db.Properties.createIndex({ "name": 1 }, { unique: true }); // UNIQUE INDEX ON NAME
db.Properties.createIndex({ "name": "text", "address": "text" });
db.Properties.createIndex({ "price": 1 });
db.Properties.createIndex({ "ownerId": 1 }); // Index on foreign key
db.Properties.createIndex({ "name": 1, "address": 1, "price": 1 });

// Owners collection indexes
db.Owners.createIndex({ "name": 1 });

print("? Created indexes including slug index for optimized queries");
print("? Database 'RealEstateDB' with separate collections is ready!");
print("");
print("?? Summary:");
print("  - Owners: " + db.Owners.countDocuments() + " documents");
print("  - Properties: " + db.Properties.countDocuments() + " documents");
print("");
print("???  Property Distribution:");
print("  - OWN-001 (Maria Rodriguez): 3 properties");
print("  - OWN-002 (John Anderson): 2 properties");
print("  - OWN-003 (Sarah Johnson): 2 properties");
print("  - OWN-004 (Robert Chen): 3 properties (high value portfolio)");
print("  - OWN-005 (Isabella Martinez): 2 properties (historic)");
print("  - OWN-006 (Michael Thompson): 1 property");
print("  - OWN-007 (Emily Davis): 2 properties");
print("  - OWN-008 (David Kim): 2 properties (eco-friendly)");
print("  - OWN-009 (Jennifer Wilson): 2 properties (golf community)");
print("  - OWN-010 (Carlos Ramirez): 1 property");
print("");
print("?? Images per property: 2-3 images (some disabled for testing)");
print("?? Traces per property: 2-4 transaction records (realistic history)");
