using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiemKiet.Helpers;
using TiemKiet.Models;
using TiemKiet.Services.Interface;
using TiemKiet.Models.ViewModel;
using X.PagedList;

namespace TiemKietAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherService _voucherService;
        private readonly ILogger<VoucherController> _logger;
        public VoucherController(IVoucherService voucherService, ILogger<VoucherController> logger)
        {
            _voucherService = voucherService;
            _logger = logger;
        }

        [HttpGet("GetVouchers")]
        public async Task<IActionResult> Get(int? page)
        {
            try
            {
                var vouchers = await _voucherService.GetListAsync();
                int pagesize = 10;
                int maxpage = (vouchers.Count / pagesize) + (vouchers.Count % 10 == 0 ? 0 : 1);
                int pagenumber = page == null || page < 0 ? 1 : page.Value;
                PagedList<Voucher> lst = new(vouchers, pagenumber, pagesize);
                var voucherlst = lst.Select(voucher => new VoucherInfoVM(voucher)
                {
                }).ToList();

                return StatusCode(StatusCodes.Status200OK, ResponseResult.CreateResponse("Success", "Đã lấy danh sách thành công.", new { Data = voucherlst, MaxPage = maxpage }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return StatusCode(StatusCodes.Status500InternalServerError, ResponseResult.CreateResponse("Error Server", "Đã có lỗi xảy ra từ máy chủ."));
        }
    }
}
