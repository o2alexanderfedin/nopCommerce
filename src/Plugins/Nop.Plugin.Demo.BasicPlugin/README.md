# nopCommerce Demo Basic Plugin

A geolocation-based vendor discovery solution for nopCommerce that enhances the marketplace experience by connecting customers with nearby vendors.

## Core Features

### Vendor Location Management
- Automatically geocodes vendor addresses into precise coordinates (latitude/longitude)
- Stores vendor locations using H3 spatial indexing for efficient geographic queries
- Maintains location data freshness through automatic updates when vendor addresses change

### Proximity-Based Vendor Search
- Allows customers to find vendors within a specified radius
- Supports configurable search radius (default and maximum)
- Returns vendors sorted by distance from a given location
- Optimizes search performance using H3 spatial indexing

### Multi-Store Support
- Configurable per store in multi-store setups
- Store-specific settings for:
  - Default search radius
  - Geocoding API keys
  - Cache duration for geocoded addresses

## Business Benefits

### For Customers
- Find local vendors quickly
- See vendor distances on product pages
- Make informed purchasing decisions based on vendor proximity
- Reduce shipping costs by choosing nearby vendors

### For Vendors
- Increased visibility to local customers
- Automatic location management (no manual coordinate entry needed)
- Competitive advantage in local markets
- Potential for reduced shipping costs

### For Store Owners
- Enhanced marketplace functionality
- Improved customer experience
- Efficient vendor discovery
- Reduced support overhead through automated location management
- Cache optimization for better performance

## Technical Features

### Geocoding Service
- Converts physical addresses to coordinates
- Caches results to reduce API calls
- Configurable cache duration
- API key management for geocoding services

### Location Services
- High-performance spatial queries using H3 indexing
- Accurate distance calculations
- Support for large vendor databases
- Efficient nearby vendor discovery

### UI Integration
- Vendor location widget on product pages
- Vendor search interface
- Distance display in search results
- Mobile-friendly design

## Configuration

### Search Settings
- Default search radius
- Maximum search radius
- Results limit

### Geocoding Settings
- API key configuration
- Cache duration
- Address validation options

### Display Settings
- Distance unit preference (km/miles)
- Location display format
- Widget placement options

## Installation

1. Download the plugin from the nopCommerce marketplace
2. Upload and install the plugin through your nopCommerce admin panel
3. Configure the plugin settings:
   - Enter your geocoding API key
   - Set default search radius
   - Configure cache duration
4. The plugin will automatically start geocoding vendor addresses

## Usage

### For Store Owners
1. Go to Configuration → Local Plugins
2. Find "Demo Basic Plugin" and click Configure
3. Set up your preferred configuration options
4. The plugin will automatically process vendor addresses

### For Customers
1. Browse products as usual
2. View vendor locations and distances on product pages
3. Use the vendor search to find nearby vendors
4. Filter search results by distance

## Technical Requirements

- nopCommerce 4.60 or later
- .NET 9.0
- Access to a geocoding service (API key required)
- Sufficient storage for location caching

## Integration Points

### nopCommerce Core
- Vendor management system
- Product catalog
- Multi-store functionality
- Address management

### External Services
- Geocoding APIs
- Mapping services (optional)
- Location validation services

### User Interface
- Product pages
- Vendor listings
- Search results
- Admin configuration panels

## Performance Considerations

- Uses H3 spatial indexing for efficient location queries
- Implements smart caching to reduce API calls
- Optimizes database queries for large vendor sets
- Supports high-traffic marketplace scenarios

## Support

For support, please:
1. Check the [documentation](https://docs.nopcommerce.com/en/plugins/demo-basic-plugin.html)
2. Visit our [community forums](https://www.nopcommerce.com/boards/)
3. Contact our [support team](https://www.nopcommerce.com/contact-us)

## Contributing

We welcome contributions! Please:
1. Fork the repository
2. Create a feature branch
3. Submit a pull request

## License

This plugin is released under the nopCommerce license. See the LICENSE file for details.
