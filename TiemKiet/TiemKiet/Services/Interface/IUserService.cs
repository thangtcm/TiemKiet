using System.ComponentModel.DataAnnotations;
using TiemKiet.Data;
using TiemKiet.ViewModel;

namespace TiemKiet.Services.Interface
{
    public interface IUserService
    {
        public Task<ICollection<ApplicationUser>> GetUsers();
        public Task<ResponseListVM<UserInfoVM>> GetUsersWithRoles(int page = 1);
        public Task<ICollection<UserInfoVM>> GetUsersWithRoles();
        public Task<ApplicationUser?> GetUser(long userId);
        public Task<bool> UpdateUser(UserInfoVM user);
        public Task<ApplicationUser?> GetUser();
        public Task<ApplicationUser?> GetUserwithPhone([Phone] string Phone);
        public Task<StatusResponse<CaculateVoucherInfo>> CaculatePrice(long userId, double TotalPrice, double ShipPrice, List<int> VoucherList);
        public Task UpdatePoint(CaculateVoucherInfo model, long userId);
    }
}
