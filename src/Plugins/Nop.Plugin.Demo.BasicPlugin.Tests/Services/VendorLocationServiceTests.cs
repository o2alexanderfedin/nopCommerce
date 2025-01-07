using Microsoft.Extensions.Caching.Memory;
using Moq;
using Nop.Core.Domain.Vendors;
using Nop.Plugin.Demo.BasicPlugin.Data;
using Nop.Plugin.Demo.BasicPlugin.Services;
using Nop.Services.Vendors;
using Xunit;

namespace Nop.Plugin.Demo.BasicPlugin.Tests.Services;

public class VendorLocationServiceTests
{
    private readonly Mock<IVendorService> _vendorServiceMock;
    private readonly Mock<IGeocodingService> _geocodingServiceMock;
    private readonly Mock<IMemoryCache> _memoryCacheMock;
    private readonly VendorLocationService _vendorLocationService;

    public VendorLocationServiceTests()
    {
        _vendorServiceMock = new Mock<IVendorService>();
        _geocodingServiceMock = new Mock<IGeocodingService>();
        _memoryCacheMock = new Mock<IMemoryCache>();
        _vendorLocationService = new VendorLocationService(
            _vendorServiceMock.Object,
            _geocodingServiceMock.Object,
            _memoryCacheMock.Object);
    }

    [Fact]
    public async Task GetNearbyVendors_WithValidParameters_ReturnsVendors()
    {
        // Arrange
        var searchParams = new GeoSearchParameters
        {
            Latitude = 37.4224764,
            Longitude = -122.0842499,
            RadiusKm = 10
        };

        var vendors = new List<Vendor>
        {
            new() { Id = 1, Name = "Vendor 1", Address = "Address 1" },
            new() { Id = 2, Name = "Vendor 2", Address = "Address 2" }
        };

        var vendorLocations = new List<VendorLocation>
        {
            new() { VendorId = 1, Latitude = 37.4224764, Longitude = -122.0842499 },
            new() { VendorId = 2, Latitude = 37.4224764, Longitude = -122.0842499 }
        };

        _vendorServiceMock.Setup(x => x.GetAllVendorsAsync(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<bool>()))
            .ReturnsAsync((vendors, vendors.Count));

        foreach (var vendor in vendors)
        {
            _geocodingServiceMock.Setup(x => x.GeocodeAddressAsync(vendor.Address))
                .ReturnsAsync(new GeoCoordinate(37.4224764, -122.0842499));
        }

        // Act
        var result = await _vendorLocationService.GetNearbyVendorsAsync(searchParams);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.All(result, v => Assert.True(v.Distance <= searchParams.RadiusKm));
    }

    [Fact]
    public async Task GetNearbyVendors_WithNoVendors_ReturnsEmptyList()
    {
        // Arrange
        var searchParams = new GeoSearchParameters
        {
            Latitude = 37.4224764,
            Longitude = -122.0842499,
            RadiusKm = 10
        };

        _vendorServiceMock.Setup(x => x.GetAllVendorsAsync(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<bool>()))
            .ReturnsAsync((new List<Vendor>(), 0));

        // Act
        var result = await _vendorLocationService.GetNearbyVendorsAsync(searchParams);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetNearbyVendors_WithGeocodeFailure_SkipsVendor()
    {
        // Arrange
        var searchParams = new GeoSearchParameters
        {
            Latitude = 37.4224764,
            Longitude = -122.0842499,
            RadiusKm = 10
        };

        var vendors = new List<Vendor>
        {
            new() { Id = 1, Name = "Vendor 1", Address = "Address 1" },
            new() { Id = 2, Name = "Vendor 2", Address = "Invalid Address" }
        };

        _vendorServiceMock.Setup(x => x.GetAllVendorsAsync(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<bool>()))
            .ReturnsAsync((vendors, vendors.Count));

        _geocodingServiceMock.Setup(x => x.GeocodeAddressAsync("Address 1"))
            .ReturnsAsync(new GeoCoordinate(37.4224764, -122.0842499));

        _geocodingServiceMock.Setup(x => x.GeocodeAddressAsync("Invalid Address"))
            .ReturnsAsync((GeoCoordinate)null);

        // Act
        var result = await _vendorLocationService.GetNearbyVendorsAsync(searchParams);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Vendor 1", result[0].Name);
    }

    [Fact]
    public async Task GetNearbyVendors_WithCachedLocations_UsesCachedData()
    {
        // Arrange
        var searchParams = new GeoSearchParameters
        {
            Latitude = 37.4224764,
            Longitude = -122.0842499,
            RadiusKm = 10
        };

        var vendors = new List<Vendor>
        {
            new() { Id = 1, Name = "Vendor 1", Address = "Address 1" }
        };

        var cachedLocation = new VendorLocation
        {
            VendorId = 1,
            Latitude = 37.4224764,
            Longitude = -122.0842499
        };

        _vendorServiceMock.Setup(x => x.GetAllVendorsAsync(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<bool>()))
            .ReturnsAsync((vendors, vendors.Count));

        object cachedValue = cachedLocation;
        _memoryCacheMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out cachedValue))
            .Returns(true);

        // Act
        var result = await _vendorLocationService.GetNearbyVendorsAsync(searchParams);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        _geocodingServiceMock.Verify(x => x.GeocodeAddressAsync(It.IsAny<string>()), Times.Never);
    }
}
