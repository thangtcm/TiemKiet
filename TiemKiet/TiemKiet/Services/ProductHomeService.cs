using Microsoft.EntityFrameworkCore.Query;
using TiemKiet.Helpers;
using TiemKiet.Models;
using TiemKiet.Repository.UnitOfWork;
using TiemKiet.Services.Interface;

namespace TiemKiet.Services
{
    public class ProductHomeService : IProductHomeService
    {
        public IUnitOfWork _unitOfWork;
        public ProductHomeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }    
        public async Task Add(ProductHome product, long userId)
        {
            product.UserUpdateId = userId;
            product.DatePublish = DateTime.UtcNow.ToTimeZone();
            _unitOfWork.ProductHomeRepository.Add(product);
            await _unitOfWork.CommitAsync();
        }

        public async Task Delete(int Id)
        {
            var product = await _unitOfWork.ProductHomeRepository.GetAsync(x => x.Id == Id);
            if(product != null)
            {
                _unitOfWork.ProductHomeRepository.Remove(product);
                await _unitOfWork.CommitAsync();
            }    
        }

        public ProductHome? GetById(int Id)
            => _unitOfWork.ProductHomeRepository.Get(x => x.Id == Id);

        public async Task<ProductHome?> GetByIdAsync(int Id)
            => await _unitOfWork.ProductHomeRepository.GetAsync(x => x.Id == Id);

        public async Task<ProductHome?> GetByIdAsync(int Id, Func<IQueryable<ProductHome>, IIncludableQueryable<ProductHome, object>> includes)
            => await _unitOfWork.ProductHomeRepository.GetAsync(x => x.Id == Id, includes);

        public async Task<ICollection<ProductHome>> GetListAsync()
            => await _unitOfWork.ProductHomeRepository.GetAllAsync();

        public async Task<ICollection<ProductHome>> GetListAsync(Func<IQueryable<ProductHome>, IIncludableQueryable<ProductHome, object>> includes)
            => await _unitOfWork.ProductHomeRepository.GetAllAsync(null, includes);

        public async Task Update(ProductHome product, long userId)
        {
            var model = await _unitOfWork.ProductHomeRepository.GetAsync(x => x.Id == product.Id);
            if(model != null)
            {
                model.DatePublish = DateTime.UtcNow.ToTimeZone();
                model.UserUpdateId = userId;
                model.productHomeType = product.productHomeType;
                _unitOfWork.ProductHomeRepository.Update(model);
                await _unitOfWork.CommitAsync();
            }    
        }
    }
}
