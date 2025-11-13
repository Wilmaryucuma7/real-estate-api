// MongoDB Seed Data Script for Real Estate API
// Run this script in MongoDB Compass MONGOSH tab (select RealEstateDB first)
// Or run in mongosh CLI

// Switch to RealEstateDB database (creates it if doesn't exist)
use RealEstateDB

// Drop existing collection if you want to start fresh
// db.Properties.drop()

// Insert sample properties with embedded owners, images, and traces
db.Properties.insertMany([
  {
    "name": "Modern Beach House",
    "address": "1250 Ocean Drive, Miami Beach, FL 33139",
    "price": NumberDecimal("1250000.00"),
    "codeInternal": "PROP-2024-001",
    "year": 2022,
    "owner": {
      "idOwner": "OWN-001",
      "name": "Maria Rodriguez",
      "address": "450 Brickell Ave, Miami, FL 33131",
      "photo": "https://i.pravatar.cc/150?img=47",
      "birthday": ISODate("1985-03-15T00:00:00Z")
    },
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
        "enabled": false
      }
    ],
    "traces": [
      {
        "idPropertyTrace": "TRACE-001-01",
        "dateSale": ISODate("2022-06-15T00:00:00Z"),
        "name": "Initial Purchase",
        "value": NumberDecimal("1250000.00"),
        "tax": NumberDecimal("62500.00")
      }
    ]
  },
  {
    "name": "Downtown Luxury Penthouse",
    "address": "88 Brickell Avenue, Miami, FL 33131",
    "price": NumberDecimal("2500000.00"),
    "codeInternal": "PROP-2024-002",
    "year": 2023,
    "owner": {
      "idOwner": "OWN-002",
      "name": "John Anderson",
      "address": "200 SE 15th Rd, Miami, FL 33129",
      "photo": "https://i.pravatar.cc/150?img=12",
      "birthday": ISODate("1978-11-22T00:00:00Z")
    },
    "images": [
      {
        "idPropertyImage": "IMG-002-01",
        "file": "https://images.unsplash.com/photo-1545324418-cc1a3fa10c00?w=800",
        "enabled": true
      },
      {
        "idPropertyImage": "IMG-002-02",
        "file": "https://images.unsplash.com/photo-1567496898669-ee935f5f647a?w=800",
        "enabled": true
      }
    ],
    "traces": [
      {
        "idPropertyTrace": "TRACE-002-01",
        "dateSale": ISODate("2023-01-10T00:00:00Z"),
        "name": "First Sale",
        "value": NumberDecimal("2500000.00"),
        "tax": NumberDecimal("125000.00")
      }
    ]
  },
  {
    "name": "Cozy Suburban Home",
    "address": "345 Elm Street, Coral Gables, FL 33134",
    "price": NumberDecimal("650000.00"),
    "codeInternal": "PROP-2024-003",
    "year": 2018,
    "owner": {
      "idOwner": "OWN-003",
      "name": "Sarah Johnson",
      "address": "789 Maple Ave, Coral Gables, FL 33146",
      "photo": "https://i.pravatar.cc/150?img=32",
      "birthday": ISODate("1990-07-08T00:00:00Z")
    },
    "images": [
      {
        "idPropertyImage": "IMG-003-01",
        "file": "https://images.unsplash.com/photo-1600596542815-ffad4c1539a9?w=800",
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
        "dateSale": ISODate("2023-09-15T00:00:00Z"),
        "name": "Refinance",
        "value": NumberDecimal("650000.00"),
        "tax": NumberDecimal("32500.00")
      }
    ]
  },
  {
    "name": "Waterfront Villa",
    "address": "567 Bay Shore Drive, Key Biscayne, FL 33149",
    "price": NumberDecimal("3800000.00"),
    "codeInternal": "PROP-2024-004",
    "year": 2021,
    "owner": {
      "idOwner": "OWN-004",
      "name": "Robert Chen",
      "address": "123 Harbor View, Key Biscayne, FL 33149",
      "photo": "https://i.pravatar.cc/150?img=68",
      "birthday": ISODate("1982-04-30T00:00:00Z")
    },
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
      }
    ]
  },
  {
    "name": "Historic Art Deco Building",
    "address": "1234 Collins Avenue, Miami Beach, FL 33139",
    "price": NumberDecimal("850000.00"),
    "codeInternal": "PROP-2024-005",
    "year": 1935,
    "owner": {
      "idOwner": "OWN-005",
      "name": "Isabella Martinez",
      "address": "901 Washington Ave, Miami Beach, FL 33139",
      "photo": "https://i.pravatar.cc/150?img=45",
      "birthday": ISODate("1988-12-10T00:00:00Z")
    },
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
        "dateSale": ISODate("2019-03-22T00:00:00Z"),
        "name": "Estate Sale",
        "value": NumberDecimal("850000.00"),
        "tax": NumberDecimal("42500.00")
      }
    ]
  },
  {
    "name": "Modern Condo with City View",
    "address": "777 Brickell Plaza, Miami, FL 33131",
    "price": NumberDecimal("475000.00"),
    "codeInternal": "PROP-2024-006",
    "year": 2020,
    "owner": {
      "idOwner": "OWN-006",
      "name": "Michael Thompson",
      "address": "333 SE 3rd Ave, Miami, FL 33131",
      "photo": "https://i.pravatar.cc/150?img=15",
      "birthday": ISODate("1992-06-18T00:00:00Z")
    },
    "images": [
      {
        "idPropertyImage": "IMG-006-01",
        "file": "https://images.unsplash.com/photo-1560448204-e02f11c3d0e2?w=800",
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
      }
    ]
  },
  {
    "name": "Tropical Paradise Estate",
    "address": "2100 Sunset Drive, Coconut Grove, FL 33133",
    "price": NumberDecimal("1950000.00"),
    "codeInternal": "PROP-2024-007",
    "year": 2019,
    "owner": {
      "idOwner": "OWN-007",
      "name": "Emily Davis",
      "address": "456 Palmetto Bay, Coconut Grove, FL 33133",
      "photo": "https://i.pravatar.cc/150?img=28",
      "birthday": ISODate("1987-09-25T00:00:00Z")
    },
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
      }
    ],
    "traces": [
      {
        "idPropertyTrace": "TRACE-007-01",
        "dateSale": ISODate("2019-12-12T00:00:00Z"),
        "name": "Private Sale",
        "value": NumberDecimal("1950000.00"),
        "tax": NumberDecimal("97500.00")
      }
    ]
  },
  {
    "name": "Eco-Friendly Smart Home",
    "address": "888 Green Boulevard, Pinecrest, FL 33156",
    "price": NumberDecimal("725000.00"),
    "codeInternal": "PROP-2024-008",
    "year": 2023,
    "owner": {
      "idOwner": "OWN-008",
      "name": "David Kim",
      "address": "222 Eco Lane, Pinecrest, FL 33156",
      "photo": "https://i.pravatar.cc/150?img=52",
      "birthday": ISODate("1991-02-14T00:00:00Z")
    },
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
    "name": "Golf Course Mansion",
    "address": "555 Fairway Drive, Doral, FL 33178",
    "price": NumberDecimal("1575000.00"),
    "codeInternal": "PROP-2024-009",
    "year": 2017,
    "owner": {
      "idOwner": "OWN-009",
      "name": "Jennifer Wilson",
      "address": "777 Club House Rd, Doral, FL 33178",
      "photo": "https://i.pravatar.cc/150?img=26",
      "birthday": ISODate("1984-08-03T00:00:00Z")
    },
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
        "dateSale": ISODate("2022-10-10T00:00:00Z"),
        "name": "Market Revaluation",
        "value": NumberDecimal("1575000.00"),
        "tax": NumberDecimal("78750.00")
      }
    ]
  },
  {
    "name": "Charming Family Home",
    "address": "432 Oak Street, Homestead, FL 33030",
    "price": NumberDecimal("385000.00"),
    "codeInternal": "PROP-2024-010",
    "year": 2015,
    "owner": {
      "idOwner": "OWN-010",
      "name": "Carlos Ramirez",
      "address": "155 Pine Ave, Homestead, FL 33030",
      "photo": "https://i.pravatar.cc/150?img=60",
      "birthday": ISODate("1989-05-28T00:00:00Z")
    },
    "images": [
      {
        "idPropertyImage": "IMG-010-01",
        "file": "https://images.unsplash.com/photo-1570129477492-45c003edd2be?w=800",
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
        "dateSale": ISODate("2023-11-30T00:00:00Z"),
        "name": "Home Improvement Appraisal",
        "value": NumberDecimal("385000.00"),
        "tax": NumberDecimal("19250.00")
      }
    ]
  }
]);

// Create indexes for optimized queries
db.Properties.createIndex({ "name": "text", "address": "text" });
db.Properties.createIndex({ "price": 1 });
db.Properties.createIndex({ "owner.idOwner": 1 });
db.Properties.createIndex({ "name": 1, "address": 1, "price": 1 });

print("? Successfully inserted 10 properties with sample data");
print("? Created indexes for optimized queries");
print("? Database 'RealEstateDB' is ready!");
