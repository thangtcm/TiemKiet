using Microsoft.EntityFrameworkCore.Query;
using TiemKiet.Models;

namespace TiemKiet.Services.Interface
{
    public interface IUserTokenService
    {
        public Task<ICollection<ApplicationUserToken>> GetListAsync(long? userId); // Lấy danh sách voucher của User đó hoặc là 0
        public Task<ICollection<ApplicationUserToken>> GetListAsync(long? userId, Func<IQueryable<ApplicationUserToken>, IIncludableQueryable<ApplicationUserToken, object>> includes);
        public Task<ApplicationUserToken?> GetByIdAsync(int Id);
        public Task Add(long? userId, string token);
        public Task<ApplicationUserToken?> GetByIdAsync(int Id, Func<IQueryable<ApplicationUserToken>, IIncludableQueryable<ApplicationUserToken, object>> includes);
        public ApplicationUserToken? GetById(int Id);
        public Task<bool> Delete(int Id, long userId);
    }
}
