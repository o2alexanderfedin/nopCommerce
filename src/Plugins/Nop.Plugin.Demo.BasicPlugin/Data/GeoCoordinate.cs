using System;

namespace Nop.Plugin.Demo.BasicPlugin.Data
{
    /// <summary>
    /// Represents a geographic coordinate (latitude/longitude pair)
    /// </summary>
    public readonly struct GeoCoordinate : IEquatable<GeoCoordinate>
    {
        /// <summary>
        /// Gets the latitude in degrees
        /// </summary>
        public decimal Latitude { get; }

        /// <summary>
        /// Gets the longitude in degrees
        /// </summary>
        public decimal Longitude { get; }

        /// <summary>
        /// Initializes a new instance of the GeoCoordinate struct
        /// </summary>
        /// <param name="latitude">Latitude in degrees (-90 to 90)</param>
        /// <param name="longitude">Longitude in degrees (-180 to 180)</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when latitude or longitude is out of valid range</exception>
        public GeoCoordinate(decimal latitude, decimal longitude)
        {
            if (latitude < -90 || latitude > 90)
                throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be between -90 and 90 degrees");
            
            if (longitude < -180 || longitude > 180)
                throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude must be between -180 and 180 degrees");

            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        /// Calculates the distance between two coordinates using the Haversine formula
        /// </summary>
        /// <param name="other">The other coordinate to calculate distance to</param>
        /// <returns>Distance in kilometers</returns>
        public decimal DistanceTo(GeoCoordinate other)
        {
            const decimal EarthRadiusKm = 6371;

            var dLat = ToRadians(other.Latitude - Latitude);
            var dLon = ToRadians(other.Longitude - Longitude);

            var lat1 = ToRadians(Latitude);
            var lat2 = ToRadians(other.Latitude);

            var a = (decimal)Math.Sin((double)(dLat / 2)) * (decimal)Math.Sin((double)(dLat / 2)) +
                   (decimal)Math.Sin((double)(dLon / 2)) * (decimal)Math.Sin((double)(dLon / 2)) * 
                   (decimal)Math.Cos((double)lat1) * (decimal)Math.Cos((double)lat2);
            var c = 2 * (decimal)Math.Atan2(Math.Sqrt((double)a), Math.Sqrt((double)(1 - a)));

            return EarthRadiusKm * c;
        }

        private static decimal ToRadians(decimal degrees)
        {
            return degrees * (decimal)Math.PI / 180;
        }

        public override bool Equals(object obj)
        {
            return obj is GeoCoordinate coordinate && Equals(coordinate);
        }

        public bool Equals(GeoCoordinate other)
        {
            return Latitude == other.Latitude && Longitude == other.Longitude;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Latitude, Longitude);
        }

        public static bool operator ==(GeoCoordinate left, GeoCoordinate right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GeoCoordinate left, GeoCoordinate right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return $"({Latitude}, {Longitude})";
        }
    }
}
