using TiemKiet.Data;
using TiemKiet.Models;
using TiemKiet.Repository.GenericRepository;
using TiemKiet.Repository.Interface;

namespace TiemKiet.Repository
{
    public class ProductHomeRepository : GenericRepository<ProductHome>, IProductHomeRepository
    {
        public ProductHomeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
