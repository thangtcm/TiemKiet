using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using TiemKiet.Data;
using TiemKiet.Enums;
using TiemKiet.Helpers;

namespace TiemKiet.Models.ViewModel
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
        [JsonIgnore]
        public double Percent { get; set; }
        [JsonIgnore]
        public string? RankName { get; set; }
        public double? Point { get; set; }
        public bool IsHasOrder { get; set; }
        public virtual ICollection<string>? Roles { get; set; }
        public virtual ICollection<string>? Tokens { get; set; }
        public UserInfoVM() { }

        public UserInfoVM(ApplicationUser user)
        {
            FullName = user.FullName;
            NumberPhone = user.PhoneNumber;
            UserId = user.Id;
            Gender = user.Gender;
            Score = user.Score;
            Point = user.Point;
            switch (user.Score)
            {
                case >= 1000.0 and < 2000.0:
                    {
                        RankName = "Đồng";
                        Percent = 100 / 2000 * user.Score;
                        break;
                    }
                case >= 2000.0 and < 4000.0:
                    {
                        RankName = "Bạc";
                        Percent = 100 / 4000.0 * user.Score;
                        break;
                    }
                case >= 4000.0 and < 8000.0:
                    {
                        RankName = "Vàng";
                        Percent = 100 / 8000.0 * user.Score;
                        break;
                    }
                case >= 8000.0:
                    {
                        RankName = "Kim Cương";
                        Percent = 100;
                        break;
                    }
                default:
                    {
                        RankName = "Mới";
                        Percent = 100 / 1000 * user.Score;
                        break;
                    }
            }
        }

        public UserInfoVM(ApplicationUser user, List<string> roles)
        {
            FullName = user.FullName;
            NumberPhone = user.PhoneNumber;
            Birthday = user.Birthday.ToString("dd/MM/yyyy");
            UserId = user.Id;
            Roles = roles;
            Score = user.Score;
            Point = user.Point;
        }

        public UserInfoVM(ApplicationUser user, List<string> roles, List<string> tokens)
        {
            FullName = user.FullName;
            NumberPhone = user.PhoneNumber;
            Birthday = user.Birthday.ToString("dd/MM/yyyy");
            UserId = user.Id;
            Roles = roles;
            Gender = user.Gender;
            Score = user.Score;
            Point = user.Point;
            Tokens = tokens;
        }
    }
}
