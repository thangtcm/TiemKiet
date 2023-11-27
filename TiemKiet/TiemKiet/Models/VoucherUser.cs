using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TiemKiet.Data;

namespace TiemKiet.Models
{
    public class VoucherUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? VoucherId { get; set; }
        [ForeignKey("VoucherId")]
        public Voucher? Voucher { get; set; }
        public long? UserIdClaim { get; set; }
        [ForeignKey("UserIdClaim")]
        public ApplicationUser? UserClaim { get; set; }
        [DisplayName("Ngày nhận voucher")]
        public DateTime RedeemedDate { get; set; }
        [DisplayName("Ngày hết hạn")]
        public DateTime ExpiryDate { get; set; }
    }
}
