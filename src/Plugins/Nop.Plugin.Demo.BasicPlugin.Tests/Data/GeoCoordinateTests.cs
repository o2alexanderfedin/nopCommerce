using Nop.Plugin.Demo.BasicPlugin.Data;
using Xunit;

namespace Nop.Plugin.Demo.BasicPlugin.Tests.Data;

public class GeoCoordinateTests
{
    [Fact]
    public void Constructor_WithValidCoordinates_CreatesInstance()
    {
        // Arrange & Act
        var coordinate = new GeoCoordinate(37.4224764m, -122.0842499m);

        // Assert
        Assert.Equal(37.4224764m, coordinate.Latitude);
        Assert.Equal(-122.0842499m, coordinate.Longitude);
    }

    [Theory]
    [InlineData(91)]  // Greater than 90
    [InlineData(-91)] // Less than -90
    public void Constructor_WithInvalidLatitude_ThrowsArgumentOutOfRangeException(int latitude)
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new GeoCoordinate(latitude, 0m));
    }

    [Theory]
    [InlineData(181)]  // Greater than 180
    [InlineData(-181)] // Less than -180
    public void Constructor_WithInvalidLongitude_ThrowsArgumentOutOfRangeException(int longitude)
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new GeoCoordinate(0m, longitude));
    }

    [Fact]
    public void DistanceTo_WithValidCoordinates_ReturnsCorrectDistance()
    {
        // Arrange
        var coord1 = new GeoCoordinate(37.4224764m, -122.0842499m);
        var coord2 = new GeoCoordinate(37.4224764m, -122.0842499m);

        // Act
        var distance = coord1.DistanceTo(coord2);

        // Assert
        Assert.Equal(0m, distance);
    }

    [Fact]
    public void DistanceTo_WithDifferentCoordinates_ReturnsNonZeroDistance()
    {
        // Arrange
        var coord1 = new GeoCoordinate(37.4224764m, -122.0842499m);
        var coord2 = new GeoCoordinate(37.7749295m, -122.4194155m);

        // Act
        var distance = coord1.DistanceTo(coord2);

        // Assert
        Assert.True(distance > 0m);
    }

    [Fact]
    public void ToString_ReturnsFormattedString()
    {
        // Arrange
        var coordinate = new GeoCoordinate(37.4224764m, -122.0842499m);

        // Act
        var result = coordinate.ToString();

        // Assert
        Assert.Contains("37.4224764", result);
        Assert.Contains("-122.0842499", result);
    }
}
