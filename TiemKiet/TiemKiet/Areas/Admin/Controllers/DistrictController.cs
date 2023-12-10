using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using TiemKiet.Helpers;
using TiemKiet.Models;
using TiemKiet.Services;
using TiemKiet.Services.Interface;
using TiemKiet.ViewModel;

namespace TiemKiet.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = Constants.Policies.RequireAdmin)]
    public class DistrictController : Controller
    { 
        private readonly ILogger<DistrictController> _logger;
        private readonly IDistrictService _districtService;
        private readonly IProvinceService _provinceService;
        private readonly IUserService _userService;
        public DistrictController(ILogger<DistrictController> logger, IDistrictService districtService, 
            IProvinceService provinceService, IUserService userService)
        {
            _logger = logger;
            _districtService = districtService;
            _provinceService = provinceService;
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            var districts = await _districtService.GetListAsync(x => x.Include(x => x.Province!));
            return View(districts);
        }

        public async Task<IActionResult> Create()
        {
            var provincelst = await _provinceService.GetListAsync();
            ViewData["ProvinceLst"] = new SelectList(provincelst, "Id", "CityName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DistrictInfoVM model)
        {
            ICollection<Province> provincelst;
            try
            {
                var user = await _userService.GetUser();
                if(user == null)
                {
                    provincelst = await _provinceService.GetListAsync();
                    ViewData["ProvinceLst"] = new SelectList(provincelst, "Id", "CityName");
                    ModelState.AddModelError(string.Empty, "Bạn cần đăng nhập.");
                    return View();
                }    
                await _districtService.Add(model, user.Id);
                return RedirectToAction(nameof(Index));
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            provincelst = await _provinceService.GetListAsync();
            ViewData["ProvinceLst"] = new SelectList(provincelst, "Id", "CityName");
            return View();
        }
    }
}
