using TiemKiet.Enums;
using TiemKiet.Models;

namespace TiemKiet.ViewModel
{
    public class VoucherInfoVM
    {
        public int VoucherId { get; set; }
        public string VoucherCode { get; set;}
        public DiscountType DiscountType { get; set; }
        public VoucherType VoucherType { get; set; }
        public double DiscountValue { get; set; }
        public string VoucherName { get; set;}
        public double MaxDiscountAmount { get; set; }
        public double MinBillAmount { get; set; }
        public bool IsRemove { get; set; }
        public VoucherInfoVM() { }
        public VoucherInfoVM (Voucher model)
        {
            this.VoucherId = model.Id;
            this.VoucherCode = model.Code;
            this.DiscountType = model.DiscountType;
            this.DiscountValue = model.DiscountValue;
            this.VoucherName = model.VoucherName ?? "";
            this.MaxDiscountAmount= model.MaxDiscountAmount;
            this.MinBillAmount = model.MinBillAmount;
            this.IsRemove = model.IsRemoved;
            this.VoucherType = model.VoucherType;
        }
    }   
}
