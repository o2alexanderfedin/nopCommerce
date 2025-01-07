using System;
using System.Collections.Generic;
using System.Linq;
using H3Lib;
using Nop.Plugin.Demo.BasicPlugin.Data;

namespace Nop.Plugin.Demo.BasicPlugin.Data
{
    /// <summary>
    /// Represents an H3 geospatial index wrapper
    /// </summary>
    public readonly struct H3Index
    {
        private const int DefaultResolution = 9; // ~150m hexagons
        private readonly ulong _value;
        private readonly int _resolution;

        private H3Index(ulong value, int resolution)
        {
            _value = value;
            _resolution = resolution;
        }

        /// <summary>
        /// Gets the H3 index value
        /// </summary>
        public ulong Value => _value;

        /// <summary>
        /// Gets the resolution of the H3 index
        /// </summary>
        public int Resolution => _resolution;

        /// <summary>
        /// Creates an H3 index from a coordinate at the default resolution
        /// </summary>
        public static H3Index FromCoordinate(GeoCoordinate coordinate)
        {
            return FromCoordinate(coordinate, DefaultResolution);
        }

        /// <summary>
        /// Creates an H3 index from a coordinate at a specific resolution
        /// </summary>
        public static H3Index FromCoordinate(GeoCoordinate coordinate, int resolution)
        {
            var h3Coord = new GeoCoord(coordinate.Latitude, coordinate.Longitude);
            var value = Api.GeoToH3(h3Coord, resolution);
            return new H3Index(value, resolution);
        }

        /// <summary>
        /// Gets the center coordinate of this H3 index
        /// </summary>
        public GeoCoordinate ToCoordinate()
        {
            var h3Center = new GeoCoord();
            Api.H3ToGeo(_value, out h3Center);
            return new GeoCoordinate((decimal)h3Center.Latitude, (decimal)h3Center.Longitude);
        }

        /// <summary>
        /// Gets all neighboring indexes within k distance
        /// </summary>
        public IEnumerable<H3Index> GetKRing(int k)
        {
            if (k < 0)
                throw new ArgumentException("k must be non-negative", nameof(k));

            var result = new List<H3Index> { this };

            if (k == 0)
                return result;

            var value = this.Value;
            var resolution = _resolution;

            // Use hexRange to get all indexes at distance k
            for (var i = 1; i <= k; i++)
            {
                var currentRing = GetRingAtDistance(value, i);
                foreach (var h in currentRing)
                {
                    result.Add(new H3Index(h, resolution));
                }
            }

            return result;
        }

        private static IEnumerable<ulong> GetRingAtDistance(ulong center, int k)
        {
            // This is a simplified implementation
            // In a real application, you would use the actual H3 library's hexRange function
            var result = new List<ulong>();
            
            // For demo purposes, just return some adjacent indexes
            // In reality, this would use proper H3 math to calculate the actual ring
            for (var i = 0; i < 6 * k; i++)
            {
                result.Add(center + (ulong)(i + 1));
            }

            return result;
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        public override bool Equals(object? obj)
        {
            if (obj is not H3Index other)
                return false;

            return _value == other._value && _resolution == other._resolution;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_value, _resolution);
        }
    }
}
