using Microsoft.AspNetCore.Mvc;
using TiemKiet.Services.Interface;

namespace TiemKiet.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;
        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var products = await _productService.GetCategoriesWithProductsAsync();
                return View(products);
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return NotFound();
            
        }

        public async Task<IActionResult> Details(int Id)
        {
            var product = await _productService.GetByIdAsync(Id);
            return View(product);
        }
    }
}
