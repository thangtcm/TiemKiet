using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace TiemKiet.Helpers
{
    public static class CallBack
    {
        public static bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static double ExtractDistanceValue(string distanceText)
        {
            // Sử dụng biểu thức chính quy để tìm các con số trong chuỗi
            Match match = Regex.Match(distanceText, @"(\d+(\.\d+)?)");

            if (match.Success)
            {
                // Lấy giá trị double từ phần tử Match
                if (double.TryParse(match.Groups[1].Value, out double result))
                {
                    return result;
                }
            }

            // Trả về giá trị mặc định hoặc xử lý nếu không trích xuất được giá trị
            return 0.0;
        }

        public static DateTime ToTimeZone(this DateTime dateTime, string timeZoneId = "SE Asia Standard Time")
        {
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZone);
        }

        public static string GetEnumDisplayName(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null)
                return "";
            var displayAttribute = (DisplayAttribute?)Attribute.GetCustomAttribute(
                fieldInfo,
                typeof(DisplayAttribute)
            );

            string displayName = displayAttribute?.Name ?? value.ToString();


            return displayName;
        }

        public static string GetRankName(double score)
        {
            var rank = string.Empty;
            if (score < 1000)
                rank = "Chưa phân hạng";
            else if (score >= 1000 && score < 2000)
                rank = "Đồng";
            else if (score >= 2000 && score < 4000)
                rank = "Bạc";
            else if (score >= 4000 && score < 8000)
                rank = "Vàng";
            else
                rank = "Kim Cương";
            return rank;
        }

        public static string GetBackGroundRank(double score)
        {
            return score switch
            {
                >= 1000.0 and < 2000.0 => "background-image: url('https://firebasestorage.googleapis.com/v0/b/tiemkiet-aa7d7.appspot.com/o/Images%2Fz4930062184432_8198106af4be6073b7bac8cec5499833.jpg?alt=media&token=0c0fc379-390b-4334-a8eb-219de279ba74')",
                >= 2000.0 and < 4000.0 => "background-image: url('https://firebasestorage.googleapis.com/v0/b/tiemkiet-aa7d7.appspot.com/o/Images%2Frank-sliver.jpg?alt=media&token=fefaf954-1bb8-480a-94e2-5cb4cca61338')",
                >= 4000.0 and < 8000.0 => "background-image: url('https://firebasestorage.googleapis.com/v0/b/tiemkiet-aa7d7.appspot.com/o/Images%2Fz4929876123487_13d097f8d629bf779879bfcbbb1ec235.jpg?alt=media&token=b30d46ef-0b95-4fd3-9b6b-02617b54d1b9')",
                >= 8000.0 => "background-image: url('https://firebasestorage.googleapis.com/v0/b/tiemkiet-aa7d7.appspot.com/o/Images%2Fz4929876123487_13d097f8d629bf779879bfcbbb1ec235.jpg?alt=media&token=b30d46ef-0b95-4fd3-9b6b-02617b54d1b9')",
                _ => "background: #ffaf75;",
            };
        }    

        public static string GetClassProgress(double score)
        {
            return score switch
            {
                >= 1000.0 and < 2000.0 => "bg-copper",
                >= 2000.0 and < 4000.0 => "bg-sliver",
                >= 4000.0 and < 8000.0 => "bg-gold",
                >= 8000.0 => "bg-diamond",
                _ => "bg-normal",
            };
        }

        public static DateTime ConvertStringToDateTime(string dateStr)
        {
            string[] formats = { "dd/MM/yyyy", "dd/M/yyyy", "d/M/yyyy", "d/MM/yyyy",
                    "dd/MM/yy", "dd/M/yy", "d/M/yy", "d/MM/yy", "dd-MM-yyyy", "dd-M-yyyy", "d-M-yyyy", "d-MM-yyyy",
                    "dd-MM-yy", "dd-M-yy", "d-M-yy", "d-MM-yy"};
            if (DateTime.TryParseExact(dateStr, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                return date;
            }
            return new DateTime();
        }
        public static double GetDiscount(double score) //tỉ lệ giảm giá
        {
            switch(score)
            {
                case >= 1000.0 and < 2000.0:{
                        return 5.0;
                }
                case >= 2000.0 and < 4000.0:
                {
                    return 10.0;
                }
                case >= 4000.0 and < 8000.0:
                {
                    return 15.0;
                }
                case >= 8000.0 :
                    return 25.0;
                default:
                    return 0.0;
            }    
        }
    }
}
