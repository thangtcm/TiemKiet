using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TiemKiet.Data;

namespace TiemKiet.Models
{
    public class Feedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Display(Name ="Tiêu đề")]
        public string Title { get; set; }
        [Display(Name = "Nội dung")]
        public string Content { get; set; }
        [Display(Name = "Thời gian feedback")]
        public DateTime DateFeedback { get; set; }
        public long UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<ImageModel>? ImageLst { get; set; }
    }
}
