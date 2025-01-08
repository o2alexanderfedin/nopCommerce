using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Vendors;
using Nop.Plugin.Demo.BasicPlugin.Data;

namespace Nop.Plugin.Demo.BasicPlugin.Services
{
    /// <summary>
    /// Service for managing vendor locations
    /// </summary>
    public interface IVendorLocationService
    {
        /// <summary>
        /// Gets a vendor location by vendor ID
        /// </summary>
        /// <param name="vendorId">The vendor ID</param>
        /// <returns>The vendor location, or null if not found</returns>
        Task<VendorLocation> GetVendorLocationAsync(int vendorId);

        /// <summary>
        /// Updates or creates a vendor location
        /// </summary>
        /// <param name="vendorLocation">The vendor location to save</param>
        Task SaveVendorLocationAsync(VendorLocation vendorLocation);

        /// <summary>
        /// Finds vendors within a specified radius of a point
        /// </summary>
        /// <param name="parameters">The search parameters</param>
        /// <returns>A paginated list of vendors with their distances</returns>
        Task<IPagedList<(Vendor Vendor, double DistanceKm)>> FindVendorsNearbyAsync(GeoSearchParameters parameters);

        /// <summary>
        /// Gets the distance between a point and a vendor
        /// </summary>
        /// <param name="coordinate">The coordinate to measure from</param>
        /// <param name="vendorId">The vendor ID to measure to</param>
        /// <returns>The distance in kilometers, or null if vendor location not found</returns>
        Task<double?> GetDistanceToVendorAsync(GeoCoordinate coordinate, int vendorId);

        /// <summary>
        /// Updates vendor locations from their addresses
        /// </summary>
        /// <param name="vendorIds">Optional list of vendor IDs to update. If null, updates all vendors.</param>
        /// <returns>Number of locations successfully updated</returns>
        Task<int> UpdateVendorLocationsFromAddressesAsync(IEnumerable<int> vendorIds = null);

        /// <summary>
        /// Validates a vendor location
        /// </summary>
        /// <param name="vendorLocation">The vendor location to validate</param>
        /// <returns>True if valid, false otherwise</returns>
        bool ValidateVendorLocation(VendorLocation vendorLocation);
    }
}
