namespace TiemKiet.Models.ViewModel
{
    public class CartItemInfoVM
    {
        public int ProductId { get; set; }
        public string ProductNotes { get; set; }
        public int Quantity { get; set; }
        public bool UpSize { get; set; }
        public bool AddIce { get; set; }
    }
}
