using Microsoft.EntityFrameworkCore.Query;
using TiemKiet.Models;
using TiemKiet.Repository.UnitOfWork;
using TiemKiet.Services.Interface;
using TiemKiet.ViewModel;

namespace TiemKiet.Services
{
    public class CountryService : ICountryService
    {
        public IUnitOfWork _unitOfWork;
        public CountryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(CountryInfoVM countryInfo, long userId)
        {
            Country country = new()
            {
                CountryName = countryInfo.CountryName,
                UserIdCreate = userId,
                UserIdUpdate = userId,
                DateCreate = DateTime.Now,
                DateUpdate = DateTime.Now
            };
            _unitOfWork.CountryRepository.Add(country);
            await _unitOfWork.CommitAsync();
        }

        public async Task<ICollection<Country>> GetListAsync()
            => await _unitOfWork.CountryRepository.GetAllAsync();

        public async Task<ICollection<Country>> GetListAsync(Func<IQueryable<Country>, IIncludableQueryable<Country, object>> includes)
            => await _unitOfWork.CountryRepository.GetAllAsync(null, includes);

        public async Task<Country?> GetByIdAsync(int Id)
            => await _unitOfWork.CountryRepository.GetAsync(x => x.Id == Id && x.IsRemoved == false);

        public async Task<Country?> GetByIdAsync(int Id, Func<IQueryable<Country>, IIncludableQueryable<Country, object>> includes)
            => await _unitOfWork.CountryRepository.GetAsync(x => x.Id == Id && x.IsRemoved == false, includes);

        public Country? GetById(int Id)
            => _unitOfWork.CountryRepository.Get(x => x.Id == Id && x.IsRemoved == false);

        public async Task<bool> Delete(int Id, long userId)
        {
            var country = await _unitOfWork.CountryRepository.GetAsync(x => x.Id == Id);
            if (country == null) return false;
            country.IsRemoved = true;
            country.UserIdRemove = userId;
            _unitOfWork.CountryRepository.Update(country);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task Update(CountryInfoVM countryInfo, long userId)
        {
            var model = await _unitOfWork.CountryRepository.GetAsync(x => x.Id == countryInfo.CountryId);
            if (model != null)
            {
                model.UserIdUpdate = userId;
                model.DateUpdate = DateTime.Now;
                model.CountryName = countryInfo.CountryName;
                _unitOfWork.CountryRepository.Update(model);
                await _unitOfWork.CommitAsync();
            }
        }
    }
}
