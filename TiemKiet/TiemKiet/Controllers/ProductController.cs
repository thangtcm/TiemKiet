using Microsoft.AspNetCore.Mvc;

namespace TiemKiet.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
