using Microsoft.EntityFrameworkCore.Query;
using TiemKiet.Models;
using TiemKiet.Repository.UnitOfWork;
using TiemKiet.Services.Interface;
using TiemKiet.ViewModel;

namespace TiemKiet.Services
{
    public class BlogService : IBlogService
    {
        public IUnitOfWork _unitOfWork;
        public BlogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task Add(BlogInfoVM blogInfoVM, long userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int Id, long userId)
        {
            throw new NotImplementedException();
        }

        public BlogPost? GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<BlogPost?> GetByIdAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<BlogPost?> GetByIdAsync(int Id, Func<IQueryable<BlogPost>, IIncludableQueryable<BlogPost, object>> includes)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<BlogPost>> GetListAsync()
            => await _unitOfWork.BlogRepository.GetAllAsync();

        public Task<ICollection<BlogPost>> GetListAsync(Func<IQueryable<BlogPost>, IIncludableQueryable<BlogPost, object>> includes)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(BlogInfoVM blogInfoVM, long userId)
        {
            throw new NotImplementedException();
        }
    }
}
