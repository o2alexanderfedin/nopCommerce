using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Vendors;
using Nop.Plugin.Demo.BasicPlugin.Services;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Demo.BasicPlugin.Components
{
    [ViewComponent(Name = "VendorLocationWidget")]
    public class VendorLocationWidgetViewComponent : NopViewComponent
    {
        private readonly IVendorLocationService _vendorLocationService;

        public VendorLocationWidgetViewComponent(IVendorLocationService vendorLocationService)
        {
            _vendorLocationService = vendorLocationService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData = null)
        {
            if (additionalData is not Vendor vendor)
                return Content(string.Empty);

            var location = await _vendorLocationService.GetVendorLocationAsync(vendor.Id);
            if (location == null)
                return Content(string.Empty);

            return View("~/Plugins/Demo.BasicPlugin/Views/Components/VendorLocationWidget/Default.cshtml", location);
        }
    }
}
