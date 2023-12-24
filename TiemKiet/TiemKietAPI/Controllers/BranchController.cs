using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiemKiet.Helpers;
using TiemKiet.Models;
using TiemKiet.Models.ViewModel;
using TiemKiet.Services;
using TiemKiet.Services.Interface;
using X.PagedList;

namespace TiemKietAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _branchService;
        private readonly ILogger<BranchController> _logger;
        private readonly IUserService _userService;
        public BranchController(IBranchService branchService, ILogger<BranchController> logger, IUserService userService)
        {
            _branchService = branchService;
            _logger = logger;
            _userService = userService;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm] BranchInfoVM branchInfoVM, [FromQuery] long userId, [FromQuery] int districtId)
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
                await _branchService.Add(branchInfoVM, userId, districtId);
                return Ok(ResponseResult.CreateResponse("Success", $"Tạo Chi nhánh {branchInfoVM.BranchName} thành công."));
            }
            catch
            (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseResult.CreateResponse("Error Server", $"Đã có lỗi xảy ra từ máy chủ. {ex.Message}"));
            }
        }


        [HttpGet("GetBranches")]
        public async Task<IActionResult> Get(int? page, int? districtId, int? provinceId)
        {
            ICollection<Branch> branches = new List<Branch>();
            try
            {
                if (!districtId.HasValue && !provinceId.HasValue)
                    return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Value Not Valid", $"Dữ liệu districtId hoặc provinceId không được để trống."));
                branches = await _branchService.GetListAsync(districtId.HasValue ? districtId.Value : null, provinceId.HasValue ? provinceId.Value : null);
                int pagesize = 10;
                int maxpage = (branches.Count / pagesize) + (branches.Count % 10 == 0 ? 0 : 1);
                int pagenumber = page == null || page < 0 ? 1 : page.Value;
                PagedList<Branch> lst = new(branches, pagenumber, pagesize);
                var branchelst = lst.Select(branch => new BranchInfoVM(branch)).ToList();
                return StatusCode(StatusCodes.Status200OK, ResponseResult.CreateResponse("Success", "Đã lấy danh sách thành công.", new { Data = branchelst, MaxPage = maxpage }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Error Server", "Đã có lỗi xảy ra từ máy chủ."));
        }

    }
}
