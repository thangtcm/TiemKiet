using Google.Api.Gax;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiemKiet.Helpers;
using TiemKiet.Models.ViewModel;
using TiemKiet.Models;
using TiemKiet.Services;
using TiemKiet.Services.Interface;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TiemKiet.Enums;
using Firebase.Auth;

namespace TiemKietAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        public OrderController(ILogger<OrderController> logger, 
            IOrderService orderService,
            IUserService userService)
        {
            _logger = logger;
            _orderService = orderService;
            _userService = userService;
        }

        [HttpGet("GetOrder")]
        public async Task<IActionResult> Get(long orderId, long userId)
        {
            try
            {
                var order = await _orderService.GetByIdAsync(orderId, userId, x => x.Include(od => od.OrderDetails!));

                return StatusCode(StatusCodes.Status200OK, ResponseResult.CreateResponse("Success", "Lấy dữ liệu thành công.", order));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return StatusCode(StatusCodes.Status500InternalServerError, ResponseResult.CreateResponse("Error Server", "Đã có lỗi xảy ra từ máy chủ."));
        }

        [HttpGet("GetOrders")]
        public async Task<IActionResult> Get(long userId, int? page)
        {
            try
            {
                var orders = await _orderService.GetListAsync(userId);
                int pagesize = 10;
                int maxpage = (orders.Count / pagesize) + (orders.Count % 10 == 0 ? 0 : 1);
                int pagenumber = page == null || page < 0 ? 1 : page.Value;
                PagedList<Order> lst = new(orders, pagenumber, pagesize);
                var orderlst = lst.Select(x => new OrderInfoVM(x)
                {
                }).ToList();

                return StatusCode(StatusCodes.Status200OK, ResponseResult.CreateResponse("Success", "Lấy dữ liệu thành công.", new { Data = orderlst , MaxPage = maxpage }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                Console.WriteLine(ex.Message.ToString());
            }
            return StatusCode(StatusCodes.Status500InternalServerError, ResponseResult.CreateResponse("Error Server", "Đã có lỗi xảy ra từ máy chủ."));
        }
        [HttpGet("GetPendingOrders")]
        public async Task<IActionResult> GetPendingOrders(int branchId, int? page)
        {
            try
            {
                var orders = await _orderService.GetPendingOrders(branchId);
                int pagesize = 10;
                int maxpage = (orders.Count / pagesize) + (orders.Count % 10 == 0 ? 0 : 1);
                int pagenumber = page == null || page < 0 ? 1 : page.Value;
                PagedList<Order> lst = new(orders, pagenumber, pagesize);
                var orderlst = lst.Select(x => new OrderInfoVM(x)
                {
                }).ToList();

                return StatusCode(StatusCodes.Status200OK, ResponseResult.CreateResponse("Success", "Lấy dữ liệu thành công.", new { Data = orderlst, MaxPage = maxpage }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                Console.WriteLine(ex.Message.ToString());
            }
            return StatusCode(StatusCodes.Status500InternalServerError, ResponseResult.CreateResponse("Error Server", "Đã có lỗi xảy ra từ máy chủ."));
        }

        [HttpPost("AddOrder")]
        public async Task<IActionResult> Add(OrderInfoVM orderInfoVM)
        {
            try
            {
                if (!ModelState.IsValid)
                    return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Value Not Valid", $"Dữ liệu nhập vào không hợp lệ - {ModelState}."));

                var user = await _userService.GetUser(orderInfoVM.UserId);
                if (user == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("User NotFound", $"Người dùng không tồn tại."));
                }
                var result = await _orderService.Add(orderInfoVM, 1000000001);
                if(result.IsSuccess)
                {
                    return Ok(ResponseResult.CreateResponse("Success", result.Message));
                }
                return NotFound(ResponseResult.CreateResponse("Error", result.Message));
            }
            catch
            (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseResult.CreateResponse("Error Server", $"Đã có lỗi xảy ra từ máy chủ. {ex.Message}"));
            };
        }

        [HttpPost("UpdateOrderStatus")]
        public async Task<IActionResult> UpdateOrderStatus(long orderId, OrderStatus orderStatus, long staffId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Value Not Valid", $"Dữ liệu nhập vào không hợp lệ - {ModelState}."));

                var user = await _userService.GetUser(staffId);
                if (user == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("User NotFound", $"Người dùng không tồn tại."));
                }
                await _orderService.UpdateStatus(orderId, orderStatus, staffId);
                return Ok(ResponseResult.CreateResponse("Success", $"Cập nhật trạng thái thành công."));
            }
            catch
            (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseResult.CreateResponse("Error Server", $"Đã có lỗi xảy ra từ máy chủ. {ex.Message}"));
            };
        }
    }
}
