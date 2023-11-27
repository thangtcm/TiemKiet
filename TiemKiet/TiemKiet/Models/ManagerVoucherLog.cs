using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TiemKiet.Data;

namespace TiemKiet.Models
{
    public class ManagerVoucherLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DisplayName("Người cấp voucher")]
        public long? UserIdGive { get; set; }
        [ForeignKey(nameof(UserIdGive))]
        public ApplicationUser? UserGive { get; set; }
        [DisplayName("Người nhận voucher")]
        public long? UserIdClaim { get; set; }
        [ForeignKey(nameof(UserIdClaim))]
        public ApplicationUser? UserClaim { get; set; }
        [DisplayName("Loại voucher")]
        public int? VoucherId { get; set; }
        [ForeignKey(nameof(VoucherId))]  
        
        public Voucher? Voucher { get; set; }
        public DateTime DateTimeGives { get; set; }
        public string? ReponseGive { get; set; }
    }
}
