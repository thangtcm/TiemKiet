using Microsoft.EntityFrameworkCore.Query;
using TiemKiet.Models;
using TiemKiet.ViewModel;

namespace TiemKiet.Services.Interface
{
    public interface IBlogService
    {
        public Task Add(BlogInfoVM blogInfoVM, long userId);
        public Task<ICollection<BlogPost>> GetListAsync();
        public Task<ICollection<BlogPost>> GetListAsync(Func<IQueryable<BlogPost>, IIncludableQueryable<BlogPost, object>> includes);
        public Task<BlogPost?> GetByIdAsync(int Id);
        public Task<BlogPost?> GetByIdAsync(int Id, Func<IQueryable<BlogPost>, IIncludableQueryable<BlogPost, object>> includes);
        public BlogPost? GetById(int Id);
        public Task<bool> Delete(int Id, long userId);
        public Task<bool> Update(BlogInfoVM blogInfoVM, long userId);
    }
}
