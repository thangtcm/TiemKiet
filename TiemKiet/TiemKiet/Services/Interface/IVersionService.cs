using TiemKiet.Models;

namespace TiemKiet.Services.Interface
{
    public interface IVersionService
    {
        public Task<VersionModel?> GetByIdAsync(int Id = 1);
        public Task Update(VersionModel version);
    }
}
