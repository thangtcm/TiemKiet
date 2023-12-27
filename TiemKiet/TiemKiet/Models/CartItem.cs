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
        public CartItem() {  }
        public CartItem(Product product)
        {
            this.ProductName = product.ProductName ?? "";
            this.ProductSale = product.ProductSale;
            this.ProductPrice = product.ProductPrice;
            this.ProductPriceUp = product.ProductPriceUp;
        }
        public bool IsSameProduct(Product product, bool upsize, bool addIce)
        {
            return ProductId == product.Id && ProductUpsize == upsize && ProductHasIce == addIce;
        }

        public double getPrice()
        {
            return this.ProductPrice + (ProductUpsize is true ? this.ProductPriceUp : 0) - this.ProductSale;
        }
    }
}
