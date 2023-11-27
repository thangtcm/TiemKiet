using TiemKiet.Data;
using TiemKiet.Models;
using TiemKiet.Repository.GenericRepository;
using TiemKiet.Repository.Interface;

namespace TiemKiet.Repository
{
    public class UserTokenRepository : GenericRepository<ApplicationUserToken>, IUserTokenRepository
    {
        public UserTokenRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
