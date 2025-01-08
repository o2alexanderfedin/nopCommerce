using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Demo.BasicPlugin.Controllers
{
    public class LicenseController : BasePluginController
    {
        [HttpGet]
        [Route("/vendor-location/license")]
        public async Task<IActionResult> Index()
        {
            var licenseFilePath = Path.Combine(CommonHelper.MapPath("~/Plugins/Demo.BasicPlugin"), "LICENSE.md");
            if (!System.IO.File.Exists(licenseFilePath))
                return NotFound();

            var licenseContent = await System.IO.File.ReadAllTextAsync(licenseFilePath);
            return Content(licenseContent, "text/markdown");
        }
    }
}
