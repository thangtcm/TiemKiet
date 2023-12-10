using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TiemKiet.Services.Interface;

namespace TiemKiet.Controllers
{

    public class DistrictController : Controller
    {
        private readonly IDistrictService _districtService;
        public DistrictController(IDistrictService districtService)
        {
            _districtService = districtService;
        }
        public async  Task<IActionResult> GetDistrictByProvince(int provinceId)
        {
            var districtlst = await _districtService.GetListAsync(provinceId);

            var districtSelectList = new SelectList(districtlst, "Id", "DistrictName");
            return Json(districtSelectList);
        }
    }
}
