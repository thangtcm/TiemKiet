namespace TiemKiet.Models.ViewModel
{
    public class CaculateVoucherInfo
    {
        public long UserId { get; set; }
        public double CurrentPrice { get; set; }
        public double DiscountPrice { get; set; }
        public double DiscountShipPrice { get; set; }
        public double TotalPrice { get; set; }
        public double ShipPrice { get; set; }
        public List<int> VoucherList { get; set; }
    }
}
