using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using TiemKiet.Data;

namespace TiemKiet.Models
{
    public class Province
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Display(Name = "Thành phố/Tỉnh")]
        public string? CityName { get; set; }
        [Display(Name = "Tên ngắn Thành phố/Tỉnh")]
        public string? CityNameShort { get; set; }
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
        public int CountryId { get; set; }
        [ForeignKey(nameof(CountryId))]
        public Country? Country { get; set; }
        public virtual ICollection<District>? Districts { get;}
    }
}
