using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TiemKiet.Data;
using TiemKiet.Enums;

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
        [Display(Name = "Ngày tạo")]
        public DateTime DateCreate { get; set; }
        public long? UserIdCreate { get; set; }
        [ForeignKey("UserIdCreate")]
        [Display(Name = "Người tạo")]
        public ApplicationUser? UserCreate { get; set; }
        [Display(Name = "Ngày cập nhật")]
        public DateTime DateUpdate { get; set; }
        public long? UserIdUpdate { get; set; }
        [ForeignKey("UserIdUpdate")]
        [Display(Name = "Ngày cập nhật")]
        public ApplicationUser? UserUpdate { get; set; }
        [Display(Name = "Ngày xóa")]
        public DateTime DateRemove { get; set; }
        public long? UserIdRemove { get; set; }
        [ForeignKey("UserIdRemove")]
        public ApplicationUser UserRemove { get; set; }
    }
}
