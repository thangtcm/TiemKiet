using System.Text.Json.Serialization;

namespace TiemKiet.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductNotes { get; set; }
        public double ProductPrice { get; set; }
        public double ProductPriceUp { get; set; }
        public double ProductSale { get; set; }
        public int ProductQuantity { get; set; }
        public bool ProductUpsize { get; set; }
        public bool ProductHasIce { get; set; } 
        public string ProductImage { get; set; }
        [JsonIgnore]
        public int BranchId { get; set; }
        public CartItem() {  }
        public CartItem(Product product)
        {
            this.ProductName = product.ProductName ?? "";
            this.ProductSale = product.ProductSale;
            this.ProductId = product.Id;
            this.ProductPrice = product.ProductPrice;
            this.ProductPriceUp = product.ProductPriceUp;
            this.ProductImage = product.ProductImg ?? "https://firebasestorage.googleapis.com/v0/b/tiemkiet-aa7d7.appspot.com/o/Images%2Flogo.png?alt=media&token=c8f55916-0d91-46a5-89ac-7d1b10fa39e8";
        }
        public bool IsSameProduct(Product product, bool upsize, bool addIce)
        {
            return ProductId == product.Id && ProductUpsize == upsize && ProductHasIce == addIce;
        }

        public double getProductPrice()
        {
            return (this.ProductPrice + (ProductUpsize is true ? this.ProductPriceUp : 0) - this.ProductSale);
        }

        public double getTotalPrice()
        {
            return (this.ProductPrice + (ProductUpsize is true ? this.ProductPriceUp : 0) - this.ProductSale) * ProductQuantity;
        }
    }
}
