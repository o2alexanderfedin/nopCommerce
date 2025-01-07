using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Services.Cms;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Demo.BasicPlugin
{
    /// <summary>
    /// Represents the demo plugin
    /// </summary>
    public class DemoPlugin : BasePlugin, IWidgetPlugin
    {
        /// <summary>
        /// Gets widget zones where this widget should be rendered
        /// </summary>
        /// <returns>Widget zones</returns>
        public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string> { PublicWidgetZones.ProductDetailsBottom });
        }

        /// <summary>
        /// Gets a type of a view component for displaying widget
        /// </summary>
        /// <param name="widgetZone">Name of the widget zone</param>
        /// <returns>View component type</returns>
        public Type GetWidgetViewComponent(string widgetZone)
        {
            return typeof(Components.VendorLocationViewComponent);
        }

        /// <summary>
        /// Gets a name of a view component for displaying widget
        /// </summary>
        /// <param name="widgetZone">Name of the widget zone</param>
        /// <returns>View component name</returns>
        public Task<string> GetWidgetViewComponentAsync(string widgetZone)
        {
            return Task.FromResult("VendorLocation");
        }

        /// <summary>
        /// Gets a value indicating whether to hide this plugin on the widget list page in the admin area
        /// </summary>
        public bool HideInWidgetList => false;
    }
}
