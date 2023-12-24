using Microsoft.EntityFrameworkCore.Query;
using TiemKiet.Models;
using TiemKiet.Models.ViewModel;

namespace TiemKiet.Services.Interface
{
    public interface IImageService
    {
        public Task<ICollection<ImageModel>> GetListAsync();
        public Task<ImageModel> Add(IFormFile upload, long userId);
        public Task<ICollection<ImageModel>> AddRange(List<IFormFile> uploadLst, long userId);
        public Task<ImageModel?> GetByIdAsync(int Id);
        public ImageModel? GetById(int Id);
        public Task<bool> Delete(int Id, long userId);
    }
}
