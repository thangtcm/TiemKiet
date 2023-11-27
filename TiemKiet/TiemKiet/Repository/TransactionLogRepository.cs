using TiemKiet.Data;
using TiemKiet.Models;
using TiemKiet.Repository.GenericRepository;
using TiemKiet.Repository.Interface;

namespace TiemKiet.Repository
{
    public class TransactionLogRepository : GenericRepository<TransactionLog>, ITransactionLogRepository
    {
        public TransactionLogRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
