using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TiemKiet.Data;
using TiemKiet.Enums;
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
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly ILogger<AccountController> _logger;
        private readonly IUserTokenService _userTokenService;
        private readonly ITranscationLogService _transcationLogService;
        private readonly IOrderService _orderService;
        public AccountController(IUserService userService, SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager, ILogger<AccountController> logger, IUserStore<ApplicationUser> userStore, 
            IUserTokenService userTokenService, ITranscationLogService transcationLogService, IOrderService orderService)
        {
            _userService = userService;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _userStore = userStore;
            _userTokenService = userTokenService;
            _transcationLogService = transcationLogService;
            _orderService = orderService;
        }

        [HttpPost("SendNotificationUser")]
        public async Task<IActionResult> SendNotificationUser([FromBody] MessageRequest request, long userId)
        {
            try
            {
                var listToken = await _userTokenService.GetListAsync(userId);
                var message = new MulticastMessage()
                {
                    Tokens = listToken.Select(x => x.UserToken).ToList(),
                    Notification = new Notification
                    {
                        Title = request.Title,
                        Body = request.Body,
                    },
                    Apns = new ApnsConfig()
                    {
                        Headers = new Dictionary<string, string>()
                        {
                            { "apns-collapse-id", "solo_changed_administrator"},
                            { "content-available", "1"},
                            { "apns-priority", "10" },
                        },
                        Aps = new Aps()
                        {
                            Sound = "default"
                        }
                    },
                    Android = new AndroidConfig()
                    {
                        Priority = Priority.High,
                        Notification = new AndroidNotification()
                        {
                            DefaultSound = true,
                        }
                    }
                };
                var messaging = FirebaseMessaging.DefaultInstance;
                var result = await messaging.SendEachForMulticastAsync(message);
                return Ok("Gửi thông báo thành công!.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return BadRequest("Đã xảy ra sự cố trong lúc gửi thông báo.");

        }    

        [HttpPost("SendNotification")]
        public async Task<IActionResult> SendNotification([FromBody] MessageRequest request)
        {
            try
            {
                var messagelst = new List<Message>();
                var message = new Message()
                {
                    Token = request.DeviceToken,
                    Notification = new Notification
                    {
                        Title = request.Title,
                        Body = request.Body,
                    },
                };
                messagelst.Add(message);
                var messaging = FirebaseMessaging.DefaultInstance;
                var result = await messaging.SendAllAsync(messagelst);
                return Ok("Gửi thông báo thành công!.");
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return BadRequest("Đã xảy ra sự cố trong lúc gửi thông báo.");
            
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

        [HttpGet("GetHistoryPayment")]
        public async Task<IActionResult> GetHistoryPayment(long userId, string? date)
        {
            try
            {
                if (!ModelState.IsValid)
                    return NotFound(ResponseResult.CreateResponse("Value Not Valid", $"Dữ liệu nhập vào không hợp lệ - {ModelState}."));
                if(await _userService.GetUser(userId) == null) return NotFound(ResponseResult.CreateResponse("User Not Valid", "Không tìm thấy người dùng."));
                DateTime datenow = DateTime.Now;
                if(!String.IsNullOrEmpty(date))
                {
                    datenow = CallBack.ConvertStringToDateTime(date);
                }    
                var logPayment = await _transcationLogService.GetListAsync(userId, datenow);
                var logpaymentlst = logPayment.Select(x => new TransactionLogVM(x)).ToList();
                return Ok(ResponseResult.CreateResponse("Success", "Đã lấy danh sách thành công.", new { Data = logpaymentlst}));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Error Server", "Đã có lỗi xảy ra từ máy chủ."));
        }

        [HttpGet("GetUserTokens")]
        public async Task<IActionResult> GetUserTokens(long userId, int? page)
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

        [HttpPost("RemoveUser")]
        public async Task<IActionResult> RemoveUser(long userId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Value Not Valid", $"Dữ liệu nhập vào không hợp lệ - {ModelState}."));

                var result = await _userService.RemoveUser(userId);

                if (!result.IsSuccess)
                {
                    return NotFound(ResponseResult.CreateResponse("Lỗi", result.Message));
                }
                return Ok(ResponseResult.CreateResponse("Success", result.Message));
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

        [HttpPost("GetPriceDiscount")]
        public async Task<IActionResult> Get(long userId, double totalPrice, double shipPrice, [FromBody] VoucherListRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Value Not Valid", $"Dữ liệu nhập vào không hợp lệ - {ModelState}."));
                var data = await _userService.CaculatePrice(userId, totalPrice, shipPrice, request.VoucherList);
                if(!data.IsSuccess)
                {
                    return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Error", data.Message));
                }    
                return StatusCode(StatusCodes.Status200OK, ResponseResult.CreateResponse("Success", data.Message, data.Result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Error Server", "Đã có lỗi xảy ra từ máy chủ."));
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(UserInfoVM model)
        {
            try
            {
                if (!model.UserId.HasValue) return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Server", "Tài khoản không tồn tại."));
                var user = await _userService.GetUser(model.UserId.Value);
                if (user is null) return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Server", "Tài khoản không tồn tại."));
                var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.PasswordOld, model.Password);
                if (!changePasswordResult.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { Error = changePasswordResult.Errors });
                }
                return StatusCode(StatusCodes.Status200OK, ResponseResult.CreateResponse("Success", "Đổi mật khẩu thành công."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString(), ex);
            }
            return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Server", "Đã có lỗi xảy ra trong quá trình tạo tài khoản (Vui lòng liên hệ đội ngũ Admin để được hỗ trợ)."));
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] UserInfoVM userinfo)
        {
            try
            {
                if (!ModelState.IsValid || !userinfo.UserId.HasValue)
                {
                    return BadRequest(ResponseResult.CreateResponse("Error", $"Lỗi thiếu dữ liệu.", null));
                }

                var result = await _userService.UpdateUser(userinfo);
                if (result)
                {
                    return Ok(ResponseResult.CreateResponse("Success", "Cập nhật thông tin người dùng thành công.", null));
                }
                return NotFound(ResponseResult.CreateResponse("Error", "Cập nhật thông tin người dùng không thành công."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Có lỗi xảy ra khi Login");
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseResult.CreateResponse("ServerError", "Đã có lỗi xảy ra từ máy chủ."));
            }
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

                if(!userResult.IsAction)
                {
                    return NotFound(ResponseResult.CreateResponse("Not Found", "Người dùng không tồn tại."));
                }
                var result = await _signInManager.PasswordSignInAsync(userResult.UserName, userinfo.Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(userResult);
                    var tokens = await _userTokenService.GetListAsync(userResult.Id);
                    var isHasOrder = await _orderService.GetUserPedingOrder(userResult.Id);
                    long orderId = 0;
                    if (isHasOrder != null)
                        orderId = isHasOrder.Id;
                    return Ok(ResponseResult.CreateResponse("Success", "Đăng nhập thành công thành công.", new UserInfoVM(userResult, roles.ToList(), tokens.Select(x => x.UserToken).ToList()) { IsHasOrder = orderId }));
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
                user.IsAction = true;
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
