using System;
using Nop.Core;

namespace Nop.Plugin.Demo.BasicPlugin.Data
{
    using H3IndexFactory = H3Index;
    
    /// <summary>
    /// Represents a vendor's geographic location
    /// </summary>
    public class VendorLocation : BaseEntity
    {
        /// <summary>
        /// Gets or sets the vendor identifier
        /// </summary>
        public int VendorId { get; set; }

        /// <summary>
        /// Gets or sets the latitude
        /// </summary>
        public decimal Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude
        /// </summary>
        public decimal Longitude { get; set; }

        /// <summary>
        /// Gets or sets the H3 index at resolution 9 (~150m hexagons)
        /// </summary>
        public ulong H3Index { get; set; }

        /// <summary>
        /// Gets or sets the date when coordinates were last updated
        /// </summary>
        public DateTime LastUpdatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets whether the coordinates are manually set
        /// </summary>
        public bool IsManuallySet { get; set; }

        /// <summary>
        /// Gets the coordinates as a GeoCoordinate value object
        /// </summary>
        public GeoCoordinate Coordinates
        {
            get => new(Latitude, Longitude);
            set
            {
                Latitude = value.Latitude;
                Longitude = value.Longitude;
                H3Index = H3IndexFactory.FromCoordinate(value).Value;
            }
        }

        /// <summary>
        /// Gets or sets whether this location is active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets whether this location has been verified
        /// </summary>
        public bool IsVerified { get; set; }

        /// <summary>
        /// Gets or sets any notes about this location
        /// </summary>
        public string Notes { get; set; }
    }
}
