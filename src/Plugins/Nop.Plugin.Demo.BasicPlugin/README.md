# Nop.Plugin.Demo.BasicPlugin - Geo-Location Vendor Matching

A demo plugin for nopCommerce that adds geo-location based vendor matching capabilities.

## Features

### Core Features
- Distance-based vendor search
- Location-aware product listings
- Vendor radius filtering
- Geographic search optimization

### Technical Components

#### 1. Data Layer
- **GeoCoordinate**: Value object for latitude/longitude
- **VendorLocation**: Entity extending vendor with geo data
- **LocationIndex**: Spatial index for efficient geo-queries
- **SearchParameters**: Value object for search criteria

#### 2. Services
- **IGeocodingService**
  - Convert addresses to coordinates
  - Caching of geocoded locations
  - Batch geocoding capabilities

- **IDistanceCalculationService**
  - Calculate distances between points
  - Support multiple distance metrics
  - Optimize for bulk calculations

- **IVendorLocationService**
  - Manage vendor location data
  - Handle location updates
  - Maintain spatial indices

- **IGeoSearchService**
  - Execute geo-spatial queries
  - Apply search filters
  - Sort by distance

#### 3. Models
- **VendorLocationModel**: DTO for vendor location data
- **GeoSearchModel**: Search parameters and filters
- **LocationViewModel**: View-specific location data
- **SearchResultModel**: Geo-enriched search results

#### 4. Controllers
- **VendorLocationController**
  - Manage vendor locations
  - Handle location updates
  - Process search requests

#### 5. Components
- **VendorLocationWidget**
  - Display vendor locations
  - Show distance information
  - Render location filters

#### 6. Infrastructure
- **GeoSearchConfiguration**
  - Search radius limits
  - Distance calculation settings
  - Caching parameters

- **LocationDataBuilder**
  - Initialize location data
  - Build spatial indices
  - Handle data migrations

#### 7. Integration Points
- **Vendor Entity Extension**
  - Add location coordinates
  - Cache geocoding results
  - Track location updates

- **Product Search Integration**
  - Inject distance filters
  - Add location-based sorting
  - Enhance search results

- **Admin Panel Integration**
  - Location management UI
  - Geocoding controls
  - Search configuration

## Technical Design

### Data Flow
1. Address → Geocoding Service → Coordinates
2. Coordinates → Spatial Index → Quick Lookup
3. Search Query → Geo Filter → Sorted Results

### Performance Optimizations
- Spatial indexing for fast queries
- Coordinate caching
- Batch geocoding
- Result pagination
- Distance approximation

### Scalability Considerations
- Distributed spatial indices
- Caching strategies
- Batch processing
- Query optimization

## Project Structure

```
Nop.Plugin.Demo.BasicPlugin/
├── Components/
│   ├── VendorLocationWidget.cs
│   └── LocationFilterComponent.cs
├── Controllers/
│   └── VendorLocationController.cs
├── Data/
│   ├── GeoCoordinate.cs
│   ├── VendorLocation.cs
│   └── LocationIndex.cs
├── Infrastructure/
│   ├── GeoSearchConfiguration.cs
│   └── LocationDataBuilder.cs
├── Models/
│   ├── VendorLocationModel.cs
│   ├── GeoSearchModel.cs
│   └── SearchResultModel.cs
├── Services/
│   ├── GeocodingService.cs
│   ├── DistanceCalculationService.cs
│   ├── VendorLocationService.cs
│   └── GeoSearchService.cs
├── Views/
│   ├── VendorLocation/
│   │   ├── Configure.cshtml
│   │   └── Search.cshtml
│   └── Components/
│       └── VendorLocationWidget/
│           └── Default.cshtml
├── plugin.json
└── DemoPlugin.cs
```

## Implementation Strategy

### Phase 1: Core Infrastructure
- Basic geocoding service
- Vendor location storage
- Simple distance calculations

### Phase 2: Search Features
- Location-based filtering
- Distance sorting
- Basic UI components

### Phase 3: Optimization
- Spatial indexing
- Caching implementation
- Performance tuning

### Phase 4: Advanced Features
- Radius search
- Location clustering
- Advanced filtering

## Configuration

Configure through:
`Admin → Configuration → Local Plugins → Demo Basic Plugin → Configure`

Settings include:
- Default search radius
- Distance units
- Geocoding service settings
- Cache duration
- Index update frequency
