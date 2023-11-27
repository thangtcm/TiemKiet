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
    public class DistrictController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IDistrictService _districtService;
        private readonly ILogger<DistrictController> _logger;
        public DistrictController(IUserService userService, IDistrictService districtService, ILogger<DistrictController> logger)
        {
            _userService = userService;
            _districtService = districtService;
            _logger = logger;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] DistrictInfoVM districtInfo, [FromQuery] long userId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Value Not Valid", $"Dữ liệu nhập vào không hợp lệ - {ModelState}."));
                var user = await _userService.GetUser(userId);
                if (user == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("User NotFound", $"Người dùng không tồn tại."));
                }
                await _districtService.Add(districtInfo, userId);
                return Ok(ResponseResult.CreateResponse("Success", $"Tạo {districtInfo.DistrictName} thành công."));
            }
            catch
            (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseResult.CreateResponse("Error Server", $"Đã có lỗi xảy ra từ máy chủ. {ex.Message}"));
            }
        }

        [HttpGet("GetDistricts")]
        public async Task<IActionResult> Get(int? page, int provinceId = 1)
        {
            try
            {
                var districts = await _districtService.GetListAsync(provinceId);
                int pagesize = 10;
                int maxpage = (districts.Count / pagesize) + (districts.Count % 10 == 0 ? 0 : 1);
                int pagenumber = page == null || page < 0 ? 1 : page.Value;
                PagedList<District> lst = new(districts, pagenumber, pagesize);
                var districtlst = lst.Select(district => new DistrictInfoVM
                {
                    DistrictId = district.Id,
                    DistrictName = district.DistrictName!,
                }).ToList();

                return StatusCode(StatusCodes.Status200OK, ResponseResult.CreateResponse("Success", "Đã lấy danh sách thành công.", new { Data = districtlst, MaxPage = maxpage }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Error Server", "Đã có lỗi xảy ra từ máy chủ."));
        }

    }
}
