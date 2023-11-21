using Microsoft.AspNetCore.Identity;
using TiemKiet.Data;

namespace TiemKiet.Services.Interface
{
    public interface IRoleService
    {
        public Task<ICollection<ApplicationRole>> GetRoles();
    }
}
