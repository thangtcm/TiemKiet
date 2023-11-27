using Microsoft.AspNetCore.Mvc;

namespace TiemKiet.Controllers
{
    public class RestaurantController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
