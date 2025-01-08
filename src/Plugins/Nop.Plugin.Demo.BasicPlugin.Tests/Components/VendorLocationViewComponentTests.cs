using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using Nop.Core;
using Nop.Core.Domain.Vendors;
using Nop.Plugin.Demo.BasicPlugin.Components;
using Nop.Plugin.Demo.BasicPlugin.Data;
using Nop.Plugin.Demo.BasicPlugin.Services;
using Xunit;

namespace Nop.Plugin.Demo.BasicPlugin.Tests.Components;

public class VendorLocationViewComponentTests
{
    private readonly Mock<IVendorLocationService> _vendorLocationServiceMock;
    private readonly VendorLocationViewComponent _component;

    public VendorLocationViewComponentTests()
    {
        _vendorLocationServiceMock = new Mock<IVendorLocationService>();
        _component = new VendorLocationViewComponent(_vendorLocationServiceMock.Object);
    }

    [Fact]
    public async Task InvokeAsync_ReturnsViewWithModel()
    {
        // Arrange
        var vendors = new List<(Vendor Vendor, double DistanceKm)>
        {
            (new Vendor { Id = 1, Name = "Vendor 1" }, 1.0),
            (new Vendor { Id = 2, Name = "Vendor 2" }, 2.0)
        };

        var pagedList = new PagedList<(Vendor Vendor, double DistanceKm)>(vendors, 0, vendors.Count);
        _vendorLocationServiceMock.Setup(x => x.FindVendorsNearbyAsync(It.IsAny<GeoSearchParameters>()))
            .ReturnsAsync(pagedList);

        // Act
        var result = await _component.InvokeAsync() as ViewViewComponentResult;

        // Assert
        Assert.NotNull(result);
        var model = result.ViewData.Model as IPagedList<(Vendor Vendor, double DistanceKm)>;
        Assert.NotNull(model);
        Assert.Equal(2, model.Count);
    }

    [Fact]
    public async Task InvokeAsync_WithNoVendors_ReturnsEmptyList()
    {
        // Arrange
        var vendors = new List<(Vendor Vendor, double DistanceKm)>();
        var pagedList = new PagedList<(Vendor Vendor, double DistanceKm)>(vendors, 0, 0);
        _vendorLocationServiceMock.Setup(x => x.FindVendorsNearbyAsync(It.IsAny<GeoSearchParameters>()))
            .ReturnsAsync(pagedList);

        // Act
        var result = await _component.InvokeAsync() as ViewViewComponentResult;

        // Assert
        Assert.NotNull(result);
        var model = result.ViewData.Model as IPagedList<(Vendor Vendor, double DistanceKm)>;
        Assert.NotNull(model);
        Assert.Empty(model);
    }
}
