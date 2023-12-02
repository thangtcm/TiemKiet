using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TiemKiet.Enums;
using TiemKiet.ViewModel;

namespace TiemKiet.Data
{
    public class ApplicationUser : IdentityUser<long>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override long Id { get; set; }
        [DisplayName("Họ và tên")]
        public string? FullName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Birthday { get; set; }
        public Gender Gender { get; set; }
        public string? ImgAvatar { get; set; }
        public string? TokenNotify { get; set; }
        public string? TokenAPI { get; set; }
        public double Score { get; set; }
        public double Point { get; set; }

        public ApplicationUser() { }

    }
}
