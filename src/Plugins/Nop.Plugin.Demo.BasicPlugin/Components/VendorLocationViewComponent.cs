using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Demo.BasicPlugin.Components
{
    public class VendorLocationViewComponent : NopViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("~/Plugins/Demo.BasicPlugin/Views/VendorLocation.cshtml");
        }
    }
}
