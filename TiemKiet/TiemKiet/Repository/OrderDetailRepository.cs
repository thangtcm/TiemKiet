using TiemKiet.Data;
using TiemKiet.Models;
using TiemKiet.Repository.GenericRepository;
using TiemKiet.Repository.Interface;

namespace TiemKiet.Repository
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
