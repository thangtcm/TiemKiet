using Google.Apis.Storage.v1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TiemKiet.Helpers;
using TiemKiet.Services;
using TiemKiet.Services.Interface;
using TiemKiet.ViewModel;

namespace TiemKiet.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [Authorize(Policy = Constants.Policies.RequireAdmin)]
    public class ProvinceController : Controller
    {
        private readonly IProvinceService _provinceService;
        private readonly ILogger<CountryController> _logger;
        private readonly ICountryService _countryService;
        private readonly IUserService _userService;
        public ProvinceController(IProvinceService provinceService, ILogger<CountryController> logger, IUserService userService, ICountryService countryService)
        {
            _provinceService = provinceService;
            _logger = logger;
            _userService = userService;
            _countryService = countryService;
        }

        public async Task<IActionResult> Index(int countryId = 1)
        {
            var provincelst = await _provinceService.GetListAsync(countryId, x => x.Include(x => x.Country!));
            return View(provincelst);
        }

        public async Task<IActionResult> Create()
        {
            var countries = await _countryService.GetListAsync();
            ViewData["Countrylst"] = new SelectList(countries, "Id", "CountryName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProvinceInfoVM model)
        {
            try
            {
                var user = await _userService.GetUser();
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Bạn cần đăng nhập.");
                    return View();
                }
                await _provinceService.Add(model, user.Id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return View();
        }
    }
}
