using Microsoft.AspNetCore.Identity;
using TiemKiet.Data;
using TiemKiet.Repository.GenericRepository;

namespace TiemKiet.Repository.Interface
{
    public interface IRoleRepository : IGenericRepository<ApplicationRole>
    {
    }
}
