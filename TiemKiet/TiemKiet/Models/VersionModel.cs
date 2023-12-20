using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiemKiet.Models
{
    public class VersionModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? VersionName { get; set; }
        public string? UrlAndroidVersion { get; set; }
        public string? UrlIOSVersion { get; set; }
        public bool IsMaintenance { get; set; }
        public bool IsDeploy { get; set; }
    }
}
