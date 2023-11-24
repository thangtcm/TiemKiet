using Microsoft.AspNetCore.Mvc;
using TiemKiet.Helpers;
using TiemKiet.Models;
using TiemKiet.Services.Interface;
using X.PagedList;

namespace TiemKietAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<ImageController> _logger;
        private readonly IImageService _imageService;
        public ImageController(IUserService userService, ILogger<ImageController> logger, IImageService imageService)
        {
            _userService = userService;
            _logger = logger;
            _imageService = imageService;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(IFormFile upload, long userId)
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
                await _imageService.Add(upload, userId);
                return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Success", $"Tải ảnh lên máy chủ thành công."));
            }
            catch
            (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseResult.CreateResponse("Error Server", $"Đã có lỗi xảy ra từ máy chủ. {ex.Message}"));
            }
        }

        [HttpGet("GetImages")]
        public async Task<IActionResult> Get(int? page)
        {
            try
            {
                var images = await _imageService.GetListAsync();
                int pagesize = 10;
                int maxpage = (images.Count / pagesize) + (images.Count % 10 == 0 ? 0 : 1);
                int pagenumber = page == null || page < 0 ? 1 : page.Value;
                PagedList<ImageModel> lst = new(images, pagenumber, pagesize);

                return StatusCode(StatusCodes.Status200OK, ResponseResult.CreateResponse("Success", "Đã lấy danh sách thành công.", new { Data = lst, MaxPage = maxpage }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Error Server", "Đã có lỗi xảy ra từ máy chủ."));
        }
    }
}
