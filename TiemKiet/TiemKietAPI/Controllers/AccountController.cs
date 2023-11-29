using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TiemKiet.Data;
using TiemKiet.Enums;
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
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly ILogger<AccountController> _logger;
        private readonly IUserTokenService _userTokenService;
        public AccountController(IUserService userService, SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager, ILogger<AccountController> logger, IUserStore<ApplicationUser> userStore, 
            IUserTokenService userTokenService)
        {
            _userService = userService;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _userStore = userStore;
            _userTokenService = userTokenService;
        }

        [HttpPost("AddToken")]
        public async Task<IActionResult> AddToken(long? userId, string token)
        {
            try
            {
                if (!ModelState.IsValid)
                    return NotFound(ResponseResult.CreateResponse("Value Not Valid", $"Dữ liệu đầu vào không hợp lệ - {ModelState}."));
                await _userTokenService.Add(userId, token);
                return Ok(ResponseResult.CreateResponse("Success", "Đã thêm token vào dữ liệu người dùng thành công.", null));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Error Server", "Đã có lỗi xảy ra từ máy chủ."));
        }

        [HttpGet("GetUserTokens")]
        public async Task<IActionResult> GetUserTokens(long? userId, int? page)
        {
            try
            {
                var userTokens = await _userTokenService.GetListAsync(userId);
                int pagesize = 10;
                int maxpage = (userTokens.Count / pagesize) + (userTokens.Count % 10 == 0 ? 0 : 1);
                int pagenumber = page == null || page < 0 ? 1 : page.Value;
                PagedList<ApplicationUserToken> lst = new(userTokens, pagenumber, pagesize);

                return Ok(ResponseResult.CreateResponse("Success", "Đã lấy danh sách thành công.", new { Data = lst, MaxPage = maxpage }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Error Server", "Đã có lỗi xảy ra từ máy chủ."));
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

        [HttpGet("GetUserWithPhone")]
        public async Task<IActionResult> GetUserWithPhone([Phone] string numberPhone)
        {
            try
            {
                if (!ModelState.IsValid)
                    return NotFound(ResponseResult.CreateResponse("Value Not Valid", $"Dữ liệu nhập vào không hợp lệ - {ModelState}."));

                var userResult = await _userManager.Users.SingleOrDefaultAsync(x => x.PhoneNumber == numberPhone);

                if (userResult == null)
                {
                    return NotFound(ResponseResult.CreateResponse("Not Exist", "NumberPhone không tồn tại."));
                }
                var roles = await _userManager.GetRolesAsync(userResult);
                var tokens = await _userTokenService.GetListAsync(userResult.Id);
                return Ok(ResponseResult.CreateResponse("Success", "Tìm thấy dữ liệu tài khoản.", new UserInfoVM(userResult, roles.ToList(), tokens.Select(x => x.UserToken).ToList())));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Error Server", "Đã có lỗi xảy ra từ máy chủ."));
        }    

        [HttpGet("IsNumberPhoneExist")]
        public async Task<IActionResult> IsNumberPhoneExist([Phone] string numberPhone)
        {
            try
            {
                if (!ModelState.IsValid)
                    return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Value Not Valid", $"Dữ liệu nhập vào không hợp lệ - {ModelState}."));

                var userResult = await _userManager.Users.SingleOrDefaultAsync(x => x.PhoneNumber == numberPhone);

                if (userResult == null)
                {
                    return NotFound(ResponseResult.CreateResponse("Not Exist", "NumberPhone không tồn tại."));
                }
                return Ok(ResponseResult.CreateResponse("Success", "NumberPhone tồn tại."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Error Server", "Đã có lỗi xảy ra từ máy chủ."));
        }    

        [HttpGet("GetPriceDiscount")]
        public async Task<IActionResult> Get(long userId, double totalPrice, int voucherId = 0)
        {
            try
            {
                if (!ModelState.IsValid)
                    return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Value Not Valid", $"Dữ liệu nhập vào không hợp lệ - {ModelState}."));
                var data = await _userService.CaculatePrice(userId, totalPrice, voucherId);
                if(data.UserId == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("User Not Found", "Không tìm thấy người dùng."));
                }    
                return StatusCode(StatusCodes.Status200OK, ResponseResult.CreateResponse("Success", "Tính tiền cần thanh toán thành công.", data));
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
                    return BadRequest(ResponseResult.CreateResponse("Error", $"Lỗi {ModelState}.", null));
                }

                var userResult = await _userManager.Users.SingleOrDefaultAsync(x => x.PhoneNumber == userinfo.NumberPhone);

                if (userResult == null)
                {
                    return NotFound(ResponseResult.CreateResponse("DataNull", "NumberPhone không tồn tại."));
                }

                var result = await _signInManager.PasswordSignInAsync(userResult.UserName, userinfo.Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(userResult);
                    var tokens = await _userTokenService.GetListAsync(userResult.Id);
                    return Ok(ResponseResult.CreateResponse("Success", "Đăng nhập thành công thành công.", new UserInfoVM(userResult, roles.ToList(), tokens.Select(x => x.UserToken).ToList())));
                }

                return NotFound(ResponseResult.CreateResponse("DataNull", "UserName or Password không đúng."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Có lỗi xảy ra khi Login");
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseResult.CreateResponse("ServerError", "Đã có lỗi xảy ra từ máy chủ."));
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserInfoVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ResponseResult.CreateResponse("Error", $"Lỗi {ModelState}.", null));
                }
                var userResult = await _userManager.Users.SingleOrDefaultAsync(x => x.PhoneNumber == model.NumberPhone);
                if (userResult != null)
                {
                    return BadRequest(ResponseResult.CreateResponse("NumberPhone Valid", $"Số điện thoại đã tồn tại.", null));
                }
                var user = CreateUser();

                user.FullName = model.FullName;
                user.EmailConfirmed = true;
                user.PhoneNumberConfirmed = true;
                user.PhoneNumber = model.NumberPhone;
                user.Birthday = CallBack.ConvertStringToDateTime(model.Birthday ?? DateTime.Now.ToString("dd/MM/yyyy"));
                user.Gender = model.Gender ?? Gender.Another;
                await _userStore.SetUserNameAsync(user, model.NumberPhone, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return Ok(ResponseResult.CreateResponse("Success", $"Đăng ký tài khoản của {model.FullName} thành công.", null));
                }
                return NotFound(ResponseResult.CreateResponse("Error", $"Đăng ký tài khoản của {model.FullName} không thành công. Lỗi :{result.Errors}", null));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Error", "Đã có lỗi xảy ra trong quá trình tạo tài khoản (Vui lòng liên hệ đội ngũ Admin để được hỗ trợ)."));
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        [HttpPost("Payment")]
        public async Task<IActionResult> Payment([FromBody] CaculateVoucherInfo model, [FromQuery] long userId)
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
                await _userService.UpdatePoint(model, userId);
                return Ok(ResponseResult.CreateResponse("Success", $"Thanh toán và cập nhật điểm của người dùng thành công."));
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
