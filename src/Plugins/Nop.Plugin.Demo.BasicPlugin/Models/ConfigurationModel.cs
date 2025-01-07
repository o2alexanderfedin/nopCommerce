using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Demo.BasicPlugin.Models
{
    public record ConfigurationModel : BaseNopModel
    {
        [NopResourceDisplayName("Plugins.Demo.BasicPlugin.DefaultSearchRadius")]
        public int DefaultSearchRadius { get; set; }

        [NopResourceDisplayName("Plugins.Demo.BasicPlugin.MaxSearchRadius")]
        public int MaxSearchRadius { get; set; }

        [NopResourceDisplayName("Plugins.Demo.BasicPlugin.EnableCaching")]
        public bool EnableCaching { get; set; }

        [NopResourceDisplayName("Plugins.Demo.BasicPlugin.CacheDurationMinutes")]
        public int CacheDurationMinutes { get; set; }
    }
}
