using TiemKiet.Data;
using TiemKiet.Models;
using TiemKiet.Repository.GenericRepository;
using TiemKiet.Repository.Interface;

namespace TiemKiet.Repository
{
    public class ManagerVoucherLogRepository : GenericRepository<ManagerVoucherLog>, IManagerVoucherLogRepository
    {
        public ManagerVoucherLogRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
