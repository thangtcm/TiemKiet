using Microsoft.EntityFrameworkCore.Query;
using TiemKiet.Models;
using TiemKiet.ViewModel;

namespace TiemKiet.Services.Interface
{
    public interface IProductService
    {
        public Task Add(ProductInfoVM productInfo, long userId, IFormFile img);
        public Task<ICollection<Product>> GetListAsync();
        public Task<ICollection<Product>> GetListAsync(int branchId);
        public Task<ICollection<Product>> GetListAsync(Func<IQueryable<Product>, IIncludableQueryable<Product, object>> includes);
        public Task<ICollection<Product>> GetListAsync(int branchId, Func<IQueryable<Product>, IIncludableQueryable<Product, object>> includes);
        public Task<Product?> GetByIdAsync(int Id);
        public Task<Product?> GetByIdAsync(int Id, Func<IQueryable<Product>, IIncludableQueryable<Product, object>> includes);
        public Product? GetById(int Id);
        public Task<bool> Delete(int Id, long userId);
        public Task<bool> Update(ProductInfoVM productInfo, long userId);
    }
}
