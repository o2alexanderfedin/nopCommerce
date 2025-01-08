using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Demo.BasicPlugin.Data;
using Nop.Plugin.Demo.BasicPlugin.Services;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Demo.BasicPlugin.Components
{
    public class VendorLocationViewComponent : NopViewComponent
    {
        private readonly IVendorLocationService _vendorLocationService;

        public VendorLocationViewComponent(IVendorLocationService vendorLocationService)
        {
            _vendorLocationService = vendorLocationService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Get default search parameters from settings
            var searchParameters = new GeoSearchParameters
            {
                MaxResults = 10,
                RadiusKm = 10,
                IncludeInactive = false
            };

            // Get nearby vendors
            var vendors = await _vendorLocationService.FindVendorsNearbyAsync(searchParameters);

            return View("~/Plugins/Demo.BasicPlugin/Views/VendorLocation.cshtml", vendors);
        }
    }
}
