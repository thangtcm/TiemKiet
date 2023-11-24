using System.Net.Mail;

namespace TiemKiet.Helpers
{
    public static class CallBack
    {
        public static bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
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
    }
}
