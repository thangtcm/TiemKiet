using Firebase.Auth;
using Firebase.Storage;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Xml.Linq;
using TiemKiet.Models;
using TiemKiet.Models.ViewModel;
using TiemKiet.Services.Interface;

namespace TiemKiet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IBlogService _blogService;
        private readonly IProductService _productService;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IBlogService blogService, IProductService productService)
        {
            _logger = logger;
            _configuration = configuration;
            _blogService = blogService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.Keys.Contains("ToastrMessage") && HttpContext.Session.Keys.Contains("ToastrMessageType"))
            {
                TempData["ToastrMessage"] = HttpContext.Session.GetString("ToastrMessage");
                TempData["ToastrMessageType"] = HttpContext.Session.GetString("ToastrMessageType");

                HttpContext.Session.Remove("ToastrMessage");
                HttpContext.Session.Remove("ToastrMessageType");
            }
            var blogs = await _blogService.GetListNewAsync();
            var products = await _productService.GetListFeatured();
            var model = new SettingVMInfo
            {
                BlogLst = blogs.ToList(),
                ProductLst = products.Select(x => new ProductInfoVM(x)).ToList()
            };
            return View(model);
        }

        [Route("Home/Privacy")]
        [Route("Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [Route("Home/Terms-of-user")]
        [Route("Terms-of-user")]
        public IActionResult Terms()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}