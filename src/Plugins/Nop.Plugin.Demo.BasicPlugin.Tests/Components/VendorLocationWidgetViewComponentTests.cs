using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using Nop.Core.Domain.Vendors;
using Nop.Plugin.Demo.BasicPlugin.Components;
using Nop.Plugin.Demo.BasicPlugin.Data;
using Nop.Plugin.Demo.BasicPlugin.Models;
using Nop.Plugin.Demo.BasicPlugin.Services;
using Xunit;

namespace Nop.Plugin.Demo.BasicPlugin.Tests.Components;

public class VendorLocationWidgetViewComponentTests
{
    private readonly Mock<IVendorLocationService> _vendorLocationServiceMock;
    private readonly VendorLocationWidgetViewComponent _component;

    public VendorLocationWidgetViewComponentTests()
    {
        _vendorLocationServiceMock = new Mock<IVendorLocationService>();
        _component = new VendorLocationWidgetViewComponent(_vendorLocationServiceMock.Object);
    }

    [Fact]
    public async Task InvokeAsync_WithValidVendor_ReturnsView()
    {
        // Arrange
        var vendor = new Vendor { Id = 1, Name = "Test Vendor" };
        var location = new VendorLocation { VendorId = vendor.Id, Latitude = 37.4224764m, Longitude = -122.0842499m };

        _vendorLocationServiceMock.Setup(x => x.GetVendorLocationAsync(vendor.Id))
            .ReturnsAsync(location);

        // Act
        var result = await _component.InvokeAsync("VendorLocation", vendor) as ViewViewComponentResult;

        // Assert
        Assert.NotNull(result);
        var model = result.ViewData.Model as VendorLocation;
        Assert.NotNull(model);
        Assert.Equal(location.VendorId, model.VendorId);
        Assert.Equal(location.Latitude, model.Latitude);
        Assert.Equal(location.Longitude, model.Longitude);
    }

    [Fact]
    public async Task InvokeAsync_WithInvalidVendor_ReturnsEmptyContent()
    {
        // Arrange
        var invalidVendor = new object(); // Not a vendor

        // Act
        var result = await _component.InvokeAsync("VendorLocation", invalidVendor) as ContentViewComponentResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(string.Empty, result.Content);
    }

    [Fact]
    public async Task InvokeAsync_WithNullVendor_ReturnsEmptyContent()
    {
        // Act
        var result = await _component.InvokeAsync("VendorLocation", null) as ContentViewComponentResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(string.Empty, result.Content);
    }
}
