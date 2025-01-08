using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Nop.Core;
using Nop.Core.Domain.Vendors;
using Nop.Data;
using Nop.Plugin.Demo.BasicPlugin.Data;
using Nop.Plugin.Demo.BasicPlugin.Services;
using Nop.Services.Vendors;
using Xunit;

namespace Nop.Plugin.Demo.BasicPlugin.Tests.Services;

public class VendorLocationServiceTests
{
    private readonly Mock<IRepository<VendorLocation>> _vendorLocationRepositoryMock;
    private readonly Mock<IGeocodingService> _geocodingServiceMock;
    private readonly Mock<IVendorService> _vendorServiceMock;
    private readonly VendorLocationService _service;

    public VendorLocationServiceTests()
    {
        _vendorLocationRepositoryMock = new Mock<IRepository<VendorLocation>>();
        _geocodingServiceMock = new Mock<IGeocodingService>();
        _vendorServiceMock = new Mock<IVendorService>();

        _service = new VendorLocationService(
            _vendorLocationRepositoryMock.Object,
            _geocodingServiceMock.Object,
            _vendorServiceMock.Object);
    }

    [Fact]
    public async Task GetVendorLocationAsync_WithExistingVendor_ReturnsLocation()
    {
        // Arrange
        var vendorId = 1;
        var vendorLocation = new VendorLocation
        {
            VendorId = vendorId,
            Latitude = 37.4224764m,
            Longitude = -122.0842499m
        };

        var locations = new List<VendorLocation> { vendorLocation };
        _vendorLocationRepositoryMock.Setup(x => x.Table)
            .Returns(locations.AsQueryable());

        // Act
        var result = await _service.GetVendorLocationAsync(vendorId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(vendorId, result.VendorId);
        Assert.Equal(vendorLocation.Latitude, result.Latitude);
        Assert.Equal(vendorLocation.Longitude, result.Longitude);
    }

    [Fact]
    public async Task GetVendorLocationAsync_WithNonExistingVendor_ReturnsNull()
    {
        // Arrange
        var vendorId = 999;
        var locations = new List<VendorLocation>();
        _vendorLocationRepositoryMock.Setup(x => x.Table)
            .Returns(locations.AsQueryable());

        // Act
        var result = await _service.GetVendorLocationAsync(vendorId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task FindVendorsNearbyAsync_WithValidParameters_ReturnsVendors()
    {
        // Arrange
        var searchParams = new GeoSearchParameters
        {
            Center = new GeoCoordinate(37.4224764m, -122.0842499m),
            RadiusKm = 10,
            MaxResults = 10,
            IncludeInactive = false
        };

        var vendor1 = new Vendor { Id = 1, Name = "Vendor 1" };
        var vendor2 = new Vendor { Id = 2, Name = "Vendor 2" };
        var vendors = new List<Vendor> { vendor1, vendor2 };

        var location1 = new VendorLocation
        {
            VendorId = vendor1.Id,
            Latitude = 37.4224764m,
            Longitude = -122.0842499m,
            H3Index = H3Index.FromCoordinate(new GeoCoordinate(37.4224764m, -122.0842499m)).Value
        };

        var location2 = new VendorLocation
        {
            VendorId = vendor2.Id,
            Latitude = 37.4224764m,
            Longitude = -122.0842499m,
            H3Index = H3Index.FromCoordinate(new GeoCoordinate(37.4224764m, -122.0842499m)).Value
        };

        var locations = new List<VendorLocation> { location1, location2 };
        _vendorLocationRepositoryMock.Setup(x => x.Table)
            .Returns(locations.AsQueryable());

        var pagedVendors = new PagedList<Vendor>(vendors, 0, 10);
        _vendorServiceMock.Setup(x => x.GetAllVendorsAsync(
                It.Is<string>(s => s == "Active"),
                It.Is<string>(s => s == null || s == string.Empty),
                It.Is<int>(i => i == 0),
                It.Is<int>(i => i == int.MaxValue),
                It.Is<bool>(b => b == false)))
            .ReturnsAsync(pagedVendors);

        // Act
        var result = await _service.FindVendorsNearbyAsync(searchParams);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, x => x.Vendor.Id == vendor1.Id);
        Assert.Contains(result, x => x.Vendor.Id == vendor2.Id);
        Assert.All(result, x => Assert.True(x.DistanceKm <= searchParams.RadiusKm));
    }
}
