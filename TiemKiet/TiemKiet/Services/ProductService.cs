using Microsoft.EntityFrameworkCore.Query;
using TiemKiet.Enums;
using TiemKiet.Helpers;
using TiemKiet.Models;
using TiemKiet.Repository.UnitOfWork;
using TiemKiet.Services.Interface;
using TiemKiet.ViewModel;

namespace TiemKiet.Services
{
    public class ProductService : IProductService
    {
        public IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;
        public ProductService(IUnitOfWork unitOfWork, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }
        public async Task Add(ProductInfoVM productInfo, long userId, int branchId)
        {
            var branchlst = await _unitOfWork.BranchRepository.GetAllAsync();
            ICollection<Product> productLst = new List<Product>();
            var resultImg = await _imageService.Add(productInfo.UploadImage, userId);
            foreach (var x in branchlst)
            {
                Product product = new()
                {
                    BranchId = x.Id,
                    DateCreate = DateTime.UtcNow.ToTimeZone(),
                    DateUpdate = DateTime.UtcNow.ToTimeZone(),
                    ProductType = productInfo.ProductType,
                    ProductName = productInfo.ProductName,
                    UserIdCreate = userId,
                    ProductImg = resultImg,
                    ProductPrice = productInfo.ProductPrice,
                    ProductDescription = productInfo.ProductDescription,
                    UserIdUpdate = userId,
                };
                productLst.Add(product);
            }    
            _unitOfWork.ProductRepository.AddRange(productLst);
            await _unitOfWork.CommitAsync();
        }

        public async Task<bool> Delete(int Id, long userId)
        {
            var product = await _unitOfWork.ProductRepository.GetAsync(x => x.Id == Id);
            if (product == null) return false;
            product.IsRemoved = true;
            _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public Product? GetById(int Id)
            => _unitOfWork.ProductRepository.Get(x => x.Id == Id);

        public async Task<Product?> GetByIdAsync(int Id)
            => await _unitOfWork.ProductRepository.GetAsync(x => x.Id == Id);

        public async Task<Product?> GetByIdAsync(int Id, Func<IQueryable<Product>, IIncludableQueryable<Product, object>> includes)
            => await _unitOfWork.ProductRepository.GetAsync(x => x.Id == Id, includes);

        public async Task<ICollection<Product>> GetListAsync()
            => await _unitOfWork.ProductRepository.GetAllAsync();

        public async Task<ICollection<Product>> GetListAsync(int branchId)
            => await _unitOfWork.ProductRepository.GetAllAsync(x => x.BranchId == branchId);

        public async Task<ICollection<Product>> GetListAsync(Func<IQueryable<Product>, IIncludableQueryable<Product, object>> includes)
            => await _unitOfWork.ProductRepository.GetAllAsync(null, includes);

        public async Task<ICollection<Product>> GetListAsync(int branchId, ProductType productType, Func<IQueryable<Product>, IIncludableQueryable<Product, object>> includes)
        {
            if(branchId != 0 && productType != 0)
            {
                return await _unitOfWork.ProductRepository.GetAllAsync(x => x.BranchId == branchId && x.ProductType == productType, includes);
            }
            else if(branchId != 0 && productType == 0)
            {
                return await _unitOfWork.ProductRepository.GetAllAsync(x => x.BranchId == branchId, includes);
            }
            else if (productType != 0 && branchId == 0)
            {
                return await _unitOfWork.ProductRepository.GetAllAsync(x => x.ProductType == productType, includes);
            }
            else
                return await _unitOfWork.ProductRepository.GetAllAsync(null, includes);
        }    

        public async Task<bool> Update(ProductInfoVM productInfo, long userId)
        {
            var product = await _unitOfWork.ProductRepository.GetAsync(x => x.Id == productInfo.ProductId);
            if (product != null)
            {
                product.ProductName = productInfo.ProductName;
                product.ProductPrice = productInfo.ProductPrice;
                product.ProductType = productInfo.ProductType;
                product.ProductDescription = productInfo.ProductDescription;
                product.DateUpdate = DateTime.UtcNow.ToTimeZone();
                product.UserIdUpdate = userId;
                _unitOfWork.ProductRepository.Update(product);
                await _unitOfWork.CommitAsync();
                return true;
            }
            return false;
        }
    }
}
