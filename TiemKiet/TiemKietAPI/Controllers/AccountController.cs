using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TiemKiet.Data;
using TiemKiet.Helpers;
using TiemKiet.Models;
using TiemKiet.Services;
using TiemKiet.Services.Interface;
using TiemKiet.ViewModel;
using X.PagedList;

namespace TiemKietAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        public AccountController(IUserService userService, SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager, ILogger<AccountController> logger)
        {
            _userService = userService;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> Get(int page = 1)
        {
            try
            {
                var data = await _userService.GetUsersWithRoles(page);
                return StatusCode(StatusCodes.Status200OK, ResponseResult.CreateResponse("Success", "Đã lấy danh sách thành công.", new { Data = data.Data, MaxPage = data.MaxPage }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Error Server", "Đã có lỗi xảy ra từ máy chủ."));
        }



        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserInfoVM userinfo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userResult = await _userManager.FindByEmailAsync(userinfo.UserName) ?? await _userManager.FindByNameAsync(userinfo.UserName);

                if (userResult == null)
                {
                    return NotFound(ResponseResult.CreateResponse("DataNull", "UserName or Email không tồn tại."));
                }

                var result = await _signInManager.PasswordSignInAsync(userResult.UserName, userinfo.Password, false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return Ok(ResponseResult.CreateResponse("Success", "Đăng nhập thành công thành công.", new UserInfoVM(userResult)));
                }

                return NotFound(ResponseResult.CreateResponse("DataNull", "UserName or Password không đúng."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Có lỗi xảy ra khi Login");
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseResult.CreateResponse("ServerError", "Đã có lỗi xảy ra từ máy chủ."));
            }
        }

    }
}
