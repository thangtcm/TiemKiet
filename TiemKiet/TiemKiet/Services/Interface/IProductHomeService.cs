using Microsoft.EntityFrameworkCore.Query;
using TiemKiet.Models;

namespace TiemKiet.Services.Interface
{
    public interface IProductHomeService
    {
        public Task<ICollection<ProductHome>> GetListAsync();
        public Task<ICollection<ProductHome>> GetListAsync(Func<IQueryable<ProductHome>, IIncludableQueryable<ProductHome, object>> includes);
        public Task Add(ProductHome product, long userId);
        public Task<ProductHome?> GetByIdAsync(int Id);
        public Task<ProductHome?> GetByIdAsync(int Id, Func<IQueryable<ProductHome>, IIncludableQueryable<ProductHome, object>> includes);
        public ProductHome? GetById(int Id);
        public Task Delete(int Id);
        public Task Update(ProductHome product, long userId);
    }
}
