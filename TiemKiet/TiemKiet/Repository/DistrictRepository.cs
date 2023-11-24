using TiemKiet.Data;
using TiemKiet.Models;
using TiemKiet.Repository.GenericRepository;
using TiemKiet.Repository.Interface;

namespace TiemKiet.Repository
{
    public class DistrictRepository : GenericRepository<District>, IDistrictRepository
    {
        public DistrictRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
