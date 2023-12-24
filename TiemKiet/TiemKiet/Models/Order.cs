using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using TiemKiet.Data;
using TiemKiet.Enums;

namespace TiemKiet.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public BigInteger Id { get; set; }
        public long? UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
        [Display(Name ="Tổng cộng")]
        public double GrandTotal { get; set; }
        [Display(Name ="Tổng giảm giá")]
        public double Discount { get; set; }
        [Display(Name = "Giảm giá hạng")]
        public double DiscountRank { get; set; }
        [Display(Name = "Giảm giá sự kiện")]
        public double DiscountEvent { get; set; }
        [Display(Name = "Giảm giá ship")]
        public double DiscountShip { get; set; }
        [Display(Name = "Tiền ship")]
        public double Shipping { get; set; }
        [Display(Name = "Họ và tên")]
        public double FullName { get; set; }
        [Display(Name ="Trạng thái đơn hàng")]
        public OrderStatus Status { get; set; }
        [Display(Name = "Số điện thoại")]
        [Phone]
        public string NumberPhone { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name ="Ngày tạo")]
        public DateTime DateCreate { get; set; }
        [Display(Name = "Ngày Update")]
        public DateTime DateUpdate { get; set; }
        [Display(Name = "Ghi chú")]
        public string Content { get; set; }
    }
}
