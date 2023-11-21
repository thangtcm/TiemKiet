using Microsoft.AspNetCore.Mvc;

namespace TiemKiet.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CountryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
