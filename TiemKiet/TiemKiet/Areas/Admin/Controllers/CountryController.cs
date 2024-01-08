using Google.Apis.Storage.v1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TiemKiet.Helpers;
using TiemKiet.Models.ViewModel;
using TiemKiet.Services.Interface;

namespace TiemKiet.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [Authorize(Policy = Constants.Policies.RequireAdmin)]
    public class CountryController : Controller
    {
        private readonly ICountryService _countryService;
        private readonly ILogger<CountryController> _logger;
        private readonly IUserService _userService;
        public CountryController(ICountryService countryService, IUserService userService, ILogger<CountryController> logger)
        {
            _countryService = countryService;
            _userService = userService;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            var countries = await _countryService.GetListAsync();
            return View(countries.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CountryInfoVM model)
        {
            try
            {
                var user = await _userService.GetUser();
                if(user == null)
                {
                    ModelState.AddModelError(string.Empty, "Bạn cần đăng nhập.");
                    return View();
                }
                this.AddToastrMessage("Tạo quốc gia thành công", Enums.ToastrMessageType.Success);
                await _countryService.Add(model, user.Id);
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
