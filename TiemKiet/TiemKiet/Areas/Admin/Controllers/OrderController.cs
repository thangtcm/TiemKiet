using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TiemKiet.Helpers;
using TiemKiet.Models;
using TiemKiet.Services.Interface;

namespace TiemKiet.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [Authorize(Policy = Constants.Policies.RequireAdmin)]
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        public OrderController(ILogger<OrderController> logger, IOrderService orderService, IUserService userService)
        {
            _logger = logger;
            _orderService = orderService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            ICollection<Order> model = new List<Order>();
            try
            {
                model = await _orderService.GetListAsync();
                this.AddToastrMessage("Tải dữ liệu thành công", Enums.ToastrMessageType.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return View(model);
        }

        public async Task<IActionResult> Details(int orderId)
        {
            try
            {
                var model = await _orderService.GetByIdAsync(orderId);
                if(model == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                return View(model);
            }
            catch
            {

            }
            return View();
        }    

    }
}
