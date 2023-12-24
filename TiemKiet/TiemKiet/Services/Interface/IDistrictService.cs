using Microsoft.EntityFrameworkCore.Query;
using TiemKiet.Models;
using TiemKiet.Models.ViewModel;

namespace TiemKiet.Services.Interface
{
    public interface IDistrictService
    {
        public Task<ICollection<District>> GetListAsync();
        public Task<ICollection<District>> GetListAsync(int provinceId);
        public Task<ICollection<District>> GetListAsync(Func<IQueryable<District>, IIncludableQueryable<District, object>> includes);
        public Task<ICollection<District>> GetListAsync(int provinceId, Func<IQueryable<District>, IIncludableQueryable<District, object>> includes);
        public Task Add(DistrictInfoVM districtInfo, long userId);
        public Task<District?> GetByIdAsync(int Id);
        public Task<District?> GetByIdAsync(int Id, Func<IQueryable<District>, IIncludableQueryable<District, object>> includes);
        public District? GetById(int Id);
        public Task<bool> Delete(int Id, long userId);
        public Task<bool> Update(DistrictInfoVM districtInfo, long userId);
    }
}
