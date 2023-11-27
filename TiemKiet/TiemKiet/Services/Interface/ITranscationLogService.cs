using Microsoft.EntityFrameworkCore.Query;
using TiemKiet.Models;
using TiemKiet.ViewModel;

namespace TiemKiet.Services.Interface
{
    public interface ITranscationLogService
    {
        public Task<ICollection<TransactionLog>> GetListAsync(long userId);
        public Task<ICollection<TransactionLog>> GetListAsync();
        public Task<ICollection<TransactionLog>> GetListAsync(long userId, Func<IQueryable<TransactionLog>, IIncludableQueryable<TransactionLog, object>> includes);
        public Task Add(TransactionLogVM transcationInfoVM, long userCustomerId, long userStaffId);
        public Task<TransactionLog?> GetByIdAsync(int Id);
        public Task<TransactionLog?> GetByIdAsync(int Id, Func<IQueryable<TransactionLog>, IIncludableQueryable<TransactionLog, object>> includes);
        public TransactionLog? GetById(int Id);
        public Task<bool> Delete(int Id, long userId);
    }
}
