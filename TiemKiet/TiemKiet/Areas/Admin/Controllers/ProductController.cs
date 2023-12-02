using Google.Apis.Storage.v1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TiemKiet.Enums;
using TiemKiet.Helpers;
using TiemKiet.Services.Interface;
using TiemKiet.ViewModel;

namespace TiemKiet.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [Authorize(Policy = Constants.Policies.RequireStaff)]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;
        private readonly IUserService _userService;
        private readonly IBranchService _branchService;
        public ProductController(IProductService productService, ILogger<ProductController> logger, IUserService userService, IBranchService branchService)
        {
            _productService = productService;
            _logger = logger;
            _userService = userService;
            _branchService = branchService;
        }

        public async Task<IActionResult> Index()
        {
            var productlst = await _productService.GetListAsync();
            return View(productlst.Select(x => new ProductInfoVM(x)).ToList());
        }

        public async Task<IActionResult> Create()
        {
            var branchlst = await _branchService.GetListAsync();
            ViewData["BranchLst"] = new SelectList(branchlst, "Id", "BranchName");
            var enumData = from ProductType e in Enum.GetValues(typeof(ProductType))
            select new
            {
                ID = (int)e,
                Name = e.ToString()
            };
            ViewData["ProductTypeLst"] = new SelectList(enumData, "ID", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductInfoVM model, int branchId)
        {
            try
            {
                var user = await _userService.GetUser();
                if(user == null)
                {
                    ModelState.AddModelError(string.Empty, "Bạn cần đăng nhập.");
                    return View();
                }    
                await _productService.Add(model, user.Id, branchId);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return View();
        }
    }
}
