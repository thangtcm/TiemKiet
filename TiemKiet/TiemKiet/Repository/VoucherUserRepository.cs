using TiemKiet.Data;
using TiemKiet.Models;
using TiemKiet.Repository.GenericRepository;
using TiemKiet.Repository.Interface;

namespace TiemKiet.Repository
{
    public class VoucherUserRepository : GenericRepository<VoucherUser>, IVoucherUserRepository
    {
        public VoucherUserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
