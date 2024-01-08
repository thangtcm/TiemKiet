using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TiemKiet.Helpers;
using TiemKiet.Services.Interface;
using TiemKiet.Models.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TiemKiet.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = Constants.Policies.RequireAdmin)]
    public class SettingsController : Controller
    {
        private readonly IBannerService _bannerService;
        private readonly ILogger<SettingsController> _logger;
        private readonly IUserService _userService;
        public SettingsController(IBannerService bannerService, ILogger<SettingsController> logger, IUserService userService)
        {
            _bannerService = bannerService;
            _logger = logger;
            _userService = userService;
        }

        public IActionResult Index()
        {
            ICollection<BannerInfoVM> list = new List<BannerInfoVM>() { new BannerInfoVM() };
            var model = new SettingVMInfo() { bannerInfoVMs = list.ToList()};
            return View(model);
        }    

        [HttpPost]
        public async Task<IActionResult> CreateBanner(SettingVMInfo model)
        {
            try
            {
                var user = await _userService.GetUser();
                if(user == null)
                {
                    return View(model);
                }    
                await _bannerService.AddRange(model.bannerInfoVMs, user.Id);
                this.AddToastrMessage("Tạo hình ảnh hiển thị banner thành công.", Enums.ToastrMessageType.Success);
                return RedirectToAction("Index", "Home");
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return View(model);
        }

        public IActionResult CreatePartialBanner()
        {
            ICollection<BannerInfoVM> list = new List<BannerInfoVM>() { new BannerInfoVM() };
            var model = new SettingVMInfo() { bannerInfoVMs = list.ToList() };
            return PartialView("_DynamicAddBanner", model);
        }
    }
}
