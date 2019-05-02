using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LogoPaasSampleApp.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        [SwaggerOperation("Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
