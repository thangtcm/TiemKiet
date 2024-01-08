namespace TiemKiet.Models.ViewModel
{
    public class ConfirmCart
    {
        public double GrandTotal { get; set; }
        public int VoucherId { get; set; }
        public List<CartItem> CartItems { get; set; }
    }
}
