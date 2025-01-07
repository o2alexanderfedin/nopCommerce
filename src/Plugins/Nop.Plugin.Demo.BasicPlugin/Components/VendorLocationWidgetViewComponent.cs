using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Demo.BasicPlugin.Components
{
    [ViewComponent(Name = "VendorLocationWidget")]
    public class VendorLocationWidgetViewComponent : NopViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData = null)
        {
            return View("~/Plugins/Demo.BasicPlugin/Views/Components/VendorLocationWidget/Default.cshtml");
        }
    }
}
