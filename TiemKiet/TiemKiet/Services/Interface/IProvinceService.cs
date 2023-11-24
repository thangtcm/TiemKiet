using Microsoft.EntityFrameworkCore.Query;
using TiemKiet.Models;
using TiemKiet.ViewModel;

namespace TiemKiet.Services.Interface
{
    public interface IProvinceService
    {
        public Task<ICollection<Province>> GetListAsync();
        public Task<ICollection<Province>> GetListAsync(int countryId);
        public Task<ICollection<Province>> GetListAsync(Func<IQueryable<Province>, IIncludableQueryable<Province, object>> includes);
        public Task<ICollection<Province>> GetListAsync(int countryId, Func<IQueryable<Province>, IIncludableQueryable<Province, object>> includes);
        public Task Add(ProvinceInfoVM provinceInfo, long userId);
        public Task<Province?> GetByIdAsync(int Id);
        public Task<Province?> GetByIdAsync(int Id, Func<IQueryable<Province>, IIncludableQueryable<Province, object>> includes);
        public Province? GetById(int Id);
        public Task<bool> Delete(int Id, long userId);
        public Task<bool> Update(ProvinceInfoVM provinceInfo, long userId);
    }
}
