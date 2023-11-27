using Microsoft.EntityFrameworkCore.Query;
using TiemKiet.Models;
using TiemKiet.ViewModel;

namespace TiemKiet.Services.Interface
{
    public interface IVoucherUserService
    {
        public Task<ICollection<VoucherUser>> GetListAsync(long? userId); // Lấy danh sách voucher của User đó hoặc là 0
        public Task<ICollection<VoucherUser>> GetListAsync(long? userId, Func<IQueryable<VoucherUser>, IIncludableQueryable<VoucherUser, object>> includes);
        public Task<VoucherUser?> GetByIdAsync(int Id);
        public Task<bool> Add(VoucherUserInfoVM voucherInfoVM);
        public Task<VoucherUser?> GetByIdAsync(int Id, Func<IQueryable<VoucherUser>, IIncludableQueryable<VoucherUser, object>> includes);
        public VoucherUser? GetById(int Id);
        public Task<bool> Delete(int Id, long userId);
    }
}
