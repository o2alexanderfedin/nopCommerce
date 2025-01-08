using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Demo.BasicPlugin.Models
{
    public record ConfigurationModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }

        [NopResourceDisplayName("Plugins.Demo.BasicPlugin.DefaultSearchRadius")]
        public double DefaultSearchRadius { get; set; }

        [NopResourceDisplayName("Plugins.Demo.BasicPlugin.GeocodingApiKey")]
        public string GeocodingApiKey { get; set; }

        [NopResourceDisplayName("Plugins.Demo.BasicPlugin.GeocodeAddressCacheDuration")]
        public int GeocodeAddressCacheDuration { get; set; }

        public bool DefaultSearchRadius_OverrideForStore { get; set; }
        public bool GeocodingApiKey_OverrideForStore { get; set; }
        public bool GeocodeAddressCacheDuration_OverrideForStore { get; set; }
    }
}
