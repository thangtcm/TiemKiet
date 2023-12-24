using System.ComponentModel;
using TiemKiet.Helpers;
using TiemKiet.Models;

namespace TiemKiet.Models.ViewModel
{
    public class TransactionLogVM
    {
        public int TransactionId { get; set; }
        public double TotalPrice { get; set; }
        public double DiscountPrice { get; set; }
        public string DateTimePayment { get; set; }
        public double PointOld { get; set; }
        public double PointNew { get; set; }
        public double ScroreOld { get; set; }
        public double ScroreNew { get; set; }

        public TransactionLogVM() { }
        public TransactionLogVM(TransactionLog model)
        {
            TransactionId = model.Id;
            TotalPrice = model.TotalPrice;
            DiscountPrice = model.DiscountPrice;
            PointOld = model.PointOld;
            PointNew = model.PointNew;
            DateTimePayment = model.DateTimePayment.ToString("HH:mm dd/MM/yyyy");
            ScroreNew = model.ScroreNew;
            ScroreOld = model.ScroreOld;
        }
    }

}
