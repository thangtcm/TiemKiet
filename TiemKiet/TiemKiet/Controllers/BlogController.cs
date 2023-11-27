using Microsoft.AspNetCore.Mvc;

namespace TiemKiet.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
