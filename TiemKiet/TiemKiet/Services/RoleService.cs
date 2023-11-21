using Microsoft.AspNetCore.Identity;
using TiemKiet.Data;
using TiemKiet.Repository.UnitOfWork;
using TiemKiet.Services.Interface;

namespace TiemKiet.Services
{
    public class RoleService : IRoleService
    {
        public IUnitOfWork _unitOfWork;
        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ICollection<ApplicationRole>> GetRoles()
            => await _unitOfWork.RoleRepository.GetAllAsync();
    }
}
