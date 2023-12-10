using System.Diagnostics.Metrics;
using System.Globalization;
using System.Net.Mail;

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

        public static DateTime ToTimeZone(this DateTime dateTime, string timeZoneId = "SE Asia Standard Time")
        {
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZone);
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
