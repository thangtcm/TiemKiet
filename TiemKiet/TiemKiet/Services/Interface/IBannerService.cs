using Microsoft.EntityFrameworkCore.Query;
using TiemKiet.Models;
using TiemKiet.Models.ViewModel;

namespace TiemKiet.Services.Interface
{
    public interface IBannerService
    {
        public Task<ICollection<Banner>> GetListAsync();
        public Task Add(BannerInfoVM model, long userId);
    }
}
