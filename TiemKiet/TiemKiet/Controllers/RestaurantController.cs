using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;
using System.Linq;
using System.Security.Claims;
using TiemKiet.Services.Interface;
using TiemKiet.ViewModel;

namespace TiemKiet.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly IBranchService _branchService;
        private readonly IDistrictService _districtService;
        private readonly IProvinceService _provinceService;
        private readonly ILogger<RestaurantController> _logger;
        public RestaurantController(IBranchService branchService, ILogger<RestaurantController> logger, 
            IDistrictService districtService, IProvinceService provinceService)
        {
            _branchService = branchService;
            _logger = logger;
            _districtService = districtService;
            _provinceService = provinceService;
        }

        public async Task<IActionResult> Index()
        {

            var branchLst = await _branchService.GetListWithBranchAsync();
            return View(branchLst);
        }
    }
}
