﻿using Microsoft.AspNetCore.Identity;
using TiemKiet.Data;
using TiemKiet.Repository.UnitOfWork;
using TiemKiet.Services.Interface;

namespace TiemKiet.Services
{
    public class RoleService : IRoleService
    {
        public IUnitOfWork _unitOfWork;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public RoleService(IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<ICollection<ApplicationRole>> GetRoles()
            => await _unitOfWork.RoleRepository.GetAllAsync();
        
        public async Task<List<string>> GetUserRoles(ApplicationUser user)
            => new List<string>(await _userManager.GetRolesAsync(user));
    }
}
