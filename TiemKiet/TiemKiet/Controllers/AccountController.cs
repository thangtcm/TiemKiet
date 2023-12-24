using BarcodeStandard;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using TiemKiet.Data;
using TiemKiet.Enums;
using TiemKiet.Helpers;
using TiemKiet.Models;
using TiemKiet.Models.ViewModel;
using TiemKiet.Services.Interface;

namespace TiemKiet.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserService _userService;
        private readonly IUserTokenService _userTokenService;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger, IUserStore<ApplicationUser> userStore, IUserService userService, IUserTokenService userTokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _userStore = userStore;
            _userService = userService;
            _userTokenService = userTokenService;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserInfoVM model)
        {
            try
            {
                Console.WriteLine($"Dữ liệu password {model.Password}");
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, error = ModelState });
                }
                var userResult = await _userManager.Users.SingleOrDefaultAsync(x => x.PhoneNumber == model.NumberPhone);
                if (userResult != null)
                {
                    return Json(new { success = false, error = "Số điện thoại đã tồn tại." });
                }
                var user = new ApplicationUser()
                {
                    FullName = model.FullName,
                    Birthday = model.Birthday is null ? DateTime.Now : CallBack.ConvertStringToDateTime(model.Birthday),
                    Gender = model.Gender ?? Gender.Another,
                };
                await _userStore.SetUserNameAsync(user, model.NumberPhone, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                return Json(new { success = true, error = $"Lỗi " });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult SendOTP([Phone] string phoneNumber)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, error = ModelState });
            }
            
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmOTP([Phone] string phoneNumber, string codeOTP)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, error = ModelState });
            }
            try
            {
                var userResult = await _userManager.Users.SingleOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
                if (userResult == null)
                {
                    return Json(new { success = true, register = true });
                }
                return Json(new { success = true, register = false });
            }
            catch
            (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout(string? returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return View();
            }
        }

        public ActionResult GenerateBarcode(string barcodeData)
        {
            Barcode barcode = new Barcode();
            var barcodeImage = barcode.Encode(BarcodeStandard.Type.Code128, barcodeData, 350, 100);

            MemoryStream ms = new MemoryStream();
            barcodeImage.Encode(SKEncodedImageFormat.Png, 100).SaveTo(ms);

            return File(ms.ToArray(), "image/png");
        }


        [HttpPost]
        public async Task<IActionResult> Login([Phone] string phoneNumber, string password, string? token)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, error = ModelState });
            }
            try
            {
                var userResult = await _userManager.Users.SingleOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
                if (userResult == null)
                    return Json(new { success = false, error = "không tìm thấy người dùng." });
                var result = await _signInManager.PasswordSignInAsync(userResult.UserName, password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    if(!string.IsNullOrEmpty(token))
                    {
                        await _userTokenService.Add(userResult.Id, token);
                    }    
                    return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") });
                }
                return Json(new { success = false, error = "Mật khẩu không đúng." });
            }
            catch(Exception ex)
            {
                return Json(new { success = false, error = ex.Message});
            }
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            try
            {
                var user = await _userService.GetUser();
                if(user == null)
                {
                    ModelState.AddModelError(string.Empty, "Bạn cần đăng nhập.");
                    return View();
                }    
                return View(new UserInfoVM(user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return View();
        }  
        
        public async Task<IActionResult> EditProfile(long? userId, UserInfoVM model)
        {
            try {
                await _userService.UpdateUser(model);
                return View(nameof(Profile));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return View();
        }

        [HttpPost]
        public IActionResult SaveToken(string token)
        {
            Console.WriteLine($"Dữ liệu là : {token}\n\n\n\n");
            return Ok(new { Message = "Token saved successfully" });
        }

        [HttpPost]
        public async Task<IActionResult> SetUserLocation(double latitude, double longitude)
        {
            try
            {
                await _userService.SetUserLocation(latitude, longitude);
                return Ok();
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return NotFound();
        }    
    }
}
