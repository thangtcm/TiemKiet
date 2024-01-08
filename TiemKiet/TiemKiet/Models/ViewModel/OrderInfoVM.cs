using System.ComponentModel.DataAnnotations;
using TiemKiet.Enums;
using TiemKiet.Helpers;

namespace TiemKiet.Models.ViewModel
{
    public class OrderInfoVM
    {
        public long OrderId { get; set; }
        public double GrandTotal { get; set; }
        public double Discount { get; set; }
        public double DiscountRank { get; set; }
        public double DiscountEvent { get; set; }
        public double DiscountShip { get; set; }
        public double ShipTotal { get; set; }
        public string FullName { get; set; }
        public OrderStatus? Status { get; set; }
        [Phone]
        public string NumberPhone { get; set; }
        public string Address { get; set; }
        public string NoteShip { get; set; }
        public long UserId { get; set; }
        public double LatCustomer { get; set; }
        public double LongCustomer { get; set; }
        public string? DateCreate { get; set; }
        public string? DatePreparing { get; set; }
        public string? DateUpdate { get; set; }
        public double Distance { get; set; }
        public int BranchId { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
        public List<int>? VoucherList { get; set; } 
        public OrderInfoVM() {
            CartItems = new List<CartItem>();
        }
        public OrderInfoVM(Order model)
        {
            this.VoucherList = new List<int>();
            this.OrderId = model.Id;
            this.FullName = model.FullName;
            this.Address = model.Address;
            this.GrandTotal = model.GrandTotal;
            this.Discount = model.Discount;
            this.ShipTotal = model.ShipTotal;
            this.DiscountShip = model.DiscountShip;
            this.DiscountRank = model.DiscountRank;
            this.DiscountEvent = model.DiscountEvent;
            this.NumberPhone = model.NumberPhone;
            this.Status = model.Status;
            this.NoteShip = model.NoteShip;
            this.UserId = model.UserId is null ? 0 : model.UserId.Value;
            this.LatCustomer = model.LatCustomer;
            this.BranchId = model.BranchId;
            this.LongCustomer = model.LongCustomer;
            this.DateCreate = model.DateCreate.ToString("HH:mm");
            this.DateUpdate = model.DateUpdate.ToString("HH:mm");
            this.DatePreparing = model.DatePreparing.ToString("HH:mm");
            this.Distance = model.Distance;
            this.VoucherList = string.IsNullOrEmpty(model.ListVoucher) ? new List<int>() : model.ListVoucher.Split(", ")
                         .Select(int.Parse)
                         .ToList();
            if (model.OrderDetails != null)
            {
                CartItems = new List<CartItem>();
                foreach (var item in model.OrderDetails)
                {
                    if(item.Product != null)
                    {
                        CartItem cart = new(item.Product)
                        {
                            ProductId = item.ProductId.HasValue ? item.ProductId.Value : 0,
                            ProductHasIce = item.AddIce,
                            ProductNotes = item.Note,
                            ProductQuantity = item.Quantity,
                            ProductUpsize = item.UpSize,
                        };
                        CartItems.Add(cart);
                    }
                }
            }    
        }    
    }
}
