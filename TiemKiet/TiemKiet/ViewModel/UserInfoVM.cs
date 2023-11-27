using System.ComponentModel.DataAnnotations;
using TiemKiet.Data;
using TiemKiet.Enums;
using TiemKiet.Helpers;

namespace TiemKiet.ViewModel
{
    public class UserInfoVM
    {
        public long? UserId { get; set; }
        [Phone]
        public string NumberPhone { get; set; }
        public string? FullName { get; set; }
        public string? Password { get; set; }
        public string? PasswordOld { get; set; }
        public Gender? Gender { get; set; }
        public string? Birthday { get; set; }
        public double? Score { get; set; }
        public double? Point { get; set; }
        public virtual ICollection<string>? Roles { get; set; }
        public UserInfoVM() { }

        public UserInfoVM(ApplicationUser user)
        {
            this.FullName = user.FullName;
            this.NumberPhone = user.PhoneNumber;
            this.UserId = user.Id;
            this.Gender = user.Gender;
        }

        public UserInfoVM(ApplicationUser user, List<string> roles)
        {
            this.FullName = user.FullName;
            this.NumberPhone = user.PhoneNumber;
            this.Birthday = user.Birthday.ToString("dd/MM/yyyy");
            this.UserId = user.Id;
            this.Roles = roles;
        }
    }
}
