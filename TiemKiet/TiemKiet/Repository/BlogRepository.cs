using TiemKiet.Data;
using TiemKiet.Models;
using TiemKiet.Repository.GenericRepository;
using TiemKiet.Repository.Interface;

namespace TiemKiet.Repository
{
    public class BlogRepository : GenericRepository<BlogPost>, IBlogRepository
    {
        public BlogRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
