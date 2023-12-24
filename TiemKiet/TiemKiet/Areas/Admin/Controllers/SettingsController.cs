using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TiemKiet.Helpers;
using TiemKiet.Services.Interface;
using TiemKiet.Models.ViewModel;

namespace TiemKiet.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = Constants.Policies.RequireAdmin)]
    public class SettingsController : Controller
    {
        private readonly IBannerService _bannerService;
        public SettingsController(IBannerService bannerService) {
            _bannerService = bannerService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CreateBanner()
        {
            var bannerlst = await _bannerService.GetListAsync();
            var listImg = bannerlst.Select(x => x.UrlBanner).ToList();
            return View(new BannerInfoVM(listImg));
        }
    }
}
