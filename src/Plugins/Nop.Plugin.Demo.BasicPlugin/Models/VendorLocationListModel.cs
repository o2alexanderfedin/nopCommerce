using System.Collections.Generic;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Demo.BasicPlugin.Models
{
    public record VendorLocationListModel : BaseNopModel
    {
        public VendorLocationListModel()
        {
            Vendors = new List<VendorLocationModel>();
        }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public double RadiusKm { get; set; }
        public List<VendorLocationModel> Vendors { get; set; }
    }
}
