using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TiemKiet.Data;

namespace TiemKiet.Models
{
    public class BlogPost
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DisplayName("Tiêu đề")]
        public string Heading { get; set; }
        [DisplayName("Nội dung")]
        public string Content { get; set; }
        [DisplayName("Tiêu đề hiển thị ngắn")]
        public string Title { get; set; }
        [DisplayName("Nội dung hiển thị ngắn")]
        public string ShortDescription { get; set; }
        [DisplayName("Ảnh đại diện blog")]
        public string FeatheredImageUrl { get; set; }
        [DisplayName("Thời gian đăng")]
        public DateTime PublishedDate { get; set; }
        [DisplayName("Tác giả")]
        public string Author { get; set; }
        [DisplayName("Trạng thái (Ẩn / Hiển thị)")]
        public bool Visible { get; set; }
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
        [Display(Name = "Người xóa")]
        public ApplicationUser? UserRemove { get; set; }
        [Display(Name = "Trạng thái")]
        public bool IsRemoved { get; set; }
    }
}
