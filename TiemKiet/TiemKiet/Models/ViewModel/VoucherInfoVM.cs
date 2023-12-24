using TiemKiet.Enums;
using TiemKiet.Models;

namespace TiemKiet.Models.ViewModel
{
    public class VoucherInfoVM
    {
        public int VoucherId { get; set; }
        public string VoucherCode { get; set; }
        public DiscountType DiscountType { get; set; }
        public VoucherType VoucherType { get; set; }
        public double DiscountValue { get; set; }
        public double DiscountPercentValue { get; set; }
        public double DiscountMoneyValue { get; set; }
        public string VoucherName { get; set; }
        public double MaxDiscountAmount { get; set; }
        public double MinBillAmount { get; set; }
        public bool IsRemove { get; set; }
        public VoucherInfoVM() { }
        public VoucherInfoVM(Voucher model)
        {
            VoucherId = model.Id;
            VoucherCode = model.Code;
            DiscountType = model.DiscountType;
            DiscountValue = model.DiscountValue;
            VoucherName = model.VoucherName ?? "";
            MaxDiscountAmount = model.MaxDiscountAmount;
            MinBillAmount = model.MinBillAmount;
            IsRemove = model.IsRemoved;
            VoucherType = model.VoucherType;
            switch (DiscountType)
            {
                case DiscountType.Percentage:
                    {
                        DiscountPercentValue = DiscountValue; break;
                    }
                case DiscountType.FixedAmount:
                    {
                        DiscountMoneyValue = DiscountValue; break;
                    }
            }
        }
    }
}
