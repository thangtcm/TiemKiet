using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TiemKiet.Enums;
using TiemKiet.Helpers;
using TiemKiet.Models;
using TiemKiet.Models.ViewModel;
using TiemKiet.Services.Interface;

namespace TiemKietAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherUserController : ControllerBase
    {
        private readonly IVoucherUserService _voucherUserService;
        private readonly ILogger<VoucherUserController> _logger;
        private readonly IUserService _userService;
        public VoucherUserController(IVoucherUserService voucherUserService, ILogger<VoucherUserController> logger, IUserService userService)
        {
            _voucherUserService = voucherUserService;
            _logger = logger;
            _userService = userService;
        }

        [HttpGet("GetVoucherUser")]
        public async Task<IActionResult> Get([Phone] string numberPhone)
        {
            try
            {
                var user = await _userService.GetUserwithPhone(numberPhone);
                if(user == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Not Found User", $"Không tìm thấy người dùng."));
                }    
                var voucherUser = await _voucherUserService.GetListAsync(user.Id, x => x.Include(v => v.Voucher!));
                var voucherUserInfo = voucherUser.Select(x => new VoucherUserInfoVM(x)).ToList();
                ICollection<VoucherUserInfoVM> voucherShip = new List<VoucherUserInfoVM>();
                ICollection<VoucherUserInfoVM> voucherRank = new List<VoucherUserInfoVM>();
                ICollection<VoucherUserInfoVM> voucherProduct = new List<VoucherUserInfoVM>();
                foreach (VoucherType e in Enum.GetValues(typeof(VoucherType)))
                {
                    var lst = voucherUserInfo.Where(x => x.Voucher.VoucherType == e).ToList();
                    switch(e)
                    {
                        case VoucherType.VoucherShip:
                        {
                            voucherShip = lst;
                            break;
                        }
                        case VoucherType.VoucherProduct:
                        {
                            voucherProduct = lst;
                            break;
                        }
                    }    
                }
                Voucher dataRank = new()
                {
                    VoucherName = $"Voucher hạng {CallBack.GetRankName(user.Score)}",
                    DiscountType = DiscountType.Percentage,
                    VoucherType = VoucherType.VoucherRank,
                    DiscountValue = CallBack.GetDiscount(user.Score)
                };
                var modelRank = new VoucherUserInfoVM()
                {
                    Voucher = new VoucherInfoVM(dataRank)
                };
                if(modelRank.Voucher.DiscountValue != 0)
                {
                    voucherRank.Add(modelRank);
                }
                return StatusCode(StatusCodes.Status200OK, ResponseResult.CreateResponse("Success", "Đã lấy danh sách thành công.", new { voucherRank = voucherRank, voucherShip = voucherShip, voucherProduct = voucherProduct }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Error Server", "Đã có lỗi xảy ra từ máy chủ."));
        }

        [HttpPost("ReceiveVoucherUser")]
        public async Task<IActionResult> ReceiveVoucherUser(string voucherCode,long userId)
        {
            try
            {
                var user = await _userService.GetUser(userId);
                if (user == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Not Found User", $"Không tìm thấy người dùng."));
                }
                var result = await _voucherUserService.ReceiveVoucher(voucherCode, userId);
                if(result.IsSuccess)
                {
                    return StatusCode(StatusCodes.Status200OK, ResponseResult.CreateResponse("Success", result.Message));
                }
                return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Lỗi", result.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
            }
            return StatusCode(StatusCodes.Status404NotFound, ResponseResult.CreateResponse("Error Server", "Đã có lỗi xảy ra từ máy chủ."));
        }
    }
}
