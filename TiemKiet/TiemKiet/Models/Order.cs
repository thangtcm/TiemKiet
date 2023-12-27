using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using TiemKiet.Data;
using TiemKiet.Enums;
using TiemKiet.Helpers;
using TiemKiet.Models.ViewModel;

namespace TiemKiet.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long? UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
        public long? StaffId { get; set; }
        [ForeignKey("StaffId")]
        public ApplicationUser? StaffUser { get; set; }
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
        public string FullName { get; set; }
        [Display(Name ="Trạng thái đơn hàng")]
        public OrderStatus Status { get; set; }
        [Display(Name = "Số điện thoại")]
        [Phone]
        public string NumberPhone { get; set; }
        public int BranchId { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name ="Ngày tạo")]
        public DateTime DateCreate { get; set; }
        [Display(Name = "Ngày Update")]
        public DateTime DateUpdate { get; set; }
        public DateTime DatePreparing { get; set; }
        [Display(Name = "Ghi chú")]
        public string NoteShip { get; set; }
        public double LatCustomer { get; set; }
        public double LongCustomer { get; set; }
        public string ListVoucher { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }
        public Order() { }
        public Order(OrderInfoVM model, long StaffId)
        {
            this.Id = model.OrderId;
            this.FullName = model.FullName;
            this.Address = model.Address;
            this.GrandTotal = model.GrandTotal;
            this.Discount = model.Discount;
            this.Shipping = model.Shipping;
            this.DiscountShip = model.DiscountShip;
            this.BranchId = model.BranchId;
            this.DiscountRank = model.DiscountRank;
            this.DiscountEvent = model.DiscountEvent;
            this.NumberPhone = model.NumberPhone;
            this.Status = model.Status ?? OrderStatus.WaitingConfirm;
            this.NoteShip = model.NoteShip;
            this.UserId = model.UserId;
            this.StaffId = StaffId;
            this.LatCustomer = model.LatCustomer;
            this.LongCustomer = model.LongCustomer;
            this.ListVoucher = (model.VoucherList is null || model.VoucherList.Count == 0) ? "" : string.Join(", ", model.VoucherList);
        }
    }
}
