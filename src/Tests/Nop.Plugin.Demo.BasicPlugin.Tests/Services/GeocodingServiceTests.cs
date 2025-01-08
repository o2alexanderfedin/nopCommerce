using Nop.Core.Domain.Common;
using Nop.Plugin.Demo.BasicPlugin.Data;
using Nop.Plugin.Demo.BasicPlugin.Services;
using NUnit.Framework;

namespace Nop.Plugin.Demo.BasicPlugin.Tests.Services;

[TestFixture]
public class GeocodingServiceTests
{
    private IGeocodingService _geocodingService = null!;

    [SetUp]
    public void Setup()
    {
        _geocodingService = new GeocodingService();
    }

    [Test]
    public async Task GeocodeAddressAsync_ValidAddress_ReturnsCoordinates()
    {
        // Arrange
        var address = new Address
        {
            Address1 = "1600 Amphitheatre Parkway",
            City = "Mountain View",
            StateProvinceId = 5, // California
            CountryId = 1 // USA
        };

        // Act
        var result = await _geocodingService.GeocodeAddressAsync(address);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Latitude, Is.InRange(37.0m, 38.0m)); // Approximate range for Mountain View
        Assert.That(result.Longitude, Is.InRange(-123.0m, -122.0m));
    }

    [Test]
    public async Task GeocodeAddressAsync_InvalidAddress_ReturnsNull()
    {
        // Arrange
        var address = new Address
        {
            Address1 = "ThisIsNotARealAddress12345"
        };

        // Act
        var result = await _geocodingService.GeocodeAddressAsync(address);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GeocodeAddressAsync_EmptyAddress_ReturnsNull()
    {
        // Arrange
        var address = new Address();

        // Act
        var result = await _geocodingService.GeocodeAddressAsync(address);

        // Assert
        Assert.That(result, Is.Null);
    }
}
