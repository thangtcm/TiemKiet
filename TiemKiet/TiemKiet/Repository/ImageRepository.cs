using TiemKiet.Data;
using TiemKiet.Models;
using TiemKiet.Repository.GenericRepository;
using TiemKiet.Repository.Interface;

namespace TiemKiet.Repository
{
    public class ImageRepository : GenericRepository<ImageModel>, IImageRepository
    {
        public ImageRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
