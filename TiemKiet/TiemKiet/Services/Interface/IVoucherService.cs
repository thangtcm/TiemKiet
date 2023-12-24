using Microsoft.EntityFrameworkCore.Query;
using TiemKiet.Models;
using TiemKiet.Models.ViewModel;

namespace TiemKiet.Services.Interface
{
    public interface IVoucherService
    {
        public Task<ICollection<Voucher>> GetListAsync();
        public Task<ICollection<Voucher>> GetListAsync(Func<IQueryable<Voucher>, IIncludableQueryable<Voucher, object>> includes);
        public Task Add(Voucher model, long userId);
        public Task<Voucher?> GetByIdAsync(int Id);
        public Task<Voucher?> GetByIdAsync(int Id, Func<IQueryable<Voucher>, IIncludableQueryable<Voucher, object>> includes);
        public Voucher? GetById(int Id);
        public Task<bool> Delete(int Id, long userId);
        public Task<bool> Update(VoucherInfoVM voucherInfo, long userId);
    }
}
