using Google.Apis.Storage.v1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TiemKiet.Helpers;
using TiemKiet.Models;
using TiemKiet.Models.ViewModel;
using TiemKiet.Services.Interface;

namespace TiemKiet.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = Constants.Policies.RequireAdmin)]
    public class BranchController : Controller
    {
        private readonly ILogger<BranchController> _logger;
        private readonly IBranchService _branchService;
        private readonly IUserService _userService;
        private readonly IProvinceService _provinceService;
        public BranchController(ILogger<BranchController> logger, IUserService userService, IBranchService branchService,
            IProvinceService provinceService)
        {
            _logger = logger;
            _userService = userService;
            _branchService = branchService;
            _provinceService = provinceService;
        }

        public async Task<IActionResult> Index()
        {
            var branches = await _branchService.GetListAsync(x => x.Include(x => x.Imagelist!));
            return View(branches);
        }

        public async Task<IActionResult> Create()
        {
            var ProvinceLst = await _provinceService.GetListAsync();
            ViewData["ProvinceLst"] = new SelectList(ProvinceLst, "Id", "CityName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BranchInfoVM model, int districtId)
        {
            ICollection<Province> ProvinceLst  = new List<Province>();
            try
            {
                var user = await _userService.GetUser();
                if(user == null)
                {
                    ProvinceLst = await _provinceService.GetListAsync();
                    ViewData["ProvinceLst"] = new SelectList(ProvinceLst, "Id", "CityName");
                    ModelState.AddModelError(string.Empty, "Bạn cần đăng nhập.");
                    return View();
                }
                await _branchService.Add(model, user.Id, districtId);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            ProvinceLst = await _provinceService.GetListAsync();
            ViewData["ProvinceLst"] = new SelectList(ProvinceLst, "Id", "CityName");
            ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi không xác định.");
            return View();
        }
    }
}
