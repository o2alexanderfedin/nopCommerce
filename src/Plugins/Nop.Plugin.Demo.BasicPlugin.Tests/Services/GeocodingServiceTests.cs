using Microsoft.Extensions.Caching.Memory;
using Moq;
using Nop.Core.Configuration;
using Nop.Plugin.Demo.BasicPlugin.Data;
using Nop.Plugin.Demo.BasicPlugin.Services;
using Nop.Services.Configuration;
using Xunit;

namespace Nop.Plugin.Demo.BasicPlugin.Tests.Services;

public class GeocodingServiceTests
{
    private readonly Mock<ISettingService> _settingServiceMock;
    private readonly Mock<IMemoryCache> _memoryCacheMock;
    private readonly GeocodingService _geocodingService;

    public GeocodingServiceTests()
    {
        _settingServiceMock = new Mock<ISettingService>();
        _memoryCacheMock = new Mock<IMemoryCache>();
        _geocodingService = new GeocodingService(_settingServiceMock.Object, _memoryCacheMock.Object);
    }

    [Fact]
    public async Task GeocodeAddress_WithValidAddress_ReturnsCoordinates()
    {
        // Arrange
        var address = "1600 Amphitheatre Parkway, Mountain View, CA";
        var expectedCoordinates = new GeoCoordinate(37.4224764, -122.0842499);

        _settingServiceMock.Setup(x => x.GetSettingByKeyAsync<string>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync("test-api-key");

        var cacheEntry = Mock.Of<ICacheEntry>();
        _memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>()))
            .Returns(cacheEntry);

        // Act
        var result = await _geocodingService.GeocodeAddressAsync(address);

        // Assert
        Assert.NotNull(result);
        Assert.True(Math.Abs(result.Latitude - expectedCoordinates.Latitude) < 0.01);
        Assert.True(Math.Abs(result.Longitude - expectedCoordinates.Longitude) < 0.01);
    }

    [Fact]
    public async Task GeocodeAddress_WithInvalidAddress_ReturnsNull()
    {
        // Arrange
        var address = "Invalid Address 12345";

        _settingServiceMock.Setup(x => x.GetSettingByKeyAsync<string>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync("test-api-key");

        // Act
        var result = await _geocodingService.GeocodeAddressAsync(address);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GeocodeAddress_WithCachedResult_ReturnsCachedCoordinates()
    {
        // Arrange
        var address = "1600 Amphitheatre Parkway, Mountain View, CA";
        var cachedCoordinates = new GeoCoordinate(37.4224764, -122.0842499);

        _settingServiceMock.Setup(x => x.GetSettingByKeyAsync<string>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync("test-api-key");

        object cachedValue = cachedCoordinates;
        _memoryCacheMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out cachedValue))
            .Returns(true);

        // Act
        var result = await _geocodingService.GeocodeAddressAsync(address);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(cachedCoordinates.Latitude, result.Latitude);
        Assert.Equal(cachedCoordinates.Longitude, result.Longitude);
    }

    [Fact]
    public async Task GeocodeAddress_WithNoApiKey_ReturnsNull()
    {
        // Arrange
        var address = "1600 Amphitheatre Parkway, Mountain View, CA";

        _settingServiceMock.Setup(x => x.GetSettingByKeyAsync<string>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(string.Empty);

        // Act
        var result = await _geocodingService.GeocodeAddressAsync(address);

        // Assert
        Assert.Null(result);
    }
}
