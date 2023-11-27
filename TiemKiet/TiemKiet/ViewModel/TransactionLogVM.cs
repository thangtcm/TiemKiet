using System.ComponentModel;

namespace TiemKiet.ViewModel
{
    public class TransactionLogVM
    {
        public int TransactionId { get; set; }
        public double TotalPrice { get; set; }
        public double DiscountPrice { get; set; }
        public DateTime DateTimePayment { get; set; }
        public double PointOld { get; set; }
        public double PointNew { get; set; }
        public double ScroreOld { get; set; }
        public double ScroreNew { get; set; }
    }
}
