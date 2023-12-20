using Newtonsoft.Json;
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
        [JsonIgnore]
        public double Percent { get; set; }
        [JsonIgnore]
        public string? RankName { get; set; }
        public double? Point { get; set; }
        public virtual ICollection<string>? Roles { get; set; }
        public virtual ICollection<string>? Tokens { get; set; }
        public UserInfoVM() { }

        public UserInfoVM(ApplicationUser user)
        {
            this.FullName = user.FullName;
            this.NumberPhone = user.PhoneNumber;
            this.UserId = user.Id;
            this.Gender = user.Gender;
            this.Score = user.Score;
            this.Point = user.Point;
            switch (user.Score)
            {
                case >= 1000.0 and < 2000.0:
                {
                    this.RankName = "Đồng";
                    this.Percent = (100 / 2000) * user.Score;
                    break;
                }
                case >= 2000.0 and < 4000.0:
                {
                    this.RankName = "Bạc";
                    this.Percent = (100 / 4000.0) * user.Score;
                    break;
                }
                case >= 4000.0 and < 8000.0:
                {
                    this.RankName = "Vàng";
                    this.Percent = (100 / 8000.0) * user.Score;
                    break;
                }
                case >= 8000.0:
                {
                    this.RankName = "Kim Cương";
                    this.Percent = 100;
                    break;
                }
                default:
                {
                    this.RankName = "Mới";
                    this.Percent = (100 / 1000) * user.Score;
                    break;
                }
            }
        }

        public UserInfoVM(ApplicationUser user, List<string> roles)
        {
            this.FullName = user.FullName;
            this.NumberPhone = user.PhoneNumber;
            this.Birthday = user.Birthday.ToString("dd/MM/yyyy");
            this.UserId = user.Id;
            this.Roles = roles;
            this.Score = user.Score;
            this.Point = user.Point;
        }

        public UserInfoVM(ApplicationUser user, List<string> roles, List<string> tokens)
        {
            this.FullName = user.FullName;
            this.NumberPhone = user.PhoneNumber;
            this.Birthday = user.Birthday.ToString("dd/MM/yyyy");
            this.UserId = user.Id;
            this.Roles = roles;
            this.Gender = user.Gender;
            this.Score = user.Score;
            this.Point = user.Point;
            this.Tokens = tokens;
        }
        }
    }
