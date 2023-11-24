using TiemKiet.Data;
using TiemKiet.Models;
using TiemKiet.Repository.GenericRepository;
using TiemKiet.Repository.Interface;

namespace TiemKiet.Repository
{
    public class BranchRepository : GenericRepository<Branch>, IBranchRepository
    {
        public BranchRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
