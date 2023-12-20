using TiemKiet.Models;
using TiemKiet.Repository.UnitOfWork;
using TiemKiet.Services.Interface;

namespace TiemKiet.Services
{
    public class VersionService : IVersionService
    {
        public IUnitOfWork _unitOfWork;
        public VersionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<VersionModel?> GetByIdAsync(int Id = 1)
            => await _unitOfWork.VersionRepository.GetAsync(x => x.Id == Id);

        public async Task Update(VersionModel model)
        {
            var version = await _unitOfWork.VersionRepository.GetAsync(x => x.Id == 1);
            if(version != null)
            {
                version.UrlAndroidVersion = model.UrlAndroidVersion;
                version.UrlIOSVersion = model.UrlIOSVersion;
                version.IsMaintenance = model.IsMaintenance;
                version.IsDeploy = model.IsDeploy;
            }    
            _unitOfWork.VersionRepository.Update(model);
            await _unitOfWork.CommitAsync();
        }
    }
}
