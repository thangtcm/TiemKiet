using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TiemKiet.Data;
using TiemKiet.ViewModel;

namespace TiemKiet.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DisplayName("Tên sản phẩm")]
        public string? ProductName { get; set; }
        [DisplayName("Giá sản phẩm")]
        public double ProductPrice { get; set; }
        [DisplayName("Mô tả sản phẩm")]
        public string? ProductDescription { get; set; }
        [DisplayName("Phần trăm giảm giá")]
        public double ProductSale { get; set; }
        [DisplayName("Ảnh")]
        public ImageModel? ProductImg {get; set; }
        [Display(Name = "Ngày tạo")]
        public DateTime DateCreate { get; set; }
        [Display(Name = "Người tạo")]
        public long? UserIdCreate { get; set; }
        [ForeignKey("UserIdCreate")]
        [Display(Name = "Người tạo")]
        public ApplicationUser? UserCreate { get; set; }
        [Display(Name = "Ngày cập nhật")]
        public DateTime DateUpdate { get; set; }
        [Display(Name = "Người cập nhật")]
        public long? UserIdUpdate { get; set; }
        [ForeignKey("UserIdUpdate")]
        [Display(Name = "Ngày cập nhật")]
        public ApplicationUser? UserUpdate { get; set; }
        [Display(Name = "Ngày xóa")]
        public DateTime DateRemove { get; set; }
        [Display(Name = "Người xóa")]
        public long? UserIdRemove { get; set; }
        [ForeignKey("UserIdRemove")]
        [Display(Name = "Người xóa")]
        public ApplicationUser? UserRemove { get; set; }
        [Display(Name = "Trạng thái")]
        public bool IsRemoved { get; set; }
        public int BranchId { get; set; }
        [ForeignKey("BranchId")]
        public Branch? Branch { get; set; }
        public Product() { }
    }
}
