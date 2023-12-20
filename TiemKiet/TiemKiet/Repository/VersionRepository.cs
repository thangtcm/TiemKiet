using TiemKiet.Data;
using TiemKiet.Models;
using TiemKiet.Repository.GenericRepository;
using TiemKiet.Repository.Interface;

namespace TiemKiet.Repository
{
    public class VersionRepository : GenericRepository<VersionModel>, IVersionRepository
    {
        public VersionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
