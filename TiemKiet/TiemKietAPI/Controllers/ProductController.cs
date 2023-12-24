using Google.Api.Gax;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
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
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly ILogger<ProductController> _logger;
        public ProductController(IProductService productService, IUserService userService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm] ProductInfoVM productInfoVM, [FromQuery] long userId, [FromQuery] int branchId)
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
                await _productService.Add(productInfoVM, userId, branchId);
                return Ok(ResponseResult.CreateResponse("Success", $"Tạo sản phẩm {productInfoVM.ProductName} thành công."));
            }
            catch
            (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseResult.CreateResponse("Error Server", $"Đã có lỗi xảy ra từ máy chủ. {ex.Message}"));
            }
        }

        [HttpGet("GetCategory")]
        public IActionResult GetCategory()
        {
            try
            {
                var enumData = from ProductType e in Enum.GetValues(typeof(ProductType))
                select new
                {
                    ID = (int)e,
                    Name = e.ToString()
                };

                return StatusCode(StatusCodes.Status200OK, ResponseResult.CreateResponse("Success", "Đã lấy danh sách thành công.", new SelectList(enumData, "ID", "Name")));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Error Server", "Đã có lỗi xảy ra từ máy chủ."));
            
        }

        [HttpGet("GetProducts")]
        public async Task<IActionResult> Get(int branchId = 1, ProductType productType = 0)
        {
            try
            {
                var products = await _productService.GetListAsync(branchId, productType);
                var productlst = products.Select(product => new ProductInfoVM(product)
                {
                    ProductDescription = product.ProductMBDescription ?? "",
                }).ToList();

                return StatusCode(StatusCodes.Status200OK, ResponseResult.CreateResponse("Success", "Đã lấy danh sách thành công.", productlst));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Error Server", "Đã có lỗi xảy ra từ máy chủ."));
        }

    }
}
