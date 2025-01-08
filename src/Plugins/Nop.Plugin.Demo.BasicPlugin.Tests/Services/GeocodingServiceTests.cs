using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Directory;
using Nop.Plugin.Demo.BasicPlugin.Data;
using Nop.Plugin.Demo.BasicPlugin.Services;
using Nop.Services.Directory;
using Xunit;

namespace Nop.Plugin.Demo.BasicPlugin.Tests.Services;

public class GeocodingServiceTests
{
    private readonly Mock<IMemoryCache> _cacheMock;
    private readonly Mock<IStateProvinceService> _stateProvinceServiceMock;
    private readonly Mock<ICountryService> _countryServiceMock;
    private readonly GeocodingService _service;

    public GeocodingServiceTests()
    {
        _cacheMock = new Mock<IMemoryCache>();
        _stateProvinceServiceMock = new Mock<IStateProvinceService>();
        _countryServiceMock = new Mock<ICountryService>();

        _service = new GeocodingService(
            _cacheMock.Object,
            _stateProvinceServiceMock.Object,
            _countryServiceMock.Object);
    }

    [Fact]
    public async Task GeocodeAddressAsync_WithValidAddress_ReturnsCoordinates()
    {
        // Arrange
        var address = new Address
        {
            Address1 = "1600 Amphitheatre Parkway",
            City = "Mountain View",
            StateProvinceId = 1,
            CountryId = 1
        };

        var stateProvince = new StateProvince { Name = "California" };
        var country = new Country { Name = "United States" };

        _stateProvinceServiceMock.Setup(x => x.GetStateProvinceByIdAsync(address.StateProvinceId.Value))
            .ReturnsAsync(stateProvince);
        _countryServiceMock.Setup(x => x.GetCountryByIdAsync(address.CountryId.Value))
            .ReturnsAsync(country);

        // Act
        var result = await _service.GeocodeAddressAsync(address);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Value.Latitude > 0);
        Assert.True(result.Value.Longitude < 0);
    }

    [Fact]
    public async Task GeocodeAddressAsync_WithInvalidAddress_ReturnsNull()
    {
        // Arrange
        var address = new Address
        {
            Address1 = null,
            City = null,
            StateProvinceId = null,
            CountryId = null
        };

        // Act
        var result = await _service.GeocodeAddressAsync(address);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GeocodeAddressAsync_WithCachedResult_ReturnsCachedCoordinates()
    {
        // Arrange
        var address = new Address
        {
            Address1 = "1600 Amphitheatre Parkway",
            City = "Mountain View",
            StateProvinceId = 1,
            CountryId = 1
        };

        var stateProvince = new StateProvince { Name = "California" };
        var country = new Country { Name = "United States" };
        var cachedCoordinate = new GeoCoordinate(37.4224764m, -122.0842499m);

        _stateProvinceServiceMock.Setup(x => x.GetStateProvinceByIdAsync(address.StateProvinceId.Value))
            .ReturnsAsync(stateProvince);
        _countryServiceMock.Setup(x => x.GetCountryByIdAsync(address.CountryId.Value))
            .ReturnsAsync(country);

        var cacheEntry = Mock.Of<ICacheEntry>();
        _cacheMock.Setup(x => x.CreateEntry(It.IsAny<object>()))
            .Returns(cacheEntry);

        // Act
        var result = await _service.GeocodeAddressAsync(address);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Value.Latitude > 0);
        Assert.True(result.Value.Longitude < 0);
    }
}
