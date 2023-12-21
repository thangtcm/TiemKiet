namespace TiemKiet.Models
{
    public class CartItem
    {
        public Product Product { get; set; }
        public string ProductNotes { get; set; }
        public int Quantity { get; set; }
        public bool Upsize { get; set; }
        public bool AddIce { get; set; } 
        public bool IsSameProduct(Product product, bool upsize, bool addIce)
        {
            return Product == product && Upsize == upsize && AddIce == addIce;
        }

        public double getPrice()
        {
            return this.Product.ProductPrice + (Upsize is true ? this.Product.ProductPriceUp : 0);
        }    
    }
}
