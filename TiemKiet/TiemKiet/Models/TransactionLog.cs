using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using TiemKiet.Data;

namespace TiemKiet.Models
{
    public class TransactionLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public long? UserIdCustomer { get; set; }
        [ForeignKey("UserIdCustomer")]
        public ApplicationUser? UserCustomer { get; set; }
        [DisplayName("Tổng tiền thanh toán")]
        public double TotalPrice { get; set; }
        [DisplayName("Số tiền được giảm giá")]
        public double DiscountPrice { get; set; }
        public DateTime DateTimePayment { get; set; }
        public long? UserIdStaff { get; set; }
        [ForeignKey("UserIdStaff")]
        public ApplicationUser? UserStaff { get; set; }
        [DisplayName("Số điểm ban đầu (Điểm tích đổi voucher)")]
        public double PointOld { get; set; }
        [DisplayName("Số điểm được cộng (Điểm tích đổi voucher)")]
        public double PointNew { get; set; }
        [DisplayName("Số điểm tích lũy ban đầu (Phân hạng)")]
        public double ScroreOld { get; set; }
        [DisplayName("Số điểm tích lũy được cộng (Phân hạng)")]
        public double ScroreNew { get; set; }

    }
}
