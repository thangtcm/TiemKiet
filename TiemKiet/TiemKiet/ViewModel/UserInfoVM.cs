using System.ComponentModel.DataAnnotations;
using TiemKiet.Data;

namespace TiemKiet.ViewModel
{
    public class UserInfoVM
    {
        public long? UserId { get; set; }
        public string UserName { get; set; }
        public string? Email { get; set; }
        public string? NumberPhone { get; set; }
        public string? FullName { get; set; }
        public string? Password { get; set; }
        public string? PasswordOld { get; set; }
        public double? Score { get; set; }
        public double? Point { get; set; }
        public virtual ICollection<string>? Roles { get; set; }
        public UserInfoVM() { }

        public UserInfoVM(ApplicationUser user)
        {
            this.FullName = user.FullName;
            this.UserName = user.UserName;
            this.Email = user.Email;
            this.NumberPhone = user.PhoneNumber;
            this.UserId = user.Id;
        }

        public UserInfoVM(ApplicationUser user, List<string> roles)
        {
            this.FullName = user.FullName;
            this.UserName = user.UserName;
            this.Email = user.Email;
            this.NumberPhone = user.PhoneNumber;
            this.UserId = user.Id;
            this.Roles = roles;
        }
    }
}
