using TiemKiet.Data;
using TiemKiet.Models;
using TiemKiet.Repository.GenericRepository;
using TiemKiet.Repository.Interface;

namespace TiemKiet.Repository
{
    public class ProvinceRepository : GenericRepository<Province>, IProvinceRepository
    {
        public ProvinceRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
