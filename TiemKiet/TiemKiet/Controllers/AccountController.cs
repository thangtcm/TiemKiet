using Microsoft.AspNetCore.Mvc;

namespace TiemKiet.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
