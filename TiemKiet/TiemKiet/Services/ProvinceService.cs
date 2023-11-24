using Microsoft.EntityFrameworkCore.Query;
using TiemKiet.Models;
using TiemKiet.Repository.UnitOfWork;
using TiemKiet.Services.Interface;
using TiemKiet.ViewModel;

namespace TiemKiet.Services
{
    public class ProvinceService : IProvinceService
    {
        public IUnitOfWork _unitOfWork;
        public ProvinceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Add(ProvinceInfoVM provinceInfo, long userId)
        {
            Province province = new()
            {
                CityName = provinceInfo.ProvinceName,
                DateCreate = DateTime.Now,
                DateUpdate = DateTime.Now,
                CountryId = provinceInfo.CountryId,
                UserIdCreate= userId,
                UserIdUpdate= userId,
                IsRemoved= false,
            };
            _unitOfWork.ProvinceRepository.Add(province);
            await _unitOfWork.CommitAsync();
        }

        public async Task<bool> Delete(int Id, long userId)
        {
            var province = await _unitOfWork.ProvinceRepository.GetAsync(x => x.Id == Id && x.IsRemoved == false);
            if(province == null) return false;
            province.IsRemoved = true;
            province.UserIdRemove = userId;
            province.DateRemove = DateTime.Now;
            await _unitOfWork.CommitAsync();
            return true;
        }

        public Province? GetById(int Id)
            => _unitOfWork.ProvinceRepository.Get(x => x.Id == Id);

        public async Task<Province?> GetByIdAsync(int Id)
            => await _unitOfWork.ProvinceRepository.GetAsync(x => x.Id == Id);

        public async Task<Province?> GetByIdAsync(int Id, Func<IQueryable<Province>, IIncludableQueryable<Province, object>> includes)
            => await _unitOfWork.ProvinceRepository.GetAsync(x => x.Id == Id, includes);

        public async Task<ICollection<Province>> GetListAsync()
            => await _unitOfWork.ProvinceRepository.GetAllAsync();

        public async Task<ICollection<Province>> GetListAsync(int countryId)
            => await _unitOfWork.ProvinceRepository.GetAllAsync(x => x.CountryId == countryId);

        public async Task<ICollection<Province>> GetListAsync(Func<IQueryable<Province>, IIncludableQueryable<Province, object>> includes)
            => await _unitOfWork.ProvinceRepository.GetAllAsync(null, includes);
        public async Task<ICollection<Province>> GetListAsync(int countryId, Func<IQueryable<Province>, IIncludableQueryable<Province, object>> includes)
            => await _unitOfWork.ProvinceRepository.GetAllAsync(x => x.CountryId == countryId, includes);
        public async Task<bool> Update(ProvinceInfoVM provinceInfo, long userId)
        {
            var province = await _unitOfWork.ProvinceRepository.GetAsync(x => x.Id == provinceInfo.ProvinceId && x.IsRemoved == false);
            if (province == null) return false;
            province.CityName = provinceInfo.ProvinceName;
            province.UserIdUpdate = userId;
            province.DateUpdate = DateTime.Now;
            _unitOfWork.ProvinceRepository.Update(province);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
