using Microsoft.AspNetCore.Mvc;
using TiemKiet.Helpers;
using TiemKiet.Models;
using TiemKiet.Services.Interface;

namespace TiemKiet.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private ILogger<CartController> _logger;
        public CartController(IProductService productService, ILogger<CartController> logger)
        {
            _productService = productService;
            _logger = logger;
        }
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ICollection<CartItem>>(Constants.UserCart) ?? new List<CartItem>();
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity, bool upsize, bool addIce)
        {
            try
            {
                var product = await _productService.GetByIdAsync(productId);

                if (product == null)
                {
                    return NotFound();
                }

                var cart = HttpContext.Session.GetObjectFromJson<ICollection<CartItem>>(Constants.UserCart) ?? new List<CartItem>();
                var existingItem = cart.FirstOrDefault(item => item.IsSameProduct(product, upsize, addIce));

                if (existingItem != null)
                {
                    existingItem.Quantity += quantity;
                }
                else
                {
                    var newItem = new CartItem
                    {
                        Product = product,
                        Quantity = quantity,
                        Upsize = upsize,
                        AddIce = addIce
                    };

                    cart.Add(newItem);
                }
                HttpContext.Session.SetObjectAsJson(Constants.UserCart, cart);
                var cartItemCount = cart.Sum(item => item.Quantity);
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
                var itemToRemove = cart.FirstOrDefault(item => item.Product.Id == productId && item.Upsize == upsize && item.AddIce == addIce);

                if (itemToRemove != null)
                {
                    cart.Remove(itemToRemove);
                    HttpContext.Session.SetObjectAsJson(Constants.UserCart, cart);
                    this.AddToastrMessage($"Bạn đã xóa {itemToRemove.Product.ProductName} ra khỏi giỏ hàng.", Enums.ToastrMessageType.Success);
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
                var itemToUpdate = cart.FirstOrDefault(item => item.Product.Id == productId && item.Upsize == upsize && item.AddIce == addIce);

                if (itemToUpdate != null)
                {
                    itemToUpdate.Quantity = quantity;
                    HttpContext.Session.SetObjectAsJson(Constants.UserCart, cart);
                    this.AddToastrMessage($"Bạn đã cập nhật số lượng {itemToUpdate.Product.ProductName} thành công.", Enums.ToastrMessageType.Success);
                }
                return Ok();
            }catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                this.AddToastrMessage($"Đã có lỗi xảy ra.", Enums.ToastrMessageType.Error);
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult GetCartItemCount()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ICollection<CartItem>>(Constants.UserCart) ?? new List<CartItem>();
            var totalQuantity = cart.Sum(item => item.Quantity);
            return Json(totalQuantity);
        }

        public IActionResult CartList()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ICollection<CartItem>>(Constants.UserCart) ?? new List<CartItem>();
            return PartialView("_PreviewCartPartialView", cart);
        }
    }
}
