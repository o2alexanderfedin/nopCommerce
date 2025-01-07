using System.Threading.Tasks;
using Nop.Core.Domain.Common;
using Nop.Plugin.Demo.BasicPlugin.Data;

namespace Nop.Plugin.Demo.BasicPlugin.Services
{
    /// <summary>
    /// Service for converting addresses to geographic coordinates
    /// </summary>
    public interface IGeocodingService
    {
        /// <summary>
        /// Geocodes an address to get its coordinates
        /// </summary>
        /// <param name="address">The address to geocode</param>
        /// <returns>The coordinates of the address, or null if geocoding failed</returns>
        Task<GeoCoordinate?> GeocodeAddressAsync(Address address);

        /// <summary>
        /// Attempts to geocode an address, with caching
        /// </summary>
        /// <param name="address">The address to geocode</param>
        /// <returns>The coordinates of the address, or null if geocoding failed</returns>
        Task<GeoCoordinate?> TryGeocodeAddressWithCachingAsync(Address address);

        /// <summary>
        /// Validates whether an address can be geocoded
        /// </summary>
        /// <param name="address">The address to validate</param>
        /// <returns>True if the address can be geocoded, false otherwise</returns>
        bool CanGeocodeAddress(Address address);

        /// <summary>
        /// Clears the geocoding cache for an address
        /// </summary>
        /// <param name="address">The address to clear cache for</param>
        Task ClearCacheAsync(Address address);
    }
}
