using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TiemKiet.Helpers;
using TiemKiet.Models;
using TiemKiet.Models.ViewModel;
using TiemKiet.Services.Interface;

namespace TiemKiet.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger<CartController> _logger;
        private readonly IUserService _userService;
        private readonly IVoucherUserService _voucherUserService;
        public CartController(IProductService productService, ILogger<CartController> logger, IUserService userService, IVoucherUserService voucherUserService)
        {
            _productService = productService;
            _logger = logger;
            _userService = userService;
            _voucherUserService = voucherUserService;
        }
        [AuthorizeWithMessageAttribute]
        public async Task<IActionResult> Index(string? VoucherCode = null)
        {
            var model = new OrderInfoVM
            {
                VoucherList = new List<int> { 0 }
            };
            try
            {
                var user = await _userService.GetUser();
                VoucherUser? voucherUser = new();
                Console.WriteLine(VoucherCode + "\n\n\n\n\n");

                if (!string.IsNullOrEmpty(VoucherCode))
                {
                    voucherUser = (await _voucherUserService.GetListAsync(user!.Id, x => x.Include(v => v.Voucher!))).FirstOrDefault(x => x.Voucher!.Code.Contains(VoucherCode));  
                }
                var voucherId = voucherUser is null ? 0 : voucherUser.VoucherId ?? 0;
                model.VoucherList.Add(voucherId);
                var cart = HttpContext.Session.GetObjectFromJson<ICollection<CartItem>>(Constants.UserCart) ?? new List<CartItem>();
                var locationUser = HttpContext.Session.GetObjectFromJson<LocationUser>(Constants.LocationUser) ?? new LocationUser();
                if(locationUser == null)
                {
                    this.AddToastrMessage("Bạn chưa cấp quyền sử dụng GPS tại dịch vụ này.", Enums.ToastrMessageType.Error);
                    return RedirectToAction("Index", "Home");
                }
                if(cart.Count < 0)
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
                    var shipPrice = ((CallBack.ExtractDistanceValue(locationUser.DistanceBranches.FirstOrDefault(x => x.LocationBranch.BranchId == barnchId)!.Distance)) - 2.5) * 5000;
                    var Total = await _userService.CaculatePrice(user!.Id, cart.Sum(x => x.getTotalPrice()), shipPrice, new List<int>() { 0, voucherId });
                    Console.WriteLine(JsonConvert.SerializeObject(Total));
                    if (Total.Result != null)
                    {
                        model.GrandTotal = Total.Result.TotalPrice;
                        model.Discount = Total.Result.DiscountPrice;
                        model.ShipTotal = Total.Result.ShipPrice;
                        model.DiscountShip = Total.Result.DiscountShipPrice;
                    }
                }
                if (!String.IsNullOrEmpty(VoucherCode) && voucherUser == null)
                {
                    this.AddToastrMessage("Mã voucher không tồn tại.", Enums.ToastrMessageType.Error);
                }
                if (!String.IsNullOrEmpty(VoucherCode) && voucherUser != null)
                {
                    this.AddToastrMessage("Áp dụng mã voucher thành công.", Enums.ToastrMessageType.Success);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                this.AddToastrMessage("Đã có lỗi xảy ra.", Enums.ToastrMessageType.Error);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity, bool upsize, bool addIce, int branchId)
        {
            try
            {
                var product = await _productService.GetByIdAsync(productId);

                if (product == null)
                {
                    return NotFound();
                }

                var cart = HttpContext.Session.GetObjectFromJson<ICollection<CartItem>>(Constants.UserCart) ?? new List<CartItem>();

                // Kiểm tra nếu chi nhánh hiện tại của giỏ hàng không khớp với chi nhánh mới được chọn
                if (cart.Any() && cart.First().BranchId != branchId)
                {
                    // Làm mới giỏ hàng cho chi nhánh mới
                    cart.Clear();
                }

                var existingItem = cart.FirstOrDefault(item => item.IsSameProduct(product, upsize, addIce));

                if (existingItem != null)
                {
                    existingItem.ProductQuantity += quantity;
                }
                else
                {
                    var newItem = new CartItem(product)
                    {
                        ProductQuantity = quantity,
                        ProductUpsize = upsize,
                        ProductHasIce = addIce,
                        BranchId = branchId  // Gán chi nhánh mới cho sản phẩm mới thêm vào giỏ hàng
                    };

                    cart.Add(newItem);
                }

                HttpContext.Session.SetObjectAsJson(Constants.UserCart, cart);
                var cartItemCount = cart.Count;
                return Ok(cartItemCount);
            }
            catch (Exception ex)
            {
                var errorMessage = "An error occurred while processing the request.";
                _logger.LogError(ex, errorMessage);
            }
            return NotFound();
        }


        [HttpPost]
        public IActionResult RemoveFromCart(int productId, bool upsize, bool addIce)
        {
            try
            {
                var cart = HttpContext.Session.GetObjectFromJson<ICollection<CartItem>>(Constants.UserCart) ?? new List<CartItem>();
                var itemToRemove = cart.FirstOrDefault(item => item.ProductId == productId && item.ProductUpsize == upsize && item.ProductHasIce == addIce);

                if (itemToRemove != null)
                {
                    cart.Remove(itemToRemove);
                    HttpContext.Session.SetObjectAsJson(Constants.UserCart, cart);
                    
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                Console.WriteLine(ex.Message);
                this.AddToastrMessage($"Đã có lỗi xảy ra.", Enums.ToastrMessageType.Error);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity, bool upsize, bool addIce)
        {
            try
            {
                var cart = HttpContext.Session.GetObjectFromJson<ICollection<CartItem>>(Constants.UserCart) ?? new List<CartItem>();
                var itemToUpdate = cart.FirstOrDefault(item => item.ProductId == productId && item.ProductUpsize == upsize && item.ProductHasIce == addIce);

                if (itemToUpdate != null)
                {
                    if (quantity <= 0)
                    {
                        cart.Remove(itemToUpdate);
                        HttpContext.Session.SetObjectAsJson(Constants.UserCart, cart);
                    }
                    else
                    {
                        itemToUpdate.ProductQuantity = quantity;
                        HttpContext.Session.SetObjectAsJson(Constants.UserCart, cart);
                    }
                    return Ok(itemToUpdate.getTotalPrice().ToString("C0", System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN")));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult GetCartItemCount()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ICollection<CartItem>>(Constants.UserCart) ?? new List<CartItem>();
            var totalQuantity = cart.Count;
            return Json(totalQuantity);
        }

        public IActionResult CartList()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ICollection<CartItem>>(Constants.UserCart) ?? new List<CartItem>();
            return PartialView("_PreviewCartPartialView", cart);
        }
    }
}
