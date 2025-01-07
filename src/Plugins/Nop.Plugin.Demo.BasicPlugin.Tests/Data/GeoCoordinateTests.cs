using Nop.Plugin.Demo.BasicPlugin.Data;
using Xunit;

namespace Nop.Plugin.Demo.BasicPlugin.Tests.Data;

public class GeoCoordinateTests
{
    [Fact]
    public void Constructor_WithValidCoordinates_CreatesInstance()
    {
        // Arrange & Act
        var coordinate = new GeoCoordinate(37.4224764, -122.0842499);

        // Assert
        Assert.Equal(37.4224764, coordinate.Latitude);
        Assert.Equal(-122.0842499, coordinate.Longitude);
    }

    [Theory]
    [InlineData(91)]
    [InlineData(-91)]
    public void Constructor_WithInvalidLatitude_ThrowsArgumentException(double latitude)
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => new GeoCoordinate(latitude, 0));
    }

    [Theory]
    [InlineData(181)]
    [InlineData(-181)]
    public void Constructor_WithInvalidLongitude_ThrowsArgumentException(double longitude)
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => new GeoCoordinate(0, longitude));
    }

    [Fact]
    public void CalculateDistance_BetweenTwoPoints_ReturnsCorrectDistance()
    {
        // Arrange
        var point1 = new GeoCoordinate(37.4224764, -122.0842499); // Google HQ
        var point2 = new GeoCoordinate(37.7749, -122.4194);      // San Francisco

        // Act
        var distance = point1.CalculateDistance(point2);

        // Assert
        Assert.True(distance > 40 && distance < 50); // Roughly 45 km
    }

    [Fact]
    public void CalculateDistance_ToSamePoint_ReturnsZero()
    {
        // Arrange
        var point = new GeoCoordinate(37.4224764, -122.0842499);

        // Act
        var distance = point.CalculateDistance(point);

        // Assert
        Assert.Equal(0, distance);
    }

    [Fact]
    public void ToString_ReturnsFormattedString()
    {
        // Arrange
        var coordinate = new GeoCoordinate(37.4224764, -122.0842499);

        // Act
        var result = coordinate.ToString();

        // Assert
        Assert.Contains("37.4224764", result);
        Assert.Contains("-122.0842499", result);
    }
}
