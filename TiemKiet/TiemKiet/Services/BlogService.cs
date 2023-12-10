using Microsoft.EntityFrameworkCore.Query;
using TiemKiet.Helpers;
using TiemKiet.Models;
using TiemKiet.Repository.UnitOfWork;
using TiemKiet.Services.Interface;
using TiemKiet.ViewModel;

namespace TiemKiet.Services
{
    public class BlogService : IBlogService
    {
        public IUnitOfWork _unitOfWork;
        private readonly IFirebaseStorageService _firebaseStorageService;
        public BlogService(IUnitOfWork unitOfWork, IFirebaseStorageService firebaseStorageService)
        {
            _unitOfWork = unitOfWork;
            _firebaseStorageService = firebaseStorageService;
        }
        public async Task Add(BlogInfoVM blogInfoVM, long userId, IFormFile upload)
        {
            BlogPost model = new()
            {
                Author = blogInfoVM.Author,
                Content = blogInfoVM.Content,
                FeatheredImageUrl = (await _firebaseStorageService.UploadFile(upload)).ToString(),
                Heading = blogInfoVM.Heading,
                ShortDescription = blogInfoVM.ShortDescription,
                Visible = blogInfoVM.Visible,
                PublishedDate = DateTime.UtcNow.ToTimeZone(),
                DateUpdate = DateTime.UtcNow.ToTimeZone(),
                UserIdCreate = userId,
                UserIdUpdate = userId,
                Title = blogInfoVM.Title
            };
            _unitOfWork.BlogRepository.Add(model);
            await _unitOfWork.CommitAsync();
        }

        public Task<bool> Delete(int Id, long userId)
        {
            throw new NotImplementedException();
        }

        public BlogPost? GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<BlogPost?> GetByIdAsync(int Id)
            => await _unitOfWork.BlogRepository.GetAsync(x => x.Id == Id);

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
