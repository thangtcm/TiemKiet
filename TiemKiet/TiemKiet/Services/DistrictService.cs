using Microsoft.EntityFrameworkCore.Query;
using TiemKiet.Helpers;
using TiemKiet.Models;
using TiemKiet.Models.ViewModel;
using TiemKiet.Repository.UnitOfWork;
using TiemKiet.Services.Interface;

namespace TiemKiet.Services
{
    public class DistrictService : IDistrictService
    {
        public IUnitOfWork _unitOfWork;
        public DistrictService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Add(DistrictInfoVM districtInfo, long userId)
        {
            District district = new()
            {
                DateCreate = DateTime.UtcNow.ToTimeZone(),
                DateUpdate = DateTime.UtcNow.ToTimeZone(),
                IsRemoved = false,
                ProvinceId = districtInfo.ProvinceId,
                UserIdCreate = userId,
                UserIdUpdate = userId,
                DistrictName = districtInfo.DistrictName,
            };
            _unitOfWork.DistrictRepository.Add(district);
            await _unitOfWork.CommitAsync();
        }

        public async Task<bool> Delete(int Id, long userId)
        {
            var district = await _unitOfWork.DistrictRepository.GetAsync(x => x.Id== Id && x.IsRemoved == false);
            if (district == null) return false;
            district.IsRemoved = true;
            district.UserIdRemove = userId;
            district.DateRemove = DateTime.UtcNow.ToTimeZone();
            await _unitOfWork.CommitAsync();
            return true;
        }

        public District? GetById(int Id)
            => _unitOfWork.DistrictRepository.Get(x => x.Id == Id);

        public async Task<District?> GetByIdAsync(int Id)
            => await _unitOfWork.DistrictRepository.GetAsync(x => x.Id == Id);

        public async Task<District?> GetByIdAsync(int Id, Func<IQueryable<District>, IIncludableQueryable<District, object>> includes)
            => await _unitOfWork.DistrictRepository.GetAsync(x => x.Id == Id, includes);

        public async Task<ICollection<District>> GetListAsync()
            => await _unitOfWork.DistrictRepository.GetAllAsync();

        public async Task<ICollection<District>> GetListAsync(int provinceId)
             => await _unitOfWork.DistrictRepository.GetAllAsync(x => x.ProvinceId == provinceId);

        public async Task<ICollection<District>> GetListAsync(Func<IQueryable<District>, IIncludableQueryable<District, object>> includes)
            => await _unitOfWork.DistrictRepository.GetAllAsync(null, includes);

        public async Task<ICollection<District>> GetListAsync(int provinceId, Func<IQueryable<District>, IIncludableQueryable<District, object>> includes)
            => await _unitOfWork.DistrictRepository.GetAllAsync(x => x.ProvinceId == provinceId, includes);

        public async Task<bool> Update(DistrictInfoVM districtInfo, long userId)
        {
            var district = await _unitOfWork.DistrictRepository.GetAsync(x => x.Id == districtInfo.DistrictId && x.IsRemoved == false);
            if (district == null) return false;
            district.DistrictName = districtInfo.DistrictName;
            district.UserIdUpdate = userId;
            district.DateUpdate = DateTime.UtcNow.ToTimeZone();
            _unitOfWork.DistrictRepository.Update(district);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
