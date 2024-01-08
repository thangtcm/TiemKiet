using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TiemKiet.Services.Interface;

namespace TiemKiet.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;
        private readonly IBranchService _branchService;
        public ProductController(IProductService productService, ILogger<ProductController> logger, IBranchService branchService)
        {
            _productService = productService;
            _logger = logger;
            _branchService = branchService;
        }

        public async Task<IActionResult> Index(int branchId = 1)
        {
            try
            {
                var branchlst = await _branchService.GetListAsync(null, 3);
                ViewData["ListBranchSelect"] = new SelectList(branchlst, "Id", "BranchName", branchId);
                ViewData["CurrentBranchId"] = branchId;
                var products = await _productService.GetCategoriesWithProductsAsync(branchId);
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
