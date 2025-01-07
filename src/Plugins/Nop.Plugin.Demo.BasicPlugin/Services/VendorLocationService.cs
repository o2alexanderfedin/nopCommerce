using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Vendors;
using Nop.Data;
using Nop.Plugin.Demo.BasicPlugin.Data;
using Nop.Services.Vendors;

namespace Nop.Plugin.Demo.BasicPlugin.Services;

public class VendorLocationService : IVendorLocationService
{
    private readonly IRepository<VendorLocation> _vendorLocationRepository;
    private readonly IGeocodingService _geocodingService;
    private readonly IVendorService _vendorService;

    public VendorLocationService(
        IRepository<VendorLocation> vendorLocationRepository,
        IGeocodingService geocodingService,
        IVendorService vendorService)
    {
        _vendorLocationRepository = vendorLocationRepository;
        _geocodingService = geocodingService;
        _vendorService = vendorService;
    }

    public async Task<VendorLocation> GetVendorLocationAsync(int vendorId)
    {
        return await Task.FromResult(_vendorLocationRepository.Table
            .FirstOrDefault(v => v.VendorId == vendorId));
    }

    public async Task SaveVendorLocationAsync(VendorLocation vendorLocation)
    {
        if (vendorLocation == null)
            return;

        // Calculate H3 index for the location
        var coordinate = new GeoCoordinate(vendorLocation.Latitude, vendorLocation.Longitude);
        var h3Index = H3Index.FromCoordinate(coordinate);
        vendorLocation.H3Index = h3Index.Value;

        var existingLocation = await GetVendorLocationAsync(vendorLocation.VendorId);
        if (existingLocation == null)
        {
            await _vendorLocationRepository.InsertAsync(vendorLocation);
        }
        else
        {
            existingLocation.Latitude = vendorLocation.Latitude;
            existingLocation.Longitude = vendorLocation.Longitude;
            existingLocation.H3Index = vendorLocation.H3Index;
            await _vendorLocationRepository.UpdateAsync(existingLocation);
        }
    }

    public async Task<IPagedList<(Vendor Vendor, double DistanceKm)>> FindVendorsNearbyAsync(GeoSearchParameters parameters)
    {
        // Get the H3 index for the search coordinate
        var centerH3 = H3Index.FromCoordinate(parameters.Center);

        // Calculate k-ring size based on radius
        // At resolution 9, each hexagon is about 0.15km across
        var k = (int)(parameters.RadiusKm / 0.15);

        // Get all indexes within k-ring
        var searchIndexes = centerH3.GetKRing(k).Select(h => h.Value).ToHashSet();

        // Get all vendors first
        var vendors = await _vendorService.GetAllVendorsAsync(parameters.IncludeInactive ? string.Empty : "Active");

        // Find all vendors within these indexes
        var query = 
            from vl in _vendorLocationRepository.Table
            join v in vendors
            on vl.VendorId equals v.Id
            where searchIndexes.Contains(vl.H3Index)
            select new { Vendor = v, Location = vl };

        // Calculate distances and filter by actual radius
        var vendorsWithDistances = query
            .AsEnumerable() // Perform distance calculation in memory
            .Select(x =>
            {
                var vendorCoord = new GeoCoordinate(x.Location.Latitude, x.Location.Longitude);
                var distance = (double)parameters.Center.DistanceTo(vendorCoord);
                return (x.Vendor, distance);
            })
            .Where(x => x.distance <= parameters.RadiusKm)
            .OrderBy(x => x.distance)
            .ToList();

        if (parameters.MaxResults > 0)
        {
            vendorsWithDistances = vendorsWithDistances.Take(parameters.MaxResults).ToList();
        }

        // Apply paging
        return new PagedList<(Vendor, double)>(
            vendorsWithDistances,
            parameters.PageNumber - 1, // Convert to 0-based index
            parameters.PageSize);
    }

    public async Task<double?> GetDistanceToVendorAsync(GeoCoordinate coordinate, int vendorId)
    {
        var vendorLocation = await GetVendorLocationAsync(vendorId);
        if (vendorLocation == null)
            return null;

        var vendorCoord = new GeoCoordinate(vendorLocation.Latitude, vendorLocation.Longitude);
        return (double)coordinate.DistanceTo(vendorCoord);
    }

    public async Task<int> UpdateVendorLocationsFromAddressesAsync(IEnumerable<int> vendorIds = null)
    {
        IList<Vendor> vendors;
        if (vendorIds != null)
        {
            var vendorTasks = vendorIds.Select(_vendorService.GetVendorByIdAsync);
            var vendorArray = await Task.WhenAll(vendorTasks);
            vendors = vendorArray.Where(v => v != null).ToList();
        }
        else
        {
            vendors = await _vendorService.GetAllVendorsAsync(string.Empty);
        }

        var updatedCount = 0;
        foreach (var vendor in vendors)
        {
            // Get vendor's address from the vendor service
            var address = await _vendorService.GetVendorAddressAsync(vendor);
            if (address == null || !_geocodingService.CanGeocodeAddress(address))
                continue;

            var coordinate = await _geocodingService.TryGeocodeAddressWithCachingAsync(address);
            if (coordinate == null)
                continue;

            var location = new VendorLocation
            {
                VendorId = vendor.Id,
                Latitude = coordinate.Value.Latitude,
                Longitude = coordinate.Value.Longitude
            };

            await SaveVendorLocationAsync(location);
            updatedCount++;
        }

        return updatedCount;
    }

    public bool ValidateVendorLocation(VendorLocation location)
    {
        if (location == null)
            return false;

        // Basic validation of coordinates
        if (location.Latitude < -90 || location.Latitude > 90)
            return false;

        if (location.Longitude < -180 || location.Longitude > 180)
            return false;

        return true;
    }
}
