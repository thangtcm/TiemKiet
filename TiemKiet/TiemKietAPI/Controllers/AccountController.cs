using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TiemKiet.Data;
using TiemKiet.Helpers;
using TiemKiet.Services.Interface;
using TiemKiet.ViewModel;

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
