using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TiemKiet.Data;

namespace TiemKiet.Models
{
    public class ImageModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime PublishUpload { get; set; }
        public long UserIdUpload { get; set; }
        [ForeignKey("UserIdUpload")]
        public ApplicationUser UserUpload { get; set; }
    }
}
