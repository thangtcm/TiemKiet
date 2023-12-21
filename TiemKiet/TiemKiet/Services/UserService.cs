using Firebase.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;
using System.Net.WebSockets;
using TiemKiet.Data;
using TiemKiet.Enums;
using TiemKiet.Helpers;
using TiemKiet.Repository.UnitOfWork;
using TiemKiet.Services.Interface;
using TiemKiet.ViewModel;
using X.PagedList;

namespace TiemKiet.Services
{
    public class UserService : IUserService
    {
        public IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITranscationLogService _transcationLogService;
        public UserService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, 
            ITranscationLogService transcationLogService, RoleManager<ApplicationRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _transcationLogService = transcationLogService;
            _roleManager = roleManager;
        }

        public async Task<ApplicationUser?> GetUser()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User);
            return user;
        }

        public async Task<ICollection<ApplicationUser>> GetUsers()
            => await _unitOfWork.UserRepository.GetAllAsync();

        public async Task<ResponseListVM<UserInfoVM>> GetUsersWithRoles(int page = 1)
        {
           
            var users = await _userManager.Users.AsNoTracking().ToListAsync();
            var userroles = await _unitOfWork.UserRoleRepository.GetAllAsync();
            var roles = await _roleManager.Roles.AsNoTracking().ToListAsync();
            int pagesize = 10;
            int totalUsers = users.Count;
            int maxpage = (totalUsers / pagesize) + (totalUsers % 10 == 0 ? 0 : 1);
            int pagenumber = page < 0 ? 1 : page;
            PagedList<ApplicationUser> lst = new(users, pagenumber, pagesize);
            var userWithRoles = new List<UserInfoVM>();
            foreach (var user in lst)
            {
                var userRoles = userroles.Where(x => x.UserId == user.Id).Select(x => x.RoleId);
                var matchingRoles = roles.Where(r => userRoles.Contains(r.Id)).Select(r => r.Name).ToList();
                userWithRoles.Add(new UserInfoVM(user, matchingRoles));
            }
            var data = new ResponseListVM<UserInfoVM>()
            {
                Data = userWithRoles.ToList(),
                MaxPage = maxpage
            };
            return data;
        }
        public async Task<ICollection<UserInfoVM>> GetUsersWithRoles()
        {

            var users = await _userManager.Users.AsNoTracking().ToListAsync();
            var userroles = await _unitOfWork.UserRoleRepository.GetAllAsync();
            var roles = await _roleManager.Roles.AsNoTracking().ToListAsync();
            var userWithRoles = new List<UserInfoVM>();
            foreach (var user in users)
            {
                var userRoles = userroles.Where(x => x.UserId == user.Id).Select(x => x.RoleId);
                var matchingRoles = roles.Where(r => userRoles.Contains(r.Id)).Select(r => r.Name).ToList();
                userWithRoles.Add(new UserInfoVM(user, matchingRoles));
            }
            return userWithRoles.ToList();
        }
        public async Task<ApplicationUser?> GetUser(long userId)
            => await _unitOfWork.UserRepository.GetAsync(x => x.Id == userId);

        public async Task<bool> UpdateUser(UserInfoVM user)
        {
            if (user.UserId < 1000000000) return false;
            var userModel = await _unitOfWork.UserRepository.GetAsync(x => x.Id == user.UserId!);
            if (userModel is null) return false;
            userModel.FullName = user.FullName;
            userModel.Gender = user.Gender ?? Gender.Another;
            _unitOfWork.UserRepository.Update(userModel);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<StatusResponse<CaculateVoucherInfo>> CaculatePrice(long userId, double TotalPrice, double ShipPrice, List<int> VoucherList)
        {
            var user = await GetUser(userId);
            StatusResponse<CaculateVoucherInfo> data = new();
            if (user == null)
            {
                data.IsSuccess = false;
                data.Message = "{ERROR} Người dùng không hợp lệ.";
                return data;
            }
            double discount = 0, shipdiscount = 0;
            List<double> DiscountTotal = new();
            if (VoucherList.Any(item => item == 0))
            {
                discount += CallBack.GetDiscount(user.Score);
                VoucherList.RemoveAll(item => item == 0);
            }
            var vouchers = await _unitOfWork.VoucherRepository.GetAllAsync(x => VoucherList.Contains(x.Id));
            if (vouchers.Count(x => x.VoucherType == VoucherType.VoucherShip) > 1 || vouchers.Count(x => x.VoucherType == VoucherType.VoucherProduct) > 1)
            {
                data.IsSuccess = false;
                data.Message = "{ERROR} Mỗi loại voucher chỉ được phép chọn 1 voucher.";
                return data;
            }    
            foreach (var item in vouchers)
            {
                if(item.MinBillAmount <= TotalPrice && item.VoucherType != VoucherType.VoucherRank)
                {
                    if (item.DiscountType == DiscountType.Percentage)
                    {
                        if(item.VoucherType == VoucherType.VoucherShip)
                        {
                            shipdiscount = ((ShipPrice * item.DiscountValue / 100) > item.MaxDiscountAmount ? item.MaxDiscountAmount : (ShipPrice * item.DiscountValue / 100));
                        }
                        else
                        {
                            DiscountTotal.Add(((TotalPrice * item.DiscountValue / 100) > item.MaxDiscountAmount ? item.MaxDiscountAmount : (TotalPrice * item.DiscountValue / 100)));
                            Console.WriteLine($"Giảm giá là {DiscountTotal.Sum()} - Min {item.MaxDiscountAmount} -- Giá {TotalPrice} -- Giảm {item.DiscountValue}");
                        }
                    }
                    else
                    {
                        if (item.VoucherType == VoucherType.VoucherShip)
                        {
                            shipdiscount = item.DiscountValue;
                        }
                        else
                        {
                            DiscountTotal.Add(item.DiscountValue);
                        }
                    }
                }    
            }
            CaculateVoucherInfo model = new()
            {
                DiscountPrice = (TotalPrice * discount / 100) + DiscountTotal.Sum(),
                CurrentPrice = TotalPrice,
                UserId = userId,
                VoucherList = VoucherList,
                ShipPrice = (ShipPrice - shipdiscount) < 0 ? 0 : (ShipPrice - shipdiscount)
            };
            Console.WriteLine($"KQ : {(TotalPrice * discount / 100)} -- {DiscountTotal.Sum()}");
            model.TotalPrice = TotalPrice - model.DiscountPrice;
            data.IsSuccess= true;
            data.Result = model;
            data.Message = "Tính toán thành công.";
            return data;
        }

        public async Task<ApplicationUser?> GetUserwithPhone([Phone] string Phone)
            => await _unitOfWork.UserRepository.GetAsync(x => x.PhoneNumber == Phone);
            
        public async Task UpdatePoint(CaculateVoucherInfo model, long userId)
        {
            var user = await GetUser(model.UserId);
            if(user != null)
            {
                double RecivePoint = model.TotalPrice / 1000.0;
                TransactionLogVM _logPayment = new()
                {
                    PointOld = user.Point,
                    TotalPrice = model.TotalPrice,
                    ScroreOld = user.Score,
                    ScroreNew = user.Score + RecivePoint,
                    PointNew = user.Score + RecivePoint,
                    DiscountPrice = model.DiscountPrice
                };
                user.Point += RecivePoint;
                user.Score += RecivePoint;
                await _transcationLogService.Add(_logPayment, model.UserId, userId);
                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.CommitAsync();
            }    
        }
    }
}
