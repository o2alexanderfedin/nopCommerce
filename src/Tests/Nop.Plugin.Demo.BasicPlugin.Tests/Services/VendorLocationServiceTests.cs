using Moq;
using Nop.Core;
using Nop.Core.Domain.Vendors;
using Nop.Data;
using Nop.Plugin.Demo.BasicPlugin.Data;
using Nop.Plugin.Demo.BasicPlugin.Services;
using NUnit.Framework;

namespace Nop.Plugin.Demo.BasicPlugin.Tests.Services;

[TestFixture]
public class VendorLocationServiceTests
{
    private Mock<IRepository<VendorLocation>> _vendorLocationRepositoryMock = null!;
    private Mock<IGeocodingService> _geocodingServiceMock = null!;
    private IVendorLocationService _vendorLocationService = null!;

    [SetUp]
    public void Setup()
    {
        _vendorLocationRepositoryMock = new Mock<IRepository<VendorLocation>>();
        _geocodingServiceMock = new Mock<IGeocodingService>();
        _vendorLocationService = new VendorLocationService(
            _vendorLocationRepositoryMock.Object,
            _geocodingServiceMock.Object);
    }

    [Test]
    public async Task GetVendorLocationAsync_ExistingVendorId_ReturnsLocation()
    {
        // Arrange
        var vendorId = 1;
        var vendorLocation = new VendorLocation
        {
            VendorId = vendorId,
            Latitude = 37.7749m,
            Longitude = -122.4194m,
            H3Index = 617733122422996991UL
        };

        _vendorLocationRepositoryMock.Setup(x => x.Table)
            .Returns(new[] { vendorLocation }.AsQueryable());

        // Act
        var result = await _vendorLocationService.GetVendorLocationAsync(vendorId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.VendorId, Is.EqualTo(vendorId));
        Assert.That(result.Latitude, Is.EqualTo(37.7749m));
        Assert.That(result.Longitude, Is.EqualTo(-122.4194m));
    }

    [Test]
    public async Task SaveVendorLocationAsync_NewLocation_SavesSuccessfully()
    {
        // Arrange
        var vendorLocation = new VendorLocation
        {
            VendorId = 1,
            Latitude = 37.7749m,
            Longitude = -122.4194m
        };

        _vendorLocationRepositoryMock.Setup(x => x.InsertAsync(It.IsAny<VendorLocation>(), default))
            .Returns(Task.CompletedTask);

        // Act
        await _vendorLocationService.SaveVendorLocationAsync(vendorLocation);

        // Assert
        _vendorLocationRepositoryMock.Verify(x => x.InsertAsync(It.Is<VendorLocation>(v => 
            v.VendorId == vendorLocation.VendorId &&
            v.Latitude == vendorLocation.Latitude &&
            v.Longitude == vendorLocation.Longitude), default), Times.Once);
    }

    [Test]
    public async Task FindNearbyVendorsAsync_ValidParameters_ReturnsVendors()
    {
        // Arrange
        var coordinate = new GeoCoordinate(37.7749m, -122.4194m);
        var radiusKm = 5;

        var vendorLocations = new List<VendorLocation>
        {
            new() { VendorId = 1, Latitude = 37.7749m, Longitude = -122.4194m, H3Index = 617733122422996991UL },
            new() { VendorId = 2, Latitude = 37.7750m, Longitude = -122.4195m, H3Index = 617733122422996991UL }
        };

        _vendorLocationRepositoryMock.Setup(x => x.Table)
            .Returns(vendorLocations.AsQueryable());

        // Act
        var results = await _vendorLocationService.FindNearbyVendorsAsync(coordinate, radiusKm);

        // Assert
        Assert.That(results, Is.Not.Null);
        Assert.That(results.Count(), Is.EqualTo(2));
        Assert.That(results.Select(v => v.VendorId), Does.Contain(1));
        Assert.That(results.Select(v => v.VendorId), Does.Contain(2));
    }

    [Test]
    public async Task DeleteVendorLocationAsync_ExistingLocation_DeletesSuccessfully()
    {
        // Arrange
        var vendorLocation = new VendorLocation { VendorId = 1 };

        _vendorLocationRepositoryMock.Setup(x => x.Table)
            .Returns(new[] { vendorLocation }.AsQueryable());
        _vendorLocationRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<VendorLocation>(), default))
            .Returns(Task.CompletedTask);

        // Act
        await _vendorLocationService.DeleteVendorLocationAsync(1);

        // Assert
        _vendorLocationRepositoryMock.Verify(x => x.DeleteAsync(It.Is<VendorLocation>(v => 
            v.VendorId == vendorLocation.VendorId), default), Times.Once);
    }
}
