using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TiemKiet.Data;
using TiemKiet.Enums;

namespace TiemKiet.Models
{
    public class ProductHome
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public ProductHomeType productHomeType { get; set; }
        public long UserUpdateId { get; set; }
        [ForeignKey("UserUpdateId")]
        public ApplicationUser? UserUpdate { get; set; }
        public DateTime DatePublish { get; set; }
    }
}
