using Microsoft.AspNetCore.Mvc;
using TiemKiet.Models;

namespace TiemKiet.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(BlogPost blog)
        {
            return View(blog);
        }    
    }
}
