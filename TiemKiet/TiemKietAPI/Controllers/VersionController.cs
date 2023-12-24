using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiemKiet.Helpers;
using TiemKiet.Models;
using TiemKiet.Services;
using TiemKiet.Services.Interface;
using TiemKiet.Models.ViewModel;
using X.PagedList;

namespace TiemKietAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        private readonly IVersionService _versionService;
        private readonly ILogger<VersionController> _logger;
        public VersionController(IVersionService versionService, ILogger<VersionController> logger)
        {
            _versionService = versionService;
            _logger = logger;
        }

        [HttpGet("GetVersion")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var version = await _versionService.GetByIdAsync();
                return StatusCode(StatusCodes.Status200OK, ResponseResult.CreateResponse("Success", "Đã lấy danh sách thành công.", new { Data = version}));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Error Server", "Đã có lỗi xảy ra từ máy chủ."));
        }
    }
}
