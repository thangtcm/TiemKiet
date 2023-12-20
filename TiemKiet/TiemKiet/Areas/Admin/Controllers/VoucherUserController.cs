using Google.Apis.Storage.v1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TiemKiet.Helpers;
using TiemKiet.Models;
using TiemKiet.Services.Interface;
using TiemKiet.ViewModel;

namespace TiemKiet.Areas.Admin.Controllers
{
    [Authorize]
    [Authorize(Policy = Constants.Policies.RequireStaff)]
    [Area("Admin")]
    public class VoucherUserController : Controller
    {
        private readonly IVoucherUserService _voucherUserService;
        private readonly ILogger<VoucherUserController> _logger;
        private readonly IUserService _userService;
        private readonly IVoucherService _voucherService;
        public VoucherUserController(IVoucherUserService voucherUserService, 
            IUserService userService, 
            ILogger<VoucherUserController> logger,
            IVoucherService voucherService)
        {
            _userService= userService;
            _voucherUserService= voucherUserService;
            _logger= logger;
            _voucherService= voucherService;
        }    
        public async Task<IActionResult> Index(long userId = 1)
        {
            ICollection<VoucherUserInfoVM> voucherUsers = new List<VoucherUserInfoVM>();
            var userlst = await _userService.GetUsers();
            ViewData["UserList"] = new SelectList(userlst, "Id", "FullName", userId);
            try
            {
                var voucherUser = await _voucherUserService.GetListAsync(userId, x => x.Include(x => x.Voucher!).Include(x => x.UserClaim!));
                voucherUsers = voucherUser is not null ? voucherUser.Select(x => new VoucherUserInfoVM(x)).ToList() : new List<VoucherUserInfoVM>();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return View(voucherUsers);
        }

        public async Task<IActionResult> Create()
        {
            var users = await _userService.GetUsers();
            ViewData["UserList"] = new SelectList(users, "Id", "NameAndNumberPhone");
            var vouchers = await _voucherService.GetListAsync();
            ViewData["VoucherList"] = new SelectList(vouchers, "Id", "VoucherName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VoucherUser model)
        {
            try
            {
                await _voucherUserService.Add(model);
                return RedirectToAction(nameof(Index));
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            var users = await _userService.GetUsers();
            ViewData["UserList"] = new SelectList(users, "Id", "FullName");
            var vouchers = await _voucherService.GetListAsync();
            ViewData["VoucherList"] = new SelectList(vouchers, "Id", "VoucherName");
            return View(model);
        }    
    }
}
