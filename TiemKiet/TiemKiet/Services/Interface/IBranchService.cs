using Microsoft.EntityFrameworkCore.Query;
using TiemKiet.Models;
using TiemKiet.ViewModel;

namespace TiemKiet.Services.Interface
{
    public interface IBranchService
    {
        public Task<ICollection<Branch>> GetListAsync();
        public Task<ICollection<Branch>> GetListAsync(int? districtId, int? provinceId);
        public Task<ICollection<ProvinceBranchVM>> GetListWithBranchAsync();
        public Task<ICollection<Branch>> GetListAsync(int? districtId, int? provinceId, Func<IQueryable<Branch>, IIncludableQueryable<Branch, object>> includes);
        public Task<ICollection<Branch>> GetListAsync(Func<IQueryable<Branch>, IIncludableQueryable<Branch, object>> includes);
        public Task Add(BranchInfoVM branchInfo, long userId, int districtId);
        public Task<Branch?> GetByIdAsync(int Id);
        public Task<Branch?> GetByIdAsync(int Id, Func<IQueryable<Branch>, IIncludableQueryable<Branch, object>> includes);
        public Branch? GetById(int Id);
        public Task<bool> Delete(int Id, long userId);
        public Task Update(BranchInfoVM branchInfo, long userId);
    }
}
