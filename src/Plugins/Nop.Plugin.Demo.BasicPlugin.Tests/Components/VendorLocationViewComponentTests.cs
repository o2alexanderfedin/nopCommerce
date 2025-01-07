using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using Nop.Plugin.Demo.BasicPlugin.Components;
using Nop.Plugin.Demo.BasicPlugin.Data;
using Nop.Plugin.Demo.BasicPlugin.Services;
using Xunit;

namespace Nop.Plugin.Demo.BasicPlugin.Tests.Components;

public class VendorLocationViewComponentTests
{
    private readonly Mock<IVendorLocationService> _vendorLocationServiceMock;
    private readonly VendorLocationViewComponent _viewComponent;

    public VendorLocationViewComponentTests()
    {
        _vendorLocationServiceMock = new Mock<IVendorLocationService>();
        _viewComponent = new VendorLocationViewComponent(_vendorLocationServiceMock.Object);
    }

    [Fact]
    public async Task InvokeAsync_WithValidLocation_ReturnsVendorList()
    {
        // Arrange
        var searchParams = new GeoSearchParameters
        {
            Latitude = 37.4224764,
            Longitude = -122.0842499,
            RadiusKm = 10
        };

        var vendorLocations = new List<VendorLocationModel>
        {
            new() { Id = 1, Name = "Vendor 1", Distance = 5.0, Latitude = 37.4224764, Longitude = -122.0842499 },
            new() { Id = 2, Name = "Vendor 2", Distance = 7.0, Latitude = 37.4224764, Longitude = -122.0842499 }
        };

        _vendorLocationServiceMock.Setup(x => x.GetNearbyVendorsAsync(It.IsAny<GeoSearchParameters>()))
            .ReturnsAsync(vendorLocations);

        // Act
        var result = await _viewComponent.InvokeAsync(searchParams) as ViewViewComponentResult;

        // Assert
        Assert.NotNull(result);
        var model = Assert.IsAssignableFrom<List<VendorLocationModel>>(result.ViewData.Model);
        Assert.Equal(2, model.Count);
        Assert.Equal("Vendor 1", model[0].Name);
        Assert.Equal("Vendor 2", model[1].Name);
    }

    [Fact]
    public async Task InvokeAsync_WithNoVendors_ReturnsEmptyList()
    {
        // Arrange
        var searchParams = new GeoSearchParameters
        {
            Latitude = 37.4224764,
            Longitude = -122.0842499,
            RadiusKm = 10
        };

        _vendorLocationServiceMock.Setup(x => x.GetNearbyVendorsAsync(It.IsAny<GeoSearchParameters>()))
            .ReturnsAsync(new List<VendorLocationModel>());

        // Act
        var result = await _viewComponent.InvokeAsync(searchParams) as ViewViewComponentResult;

        // Assert
        Assert.NotNull(result);
        var model = Assert.IsAssignableFrom<List<VendorLocationModel>>(result.ViewData.Model);
        Assert.Empty(model);
    }
}
