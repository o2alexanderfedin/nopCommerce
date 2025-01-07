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
    public class H3Index
    {
        private const int DefaultResolution = 9; // ~150m hexagons

        /// <summary>
        /// Gets the H3 index for a coordinate at the default resolution
        /// </summary>
        public ulong GetIndex(GeoCoordinate coordinate)
        {
            return GetIndex(coordinate, DefaultResolution);
        }

        /// <summary>
        /// Gets the H3 index for a coordinate at a specific resolution
        /// </summary>
        public ulong GetIndex(GeoCoordinate coordinate, int resolution)
        {
            var h3Coord = new GeoCoord(coordinate.Latitude, coordinate.Longitude);
            return Api.GeoToH3(h3Coord, resolution);
        }

        /// <summary>
        /// Gets the center coordinate of an H3 index
        /// </summary>
        public GeoCoordinate GetCenterCoordinate(ulong h3Index)
        {
            var h3Center = new GeoCoord();
            Api.H3ToGeo(h3Index, out h3Center);
            return new GeoCoordinate(h3Center.Latitude, h3Center.Longitude);
        }

        /// <summary>
        /// Gets all neighboring indexes within k distance
        /// </summary>
        public IEnumerable<ulong> GetKRing(ulong h3Index, int k)
        {
            var outCells = new List<H3Lib.H3Index>();
            Api.KRing(h3Index, k, out outCells);
            return outCells.Select(x => x.Value);
        }

        /// <summary>
        /// Gets all neighboring indexes within a specified radius in kilometers
        /// </summary>
        public IEnumerable<ulong> GetIndexesWithinRadius(GeoCoordinate center, decimal radiusKm)
        {
            // Calculate the k-ring size based on the radius
            // At resolution 9, each hexagon is about 150m across
            var hexagonRadiusKm = 0.15m;
            var k = (int)Math.Ceiling((double)(radiusKm / hexagonRadiusKm));

            var centerIndex = GetIndex(center);
            var indexes = GetKRing(centerIndex, k);

            // Filter indexes by actual distance
            return indexes.Where(idx =>
            {
                var idxCenter = GetCenterCoordinate(idx);
                return center.DistanceTo(idxCenter) <= radiusKm;
            });
        }

        /// <summary>
        /// Checks if two indexes are neighbors
        /// </summary>
        public bool AreNeighbors(ulong h3Index1, ulong h3Index2)
        {
            return Api.H3IndexesAreNeighbors(h3Index1, h3Index2) == 1;
        }

        /// <summary>
        /// Gets the approximate edge length of hexagons at a resolution in kilometers
        /// </summary>
        public decimal GetHexagonEdgeLengthKm(int resolution)
        {
            return Api.EdgeLengthKm(resolution);
        }

        /// <summary>
        /// Gets the area of a hexagon at a resolution in square kilometers
        /// </summary>
        public decimal GetHexagonAreaKm2(int resolution)
        {
            return Api.HexAreaKm2(resolution);
        }
    }
}
