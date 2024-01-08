using Microsoft.EntityFrameworkCore.Query;
using TiemKiet.Models;
using TiemKiet.Models.ViewModel;

namespace TiemKiet.Services.Interface
{
    public interface IBlogService
    {
        public Task Add(BlogInfoVM blogInfoVM, long userId, IFormFile upload);
        public Task<ICollection<BlogPost>> GetListAsync();
        public Task<ICollection<BlogInfoVM>> GetListNewAsync(Func<IQueryable<BlogPost>, IIncludableQueryable<BlogPost, object>>? includes = null);
        public Task<ICollection<BlogPost>> GetListAsync(Func<IQueryable<BlogPost>, IIncludableQueryable<BlogPost, object>> includes);
        public Task<BlogPost?> GetByIdAsync(int Id);
        public Task<BlogPost?> GetByIdAsync(int Id, Func<IQueryable<BlogPost>, IIncludableQueryable<BlogPost, object>> includes);
        public BlogPost? GetById(int Id);
        public Task<bool> Delete(int Id, long userId);
        public Task<bool> Update(BlogInfoVM blogInfoVM, long userId);
    }
}
