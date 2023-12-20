using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq;
using TiemKiet.Enums;
using TiemKiet.Helpers;
using TiemKiet.Models;
using TiemKiet.Repository.UnitOfWork;
using TiemKiet.Services.Interface;
using TiemKiet.ViewModel;
using static System.Net.WebRequestMethods;

namespace TiemKiet.Services
{
    public class ProductService : IProductService
    {
        public IUnitOfWork _unitOfWork;
        private readonly IFirebaseStorageService _firebaseStorageService;
        public ProductService(IUnitOfWork unitOfWork, IFirebaseStorageService firebaseStorage)
        {
            _unitOfWork = unitOfWork;
            _firebaseStorageService= firebaseStorage;
        }
        public async Task Add(ProductInfoVM productInfo, long userId, int branchId)
        {
            var branchlst = await _unitOfWork.BranchRepository.GetAllAsync();
            ICollection<Product> productLst = new List<Product>();
            string resultImg;
            if (productInfo.UploadImage != null)
            {
                resultImg = (await _firebaseStorageService.UploadFile(productInfo.UploadImage)).ToString();
            }
            else
            {
                resultImg ="https://firebasestorage.googleapis.com/v0/b/tiemkiet-aa7d7.appspot.com/o/Images%2Flogo.png?alt=media&token=c8f55916-0d91-46a5-89ac-7d1b10fa39e8";
            }
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
                    ProductMBDescription = productInfo.ProductMBDescription,
                    ProductPriceUp = productInfo.ProductPriceUpSize,
                    UserIdUpdate = userId,
                };
                productLst.Add(product);
            }    
            _unitOfWork.ProductRepository.AddRange(productLst);
            await _unitOfWork.CommitAsync();
        }

        public async Task<ICollection<CategorywithProduct>> GetCategoriesWithProductsAsync()
        {
            var products = await _unitOfWork.ProductRepository.GetAllAsync(x => x.BranchId == 1);
            ICollection<CategorywithProduct> model = new List<CategorywithProduct>();
            foreach(ProductType e in Enum.GetValues(typeof(ProductType)))
            {
                var lst = products.Where(x => x.ProductType == e).ToList();
                var item = new CategorywithProduct()
                {
                    CategoryName = e.ToString(),
                    ProductCount = lst.Count,
                    CategoryId = (int)e,
                    Products = lst.Select(x => new ProductInfoVM(x)).ToList()
                };
                model.Add(item);
            }    
            return model;
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

        public async Task<ICollection<Product>> GetListBranchAsync(int branchId, Func<IQueryable<Product>, IIncludableQueryable<Product, object>> includes)
            => await _unitOfWork.ProductRepository.GetAllAsync(x => x.BranchId == branchId, includes);

        public async Task<ICollection<Product>> GetListAsync(int branchId, ProductType productType)
        {
            if(branchId != 0 && productType != 0)
            {
                return await _unitOfWork.ProductRepository.GetAllAsync(x => x.BranchId == branchId && x.ProductType == productType);
            }
            else if(branchId != 0 && productType == 0)
            {
                return await _unitOfWork.ProductRepository.GetAllAsync(x => x.BranchId == branchId);
            }
            else if (productType != 0 && branchId == 0)
            {
                return await _unitOfWork.ProductRepository.GetAllAsync(x => x.ProductType == productType);
            }
            else
                return await _unitOfWork.ProductRepository.GetAllAsync(null);
        }    

        public async Task<bool> Update(ProductInfoVM productInfo, long userId)
        {
            var product = await _unitOfWork.ProductRepository.GetAsync(x => x.Id == productInfo.ProductId);
            if (product != null)
            {
                if (productInfo.UploadImage != null)
                {
                    product.ProductImg = (await _firebaseStorageService.UploadFile(productInfo.UploadImage)).ToString();
                }
                product.ProductName = productInfo.ProductName;
                product.ProductPrice = productInfo.ProductPrice;
                product.ProductType = productInfo.ProductType;
                product.ProductDescription = productInfo.ProductDescription;
                product.DateUpdate = DateTime.UtcNow.ToTimeZone();
                product.UserIdUpdate = userId;
                product.ProductMBDescription = productInfo.ProductMBDescription;
                product.ProductPriceUp = productInfo.ProductPriceUpSize;
                _unitOfWork.ProductRepository.Update(product);
                await _unitOfWork.CommitAsync();
                return true;
            }
            return false;
        }
    }
}
