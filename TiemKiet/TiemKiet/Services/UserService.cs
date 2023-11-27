using Firebase.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using TiemKiet.Data;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITranscationLogService _transcationLogService;
        public UserService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, 
            ITranscationLogService transcationLogService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _transcationLogService = transcationLogService;
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
            int pagesize = 10;
            int totalUsers = users.Count;
            int maxpage = (totalUsers / pagesize) + (totalUsers % 10 == 0 ? 0 : 1);
            int pagenumber = page < 0 ? 1 : page;
            PagedList<ApplicationUser> lst = new(users, pagenumber, pagesize);
            var userWithRoles = new List<UserInfoVM>();
            foreach (var user in users)
            {
                var userRoles = new List<string>(await _userManager.GetRolesAsync(user));
                userWithRoles.Add(new UserInfoVM(user, userRoles));
            }
            var data = new ResponseListVM<UserInfoVM>()
            {
                Data = userWithRoles.ToList(),
                MaxPage = maxpage
            };
            return data;
        }
        public async Task<ApplicationUser?> GetUser(long userId)
            => await _unitOfWork.UserRepository.GetAsync(x => x.Id == userId);

        public async Task<bool> UpdateUser(UserInfoVM user)
        {
            if (user.UserId < 1000000000) return false;
            var userModel = await _unitOfWork.UserRepository.GetAsync(x => x.Id == user.UserId!);
            if (userModel is null) return false;
            userModel.FullName = user.FullName;
            userModel.PhoneNumber = user.NumberPhone;
            _unitOfWork.UserRepository.Update(userModel);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<CaculateVoucherInfo> CaculatePrice(long userId, double TotalPrice, int VoucherId = 0)
        {
            var user = await GetUser(userId);
            CaculateVoucherInfo model = new();
            if(user != null)
            {
                var discount = CallBack.GetDiscount(user.Score);
                model.DiscountPrice = TotalPrice * discount/100;
                model.CurrentPrice = TotalPrice;
                model.TotalPrice = TotalPrice - model.DiscountPrice;
                model.UserId = userId;
                model.VoucherId = VoucherId;
            }
            return model;
        }

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
