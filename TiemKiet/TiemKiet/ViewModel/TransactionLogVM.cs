using System.ComponentModel;
using TiemKiet.Models;

namespace TiemKiet.ViewModel
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
            this.TransactionId = model.Id;
            this.TotalPrice = model.TotalPrice;
            this.DiscountPrice = model.DiscountPrice;
            this.PointOld = model.PointOld;
            this.PointNew = model.PointNew;
            this.DateTimePayment = model.DateTimePayment.ToString("hh:mm dd/MM/yyyy");
            this.ScroreNew= model.ScroreNew;
            this.ScroreOld= model.ScroreOld;
        }
    }

}
