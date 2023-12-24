using Microsoft.AspNetCore.Mvc;
using TiemKiet.Helpers;
using TiemKiet.Models;
using TiemKiet.Models.ViewModel;
using TiemKiet.Services.Interface;
using X.PagedList;

namespace TiemKietAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<CountryController> _logger;
        private readonly ICountryService _countryService;
        public CountryController(IUserService userService, ILogger<CountryController> logger, ICountryService countryService)
        {
            _userService = userService;
            _logger = logger;
            _countryService = countryService;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] CountryInfoVM countryInfoVM,[FromQuery] long userId)
        {
            try
            {
                if(!ModelState.IsValid)
                    return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Value Not Valid", $"Dữ liệu nhập vào không hợp lệ - {ModelState}."));
                var user = await _userService.GetUser(userId);
                if(user == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("User NotFound", $"Người dùng không tồn tại."));
                }
                await _countryService.Add(countryInfoVM, userId);
                return Ok( ResponseResult.CreateResponse("Success", $"Tạo Country {countryInfoVM.CountryName} thành công."));
            }
            catch
            (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseResult.CreateResponse("Error Server", $"Đã có lỗi xảy ra từ máy chủ. {ex.Message}"));
            }
        }

        [HttpGet("GetCountries")]
        public async Task<IActionResult> Get(int? page)
        {
            try
            {
                var countries = await _countryService.GetListAsync();
                int pagesize = 10;
                int maxpage = (countries.Count / pagesize) + (countries.Count % 10 == 0 ? 0 : 1);
                int pagenumber = page == null || page < 0 ? 1 : page.Value;
                PagedList<Country> lst = new(countries, pagenumber, pagesize);
                var countrylst = lst.Select(country => new CountryInfoVM
                {
                    CountryId = country.Id,
                    CountryName = country.CountryName!,
                }).ToList();

                return StatusCode(StatusCodes.Status200OK, ResponseResult.CreateResponse("Success", "Đã lấy danh sách thành công.", new { Data = countrylst, MaxPage = maxpage }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Error Server", "Đã có lỗi xảy ra từ máy chủ."));
        }


        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] CountryInfoVM countryInfoVM, [FromQuery] long userId)
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
                await _countryService.Update(countryInfoVM, userId);
                return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Success", $"Cập Nhật Country {countryInfoVM.CountryName} thành công."));
            }
            catch
            (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseResult.CreateResponse("Error Server", $"Đã có lỗi xảy ra từ máy chủ. {ex.Message}"));
            }
        }
    }
}
