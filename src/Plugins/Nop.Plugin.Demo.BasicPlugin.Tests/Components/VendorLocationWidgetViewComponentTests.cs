using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using Nop.Plugin.Demo.BasicPlugin.Components;
using Nop.Plugin.Demo.BasicPlugin.Data;
using Nop.Plugin.Demo.BasicPlugin.Services;
using Xunit;

namespace Nop.Plugin.Demo.BasicPlugin.Tests.Components;

public class VendorLocationWidgetViewComponentTests
{
    private readonly Mock<IVendorLocationService> _vendorLocationServiceMock;
    private readonly VendorLocationWidgetViewComponent _viewComponent;

    public VendorLocationWidgetViewComponentTests()
    {
        _vendorLocationServiceMock = new Mock<IVendorLocationService>();
        _viewComponent = new VendorLocationWidgetViewComponent(_vendorLocationServiceMock.Object);
    }

    [Fact]
    public async Task InvokeAsync_WithValidVendorId_ReturnsVendorLocation()
    {
        // Arrange
        var vendorId = 1;
        var vendorLocation = new VendorLocationModel
        {
            Id = vendorId,
            Name = "Test Vendor",
            Distance = 5.0,
            Latitude = 37.4224764,
            Longitude = -122.0842499,
            Address = "Test Address"
        };

        _vendorLocationServiceMock.Setup(x => x.GetVendorLocationAsync(vendorId))
            .ReturnsAsync(vendorLocation);

        // Act
        var result = await _viewComponent.InvokeAsync(vendorId) as ViewViewComponentResult;

        // Assert
        Assert.NotNull(result);
        var model = Assert.IsAssignableFrom<VendorLocationModel>(result.ViewData.Model);
        Assert.Equal(vendorId, model.Id);
        Assert.Equal("Test Vendor", model.Name);
        Assert.Equal(5.0, model.Distance);
        Assert.Equal(37.4224764, model.Latitude);
        Assert.Equal(-122.0842499, model.Longitude);
        Assert.Equal("Test Address", model.Address);
    }

    [Fact]
    public async Task InvokeAsync_WithInvalidVendorId_ReturnsNull()
    {
        // Arrange
        var vendorId = 999;

        _vendorLocationServiceMock.Setup(x => x.GetVendorLocationAsync(vendorId))
            .ReturnsAsync((VendorLocationModel)null);

        // Act
        var result = await _viewComponent.InvokeAsync(vendorId) as ViewViewComponentResult;

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.ViewData.Model);
    }
}
