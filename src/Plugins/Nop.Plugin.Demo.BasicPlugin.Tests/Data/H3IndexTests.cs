using System;
using Xunit;
using Nop.Plugin.Demo.BasicPlugin.Data;

namespace Nop.Plugin.Demo.BasicPlugin.Tests.Data;

public class H3IndexTests
{
    [Fact]
    public void FromCoordinate_WithValidCoordinate_ReturnsH3Index()
    {
        // Arrange
        var coordinate = new GeoCoordinate(37.4224764m, -122.0842499m);

        // Act
        var h3Index = H3Index.FromCoordinate(coordinate);

        // Assert
        Assert.True(h3Index.Value > 0);
    }

    [Fact]
    public void FromCoordinate_WithSameCoordinate_ReturnsSameIndex()
    {
        // Arrange
        var coordinate1 = new GeoCoordinate(37.4224764m, -122.0842499m);
        var coordinate2 = new GeoCoordinate(37.4224764m, -122.0842499m);

        // Act
        var h3Index1 = H3Index.FromCoordinate(coordinate1);
        var h3Index2 = H3Index.FromCoordinate(coordinate2);

        // Assert
        Assert.Equal(h3Index1.Value, h3Index2.Value);
    }

    [Fact]
    public void FromCoordinate_WithDifferentCoordinates_ReturnsDifferentIndexes()
    {
        // Arrange
        var coordinate1 = new GeoCoordinate(37.4224764m, -122.0842499m);
        var coordinate2 = new GeoCoordinate(37.7749295m, -122.4194155m);

        // Act
        var h3Index1 = H3Index.FromCoordinate(coordinate1);
        var h3Index2 = H3Index.FromCoordinate(coordinate2);

        // Assert
        Assert.NotEqual(h3Index1.Value, h3Index2.Value);
    }

    [Fact]
    public void FromCoordinate_WithInvalidCoordinate_ThrowsArgumentOutOfRangeException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => H3Index.FromCoordinate(new GeoCoordinate(91m, 0m))); // Invalid latitude
    }
}
