using Microsoft.AspNetCore.Identity;
using TiemKiet.Data;
using TiemKiet.Helpers;
using TiemKiet.Repository.UnitOfWork;
using TiemKiet.Services.Interface;
using TiemKiet.ViewModel;

namespace TiemKiet.Services
{
    public class UserService : IUserService
    {
        public IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApplicationUser?> GetUser()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User);
            return user;
        }

        public async Task<ICollection<ApplicationUser>> GetUsers()
            => await _unitOfWork.UserRepository.GetAllAsync();

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
    }
}
