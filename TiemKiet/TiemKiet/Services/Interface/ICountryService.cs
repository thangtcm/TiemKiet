using Microsoft.EntityFrameworkCore.Query;
using TiemKiet.Models;
using TiemKiet.ViewModel;

namespace TiemKiet.Services.Interface
{
    public interface ICountryService
    {
        public Task<ICollection<Country>> GetListAsync();
        public Task<ICollection<Country>> GetListAsync(Func<IQueryable<Country>, IIncludableQueryable<Country, object>> includes);
        public Task Add(CountryInfoVM countryInfo, long userId);
        public Task<Country?> GetByIdAsync(int Id);
        public Task<Country?> GetByIdAsync(int Id, Func<IQueryable<Country>, IIncludableQueryable<Country, object>> includes);
        public Country? GetById(int Id);
        public Task<bool> Delete(int Id, long userId);
        public Task Update(CountryInfoVM countryInfo, long userId);
    }
}
