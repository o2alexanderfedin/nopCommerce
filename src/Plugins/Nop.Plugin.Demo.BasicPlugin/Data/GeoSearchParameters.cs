using System;

namespace Nop.Plugin.Demo.BasicPlugin.Data
{
    /// <summary>
    /// Represents parameters for geographic vendor search
    /// </summary>
    public class GeoSearchParameters
    {
        /// <summary>
        /// Gets or sets the center point for the search
        /// </summary>
        public GeoCoordinate Center { get; set; }

        /// <summary>
        /// Gets or sets the search radius in kilometers
        /// </summary>
        public double RadiusKm { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of results to return
        /// </summary>
        public int MaxResults { get; set; }

        /// <summary>
        /// Gets or sets whether to include inactive vendors
        /// </summary>
        public bool IncludeInactive { get; set; }

        /// <summary>
        /// Gets or sets whether to include unverified locations
        /// </summary>
        public bool IncludeUnverified { get; set; }

        /// <summary>
        /// Gets or sets the page number (1-based)
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the page size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Creates a new instance of GeoSearchParameters
        /// </summary>
        public GeoSearchParameters()
        {
            // Set default values
            MaxResults = 100;
            PageNumber = 1;
            PageSize = 10;
            RadiusKm = 50; // Default 50km radius
            IncludeInactive = false;
            IncludeUnverified = false;
        }

        /// <summary>
        /// Validates the search parameters
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when parameters are invalid</exception>
        public void Validate()
        {
            if (RadiusKm <= 0)
                throw new ArgumentException("Search radius must be positive", nameof(RadiusKm));

            if (MaxResults <= 0)
                throw new ArgumentException("Max results must be positive", nameof(MaxResults));

            if (PageNumber <= 0)
                throw new ArgumentException("Page number must be positive", nameof(PageNumber));

            if (PageSize <= 0)
                throw new ArgumentException("Page size must be positive", nameof(PageSize));
        }
    }
}
