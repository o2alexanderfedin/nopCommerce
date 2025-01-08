#nullable enable

using Nop.Web.Framework.Models;

namespace Nop.Plugin.Demo.BasicPlugin.Models
{
    public record VendorLocationModel : BaseNopModel
    {
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public double? DistanceKm { get; set; }
        public string? Address { get; set; }
    }
}
