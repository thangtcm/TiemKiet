using TiemKiet.Enums;

namespace TiemKiet.ViewModel
{
    public class VoucherInfoVM
    {
        public int VoucherId { get; set; }
        public string VoucherCode { get; set;}
        public DiscountType DiscountType { get; set; }
        public double DiscountValue { get; set; }
        public string VoucherName { get; set;}
        public double MaxDiscountAmount { get; set; }
        public double MinBillAmount { get; set; }
    }   
}
