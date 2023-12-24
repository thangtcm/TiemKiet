namespace TiemKiet.Models.ViewModel
{
    public class CategorywithProduct
    {
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public int ProductCount { get; set; }
        public ICollection<ProductInfoVM> Products { get; set; }
    }
}
