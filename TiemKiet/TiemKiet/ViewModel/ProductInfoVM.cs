using Newtonsoft.Json;
using TiemKiet.Enums;
using TiemKiet.Models;

namespace TiemKiet.ViewModel
{
    public class ProductInfoVM
    {
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set;}
        public double ProductPrice { get; set; }
        public double ProductSale { get; set; }
        public string? BranchName { get; set; }
        public string? ProductImage { get; set; }
        [JsonIgnore]
        public IFormFile UploadImage { get; set; }
        public ProductType ProductType { get; set; }

        public ProductInfoVM() { }
        public ProductInfoVM(Product model)
        {
            this.ProductId = model.Id;
            this.ProductName = model.ProductName ?? "";
            this.ProductDescription = model.ProductDescription ?? "";
            this.ProductPrice = model.ProductPrice;
            this.ProductSale = model.ProductSale;
            this.ProductImage = model.ProductImg is null ? "" : (model.ProductImg.ImageUrl ?? "");
            this.BranchName = model.Branch is null ? "" : (model.Branch.BranchName ?? "");
            this.ProductType = model.ProductType;
        }
    }
}
