using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TiemKiet.Data;

namespace TiemKiet.Models
{
    public class Banner
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UrlBannerPC { get; set; }
        public string UrlBannerMobile { get; set; }
        public DateTime DatePublish { get; set; }
        public long UserId { get; set; }
        public ApplicationUser UserPublish { get; set; }
    }
}
