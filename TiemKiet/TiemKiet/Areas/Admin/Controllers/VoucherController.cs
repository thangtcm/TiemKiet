using Google.Apis.Storage.v1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TiemKiet.Enums;
using TiemKiet.Helpers;
using TiemKiet.Models;
using TiemKiet.Services.Interface;
using TiemKiet.Models.ViewModel;

namespace TiemKiet.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = Constants.Policies.RequireAdmin)]
    public class VoucherController : Controller
    {
        private readonly IVoucherService _voucherService;
        private readonly IUserService _userService;
        private readonly ILogger<VoucherController> _logger;
        public VoucherController(ILogger<VoucherController> logger, IVoucherService voucherService, IUserService userService)
        {
            _logger = logger;
            _voucherService = voucherService;
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            var voucheres = await _voucherService.GetListAsync();
            return View(voucheres.Select(x => new VoucherInfoVM(x)).ToList());
        }

        public IActionResult Create()
        {
            var enumData = from DiscountType e in Enum.GetValues(typeof(DiscountType))
                           select new
                           {
                               ID = (int)e,
                               Name = e.ToString()
                           };
            ViewData["DiscountTypeLst"] = new SelectList(enumData, "ID", "Name");
            enumData = from VoucherType e in Enum.GetValues(typeof(VoucherType))
                       select new
                       {
                           ID = (int)e,
                           Name = e.ToString()
                       };
            ViewData["VoucherTypeLst"] = new SelectList(enumData, "ID", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Voucher model)
        {
            try
            {
                var user = await _userService.GetUser();
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Bạn cần đăng nhập.");
                    return RedirectToAction(nameof(Index));
                }
                await _voucherService.Add(model, user.Id);
                this.AddToastrMessage("Tạo voucher thành công", Enums.ToastrMessageType.Success);
                return RedirectToAction(nameof(Index));
            }
            catch
            (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                var enumData = from DiscountType e in Enum.GetValues(typeof(DiscountType))
                               select new
                               {
                                   ID = (int)e,
                                   Name = e.ToString()
                               };
                ViewData["DiscountTypeLst"] = new SelectList(enumData, "ID", "Name");
                enumData = from VoucherType e in Enum.GetValues(typeof(VoucherType))
                           select new
                           {
                               ID = (int)e,
                               Name = e.ToString()
                           };
                ViewData["VoucherTypeLst"] = new SelectList(enumData, "ID", "Name");
            }

            return View();
        }
    }
}
