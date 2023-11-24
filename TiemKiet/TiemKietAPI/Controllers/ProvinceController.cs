using Microsoft.AspNetCore.Mvc;
using TiemKiet.Helpers;
using TiemKiet.Models;
using TiemKiet.Services.Interface;
using TiemKiet.ViewModel;
using X.PagedList;

namespace TiemKietAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinceController : ControllerBase
    {
        private readonly IProvinceService _provinceService;
        private readonly ILogger<ProvinceController> _logger;
        private readonly IUserService _userService;
        public ProvinceController(IProvinceService provinceService, ILogger<ProvinceController> logger, IUserService userService)
        {
            _provinceService = provinceService;
            _logger = logger;
            _userService = userService;
        }
        [HttpGet("GetProvinces")]
        public async Task<IActionResult> Get(int? page, int? countryId = 1)
        {
            ICollection<Province> provinces = new List<Province>();
            try
            {
                if (countryId.HasValue)
                {
                    provinces = await _provinceService.GetListAsync(countryId.Value);
                }
                else
                {
                    provinces = await _provinceService.GetListAsync();
                }
                int pagesize = 10;
                int maxpage = (provinces.Count / pagesize) + (provinces.Count % 10 == 0 ? 0 : 1);
                int pagenumber = page == null || page < 0 ? 1 : page.Value;
                PagedList<Province> lst = new(provinces, pagenumber, pagesize);
                var provincelst = lst.Select(provinces => new ProvinceInfoVM
                {
                    ProvinceId = provinces.Id,
                    ProvinceName = provinces.CityName,
                }).ToList();

                return StatusCode(StatusCodes.Status200OK, ResponseResult.CreateResponse("Success", "Đã lấy danh sách thành công.", new { Data = provincelst, MaxPage = maxpage }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Error Server", "Đã có lỗi xảy ra từ máy chủ."));  
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(ProvinceInfoVM provinceInfo, long userId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Value Not Valid", $"Dữ liệu nhập vào không hợp lệ - {ModelState}."));
                var user = await _userService.GetUser(userId);
                if(user == null) return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("User NotFound", $"Không tìm thấy dữ liệu của người dùng."));
                await _provinceService.Add(provinceInfo, userId);
                return StatusCode(StatusCodes.Status200OK, ResponseResult.CreateResponse("Create Success", $"Tạo {provinceInfo.ProvinceName} thành công."));
            }
            catch(Exception ex)
            {
                _logger.LogError($"Lỗi Add Province : {ex.Message.ToString()}");
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseResult.CreateResponse("Error Server", $"Đã có lỗi xảy ra từ máy chủ. {ex.Message}"));
            }
        }
    }
}
