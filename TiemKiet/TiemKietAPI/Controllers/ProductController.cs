using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiemKiet.Helpers;
using TiemKiet.Models;
using TiemKiet.Services;
using TiemKiet.Services.Interface;
using TiemKiet.ViewModel;
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

        [HttpGet("GetProducts")]
        public async Task<IActionResult> Get(int? page, int? branchId)
        {
            try
            {
                var products = await _productService.GetListAsync(branchId, x => x.Include(x => x.ProductImg!));
                int pagesize = 10;
                int maxpage = (products.Count / pagesize) + (products.Count % 10 == 0 ? 0 : 1);
                int pagenumber = page == null || page < 0 ? 1 : page.Value;
                PagedList<Product> lst = new(products, pagenumber, pagesize);
                var productlst = lst.Select(product => new ProductInfoVM(product)).ToList();

                return StatusCode(StatusCodes.Status200OK, ResponseResult.CreateResponse("Success", "Đã lấy danh sách thành công.", new { Data = productlst, MaxPage = maxpage }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Error Server", "Đã có lỗi xảy ra từ máy chủ."));
        }

    }
}
