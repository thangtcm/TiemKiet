using TiemKiet.Data;
using TiemKiet.ViewModel;

namespace TiemKiet.Services.Interface
{
    public interface IUserService
    {
        public Task<ICollection<ApplicationUser>> GetUsers();
        public Task<ApplicationUser?> GetUser(long userId);
        public Task<bool> UpdateUser(UserInfoVM user);
        public Task<ApplicationUser?> GetUser();
    }
}
