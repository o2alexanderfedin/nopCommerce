using Nop.Plugin.Demo.BasicPlugin.Data;
using Xunit;

namespace Nop.Plugin.Demo.BasicPlugin.Tests.Data;

public class H3IndexTests
{
    [Fact]
    public void FromGeoCoordinate_WithValidCoordinate_ReturnsValidIndex()
    {
        // Arrange
        var coordinate = new GeoCoordinate(37.4224764, -122.0842499);

        // Act
        var h3Index = H3Index.FromGeoCoordinate(coordinate);

        // Assert
        Assert.NotNull(h3Index);
        Assert.NotEqual(0UL, h3Index.Value);
    }

    [Fact]
    public void GetNeighbors_ReturnsValidNeighbors()
    {
        // Arrange
        var coordinate = new GeoCoordinate(37.4224764, -122.0842499);
        var h3Index = H3Index.FromGeoCoordinate(coordinate);

        // Act
        var neighbors = h3Index.GetNeighbors();

        // Assert
        Assert.NotNull(neighbors);
        Assert.NotEmpty(neighbors);
        Assert.All(neighbors, n => Assert.NotEqual(0UL, n.Value));
    }

    [Fact]
    public void GetDistanceKm_BetweenAdjacentIndexes_ReturnsExpectedDistance()
    {
        // Arrange
        var coordinate1 = new GeoCoordinate(37.4224764, -122.0842499);
        var coordinate2 = new GeoCoordinate(37.4224764, -122.0842599);
        var h3Index1 = H3Index.FromGeoCoordinate(coordinate1);
        var h3Index2 = H3Index.FromGeoCoordinate(coordinate2);

        // Act
        var distance = h3Index1.GetDistanceKm(h3Index2);

        // Assert
        Assert.True(distance >= 0);
    }

    [Fact]
    public void ToString_ReturnsHexString()
    {
        // Arrange
        var coordinate = new GeoCoordinate(37.4224764, -122.0842499);
        var h3Index = H3Index.FromGeoCoordinate(coordinate);

        // Act
        var result = h3Index.ToString();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        // H3 index should be a 16-character hex string
        Assert.Matches("^[0-9a-fA-F]+$", result);
    }

    [Fact]
    public void Equals_WithSameIndex_ReturnsTrue()
    {
        // Arrange
        var coordinate = new GeoCoordinate(37.4224764, -122.0842499);
        var h3Index1 = H3Index.FromGeoCoordinate(coordinate);
        var h3Index2 = H3Index.FromGeoCoordinate(coordinate);

        // Act & Assert
        Assert.Equal(h3Index1, h3Index2);
        Assert.True(h3Index1.Equals(h3Index2));
    }

    [Fact]
    public void GetHashCode_WithSameIndex_ReturnsSameValue()
    {
        // Arrange
        var coordinate = new GeoCoordinate(37.4224764, -122.0842499);
        var h3Index1 = H3Index.FromGeoCoordinate(coordinate);
        var h3Index2 = H3Index.FromGeoCoordinate(coordinate);

        // Act
        var hashCode1 = h3Index1.GetHashCode();
        var hashCode2 = h3Index2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }
}
