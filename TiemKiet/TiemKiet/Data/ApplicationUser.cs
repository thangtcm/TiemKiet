using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TiemKiet.Enums;

namespace TiemKiet.Data
{
    public class ApplicationUser : IdentityUser<long>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override long Id { get; set; }
        [DisplayName("Họ và tên")]
        public string? FullName { get; set; }
        public Gender Gender { get; set; }
        public string? ImgAvatar { get; set; }
        public string? TokenNotify { get; set; }
        public string? TokenAPI { get; set; }
    }
}
