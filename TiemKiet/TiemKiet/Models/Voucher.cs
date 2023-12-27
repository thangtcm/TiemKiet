using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TiemKiet.Data;
using TiemKiet.Enums;
using TiemKiet.Helpers;

namespace TiemKiet.Models
{
    public class Voucher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DisplayName("Tên voucher")]
        public string? VoucherName { get; set; }
        [DisplayName("Mã giảm giá")]
        public string Code { get; set; }
        [Display(Name ="Loại voucher")]
        public VoucherType VoucherType { get; set; }
        [DisplayName("Loại giảm giá")]
        public DiscountType DiscountType { get; set; }
        [DisplayName("Giảm giá")]
        public double DiscountValue { get; set; }
        [DisplayName("Giảm giá tối đa")]
        public double MaxDiscountAmount { get; set; }
        [DisplayName("Tổng hóa đơn tối thiểu")]
        public double MinBillAmount { get; set; }
        [DisplayName("Thời lượng hết hạn (Ngày)")]
        public int ExpiryDays { get; set; }
        [Display(Name = "Trạng thái")]
        public bool IsRemoved { get; set; }
        [Display(Name = "Ngày cập nhật")]
        public DateTime DateUpdate { get; set; }
        public long? UserIdUpdate { get; set; }
        [ForeignKey("UserIdUpdate")]
        [Display(Name = "Ngày cập nhật")]
        public ApplicationUser? UserUpdate { get; set; }

        public Voucher()
        {

        }
        public Voucher(Voucher model)
        {
            this.VoucherName = model.VoucherName;
            this.VoucherType = model.VoucherType;
            this.DiscountType = model.DiscountType;
            this.DiscountValue = model.DiscountValue;
            this.ExpiryDays = model.ExpiryDays;
            this.Code = model.Code.ToUpper();
            this.DateUpdate = DateTime.UtcNow.ToTimeZone();
            this.MaxDiscountAmount= model.MaxDiscountAmount;
            this.MinBillAmount  = model.MinBillAmount;
        }
    }
}
