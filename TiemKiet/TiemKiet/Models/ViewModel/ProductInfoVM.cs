using Newtonsoft.Json;
using TiemKiet.Enums;
using TiemKiet.Models;

namespace TiemKiet.Models.ViewModel
{
    public class ProductInfoVM
    {
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public double ProductPrice { get; set; }
        public double ProductPriceUpSize { get; set; }
        public double ProductSale { get; set; }
        public string? BranchName { get; set; }
        public bool? ItemUnavailable { get; set; }
        public string? ProductImage { get; set; }
        [JsonIgnore]
        public string ProductMBDescription { get; set; }
        [JsonIgnore]
        public IFormFile? UploadImage { get; set; }
        public ProductType ProductType { get; set; }

        public ProductInfoVM() { }
        public ProductInfoVM(Product model)
        {
            ProductId = model.Id;
            ProductName = model.ProductName ?? "";
            ProductDescription = model.ProductDescription ?? "";
            ProductPrice = model.ProductPrice;
            ProductSale = model.ProductSale;
            ProductImage = model.ProductImg is null ? "https://firebasestorage.googleapis.com/v0/b/tiemkiet-aa7d7.appspot.com/o/Images%2Flogo.png?alt=media&token=c8f55916-0d91-46a5-89ac-7d1b10fa39e8" : model.ProductImg;
            BranchName = model.Branch is null ? "" : model.Branch.BranchName ?? "";
            ProductType = model.ProductType;
            ProductPriceUpSize = model.ProductPriceUp;
            ItemUnavailable = model.ItemUnavailable;
        }
    }
}
