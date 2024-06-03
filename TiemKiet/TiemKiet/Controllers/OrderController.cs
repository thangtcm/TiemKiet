using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiemKiet.Helpers;
using TiemKiet.Models;
using TiemKiet.Models.ViewModel;
using TiemKiet.Services;
using TiemKiet.Services.Interface;

namespace TiemKiet.Controllers
{
    [AuthorizeWithMessageAttribute]
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly IBranchService _branchService;
        public OrderController(ILogger<OrderController> logger, IOrderService orderService, IUserService userService, IBranchService branchService)
        {
            _logger = logger;
            _orderService = orderService;
            _userService = userService;
            _branchService = branchService;
        }


        [HttpPost]
        public async Task<IActionResult> Create(int VoucherId = 0, string? Note = "")
        {
            try
            {
                var user = await _userService.GetUser();
                var model = new OrderInfoVM
                {
                    VoucherList = new List<int> { 0, VoucherId}
                };
                var cart = HttpContext.Session.GetObjectFromJson<ICollection<CartItem>>(Constants.UserCart) ?? new List<CartItem>();
                var locationUser = HttpContext.Session.GetObjectFromJson<LocationUser>(Constants.LocationUser) ?? new LocationUser();
                if (locationUser == null)
                {
                    this.AddToastrMessage("Bạn chưa cấp quyền sử dụng GPS tại dịch vụ này.", Enums.ToastrMessageType.Error);
                    return RedirectToAction("Index", "Home");
                }
                if (cart.Count < 0)
                {
                    this.AddToastrMessage("Giỏ hàng của bạn bị rỗng.", Enums.ToastrMessageType.Error);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var barnchId = cart.FirstOrDefault()?.BranchId;
                    if (!barnchId.HasValue)
                    {
                        this.AddToastrMessage("Đã có lỗi xảy ra trong giỏ hàng của bạn(vui lòng chọn lại).", Enums.ToastrMessageType.Error);
                        return RedirectToAction("Index", "Home");
                    }
                    model.BranchId = barnchId.Value;
                    model.CartItems = cart;
                    model.NumberPhone = user!.PhoneNumber;
                    model.Address = locationUser.AddreasUserName;
                    model.UserId = user.Id;
                    model.FullName = user.FullName ?? "";
                    model.LatCustomer = locationUser.Latitude;
                    model.LongCustomer = locationUser.Longitude;
                    model.NoteShip = Note;
                    var totalPrice = cart.Sum(x => x.getTotalPrice());
                    var shipPrice = ((CallBack.ExtractDistanceValue(locationUser.DistanceBranches.FirstOrDefault(x => x.LocationBranch.BranchId == barnchId)!.Distance)) - 2.5) * 5000;
                    var Calucate = await _userService.CaculatePrice(user!.Id, totalPrice, shipPrice, new List<int>() { 0, VoucherId });
                    var discountRank = (totalPrice * CallBack.GetDiscount(user.Score) / 100) ;
                    if (Calucate.Result != null)
                    {
                        model.GrandTotal = Calucate.Result.TotalPrice;
                        model.Discount = Calucate.Result.DiscountPrice;
                        model.ShipTotal = Calucate.Result.ShipPrice;
                        model.DiscountShip = Calucate.Result.DiscountShipPrice;
                        model.DiscountRank = discountRank;
                        model.DiscountEvent = Calucate.Result.DiscountPrice - discountRank;
                    }
                }

                var result = await _orderService.Add(model, 1000000001);
                if (result.IsSuccess)
                {
                    this.AddToastrMessage(result.Message, Enums.ToastrMessageType.Success);
                    return RedirectToAction(nameof(OrderDetail), new { orderId = result.Result });
                }
                this.AddToastrMessage(result.Message, Enums.ToastrMessageType.Error);
                return RedirectToAction("Index", "Cart");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                this.AddToastrMessage("Đã có lỗi xảy ra từ máy chủ", Enums.ToastrMessageType.Error);
            }
            return RedirectToAction("Index", "Cart");
        }
        [AuthorizeWithMessageAttribute]
        public async Task<IActionResult> OrderDetail(int orderId)
        {
            Order model = new ();
            try
            {
                var user = await _userService.GetUser();
                if(user != null)
                {
                    var result = await _orderService.GetByIdAsync(orderId, user.Id, x => x.Include(d => d.OrderDetails!).ThenInclude(p => p.Product!));
                    if (result == null)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    var branch = await _branchService.GetByIdAsync(result.BranchId);
                    if(branch != null)
                    {
                        ViewData["BranchAddress"] = branch.BranchName;
                    }    
                    model = result;
                }    
               
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return View(new OrderInfoVM(model));
        }
    }
}
