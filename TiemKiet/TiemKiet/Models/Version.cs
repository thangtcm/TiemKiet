using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiemKiet.Models
{
    public class Version
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? VersionName { get; set; }
        public string? UrlVersion { get; set; }
        public bool IsDeploy { get; set; }
    }
}
