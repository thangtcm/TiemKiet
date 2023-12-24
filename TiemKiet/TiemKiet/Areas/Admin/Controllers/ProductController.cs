using Google.Apis.Storage.v1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TiemKiet.Enums;
using TiemKiet.Helpers;
using TiemKiet.Models;
using TiemKiet.Models.ViewModel;
using TiemKiet.Services.Interface;

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
            var productlst = await _productService.GetListBranchAsync(1, x => x.Include(x => x.Branch!));
            return View(productlst.Select(x => new ProductInfoVM(x)).ToList());
        }

        public IActionResult Create()
        {
            //var branchlst = await _branchService.GetListAsync();
            //ViewData["BranchLst"] = new SelectList(branchlst, "Id", "BranchName");
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
            ICollection<Branch> branchlst = new List<Branch>();
            var enumData = from ProductType e in Enum.GetValues(typeof(ProductType))
                select new
                {
                    ID = (int)e,
                    Name = e.ToString()
                };
            ViewData["ProductTypeLst"] = new SelectList(enumData, "ID", "Name");
            try
            {
                
                var user = await _userService.GetUser();
                if(user == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                await _productService.Add(model, user.Id, branchId);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            branchlst = await _branchService.GetListAsync();
            ViewData["BranchLst"] = new SelectList(branchlst, "Id", "BranchName");
            return View();
        }
 

        public async Task<IActionResult> Edit(int? Id)
        {
            if(Id == null)
            {
                return NotFound();
            }
            var product = await _productService.GetByIdAsync(Id.Value);
            if(product == null)
            {
                return NotFound();
            }
            //var branchlst = await _branchService.GetListAsync();
            //ViewData["BranchLst"] = new SelectList(branchlst, "Id", "BranchName", product.BranchId);
            var enumData = from ProductType e in Enum.GetValues(typeof(ProductType))
                           select new
                           {
                               ID = (int)e,
                               Name = e.ToString()
                           };
            ViewData["ProductTypeLst"] = new SelectList(enumData, "ID", "Name", product.ProductType);
            return View(new ProductInfoVM(product) { ProductDescription = product.ProductDescription ?? "",ProductMBDescription = product.ProductMBDescription ?? "" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int ProductId, ProductInfoVM model)
        {
            try
            {
                if(ProductId != model.ProductId)
                {
                    ModelState.AddModelError(string.Empty, "Lỗi không xác định.");
                    return RedirectToAction(nameof(Index));
                }
                var user = await _userService.GetUser();
                if(user == null)
                {
                    return RedirectToAction(nameof(Index));
                }    
                await _productService.Update(model, user.Id);
                return RedirectToAction(nameof(Index));
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            var enumData = from ProductType e in Enum.GetValues(typeof(ProductType))
                           select new
                           {
                               ID = (int)e,
                               Name = e.ToString()
                           };
            ViewData["ProductTypeLst"] = new SelectList(enumData, "ID", "Name", model.ProductType);
            return View();
        }
    }
}
