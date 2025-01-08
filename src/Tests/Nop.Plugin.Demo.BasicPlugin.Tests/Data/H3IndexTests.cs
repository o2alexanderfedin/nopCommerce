using NUnit.Framework;
using Nop.Plugin.Demo.BasicPlugin.Data;

namespace Nop.Plugin.Demo.BasicPlugin.Tests.Data;

[TestFixture]
public class H3IndexTests
{
    [Test]
    public void FromGeoCoordinate_ValidCoordinates_ReturnsValidH3Index()
    {
        // Arrange
        var coordinate = new GeoCoordinate(37.7749m, -122.4194m); // San Francisco coordinates
        var resolution = 9;

        // Act
        var h3Index = H3Index.FromCoordinate(coordinate, resolution);

        // Assert
        Assert.That(h3Index, Is.Not.Null);
        Assert.That(h3Index.Resolution, Is.EqualTo(resolution));
        Assert.That(h3Index.Value, Is.Not.Empty);
    }

    [Test]
    public void ToGeoCoordinate_ValidH3Index_ReturnsValidCoordinate()
    {
        // Arrange
        var coordinate = new GeoCoordinate(37.7749m, -122.4194m);
        var resolution = 9;
        var h3Index = H3Index.FromCoordinate(coordinate, resolution);

        // Act
        var resultCoordinate = h3Index.ToCoordinate();

        // Assert
        Assert.That(resultCoordinate, Is.Not.Null);
        // Check if coordinates are approximately equal (within 0.1 degree)
        Assert.That(resultCoordinate.Latitude, Is.EqualTo(coordinate.Latitude).Within(0.1m));
        Assert.That(resultCoordinate.Longitude, Is.EqualTo(coordinate.Longitude).Within(0.1m));
    }

    [Test]
    public void GetKRing_ValidRadius_ReturnsCorrectNumberOfIndexes()
    {
        // Arrange
        var coordinate = new GeoCoordinate(37.7749m, -122.4194m);
        var resolution = 9;
        var h3Index = H3Index.FromCoordinate(coordinate, resolution);
        var radius = 1;

        // Act
        var kRing = h3Index.GetKRing(radius);

        // Assert
        Assert.That(kRing, Is.Not.Null);
        // For radius 1, we expect 7 hexagons (center + 6 neighbors)
        Assert.That(kRing.Count(), Is.EqualTo(7));
    }

    [Test]
    public void ToString_ValidH3Index_ReturnsNonEmptyString()
    {
        // Arrange
        var coordinate = new GeoCoordinate(37.7749m, -122.4194m);
        var resolution = 9;
        var h3Index = H3Index.FromCoordinate(coordinate, resolution);

        // Act
        var result = h3Index.ToString();

        // Assert
        Assert.That(result, Is.Not.Empty);
        Assert.That(result, Is.EqualTo(h3Index.Value));
    }

    [Test]
    public void Equals_SameH3Indexes_ReturnsTrue()
    {
        // Arrange
        var coordinate = new GeoCoordinate(37.7749m, -122.4194m);
        var resolution = 9;
        var h3Index1 = H3Index.FromCoordinate(coordinate, resolution);
        var h3Index2 = H3Index.FromCoordinate(coordinate, resolution);

        // Act & Assert
        Assert.That(h3Index1, Is.EqualTo(h3Index2));
        Assert.That(h3Index1.GetHashCode(), Is.EqualTo(h3Index2.GetHashCode()));
    }
}
