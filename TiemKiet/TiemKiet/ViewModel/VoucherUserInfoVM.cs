using Newtonsoft.Json;
using TiemKiet.Data;
using TiemKiet.Models;

namespace TiemKiet.ViewModel
{
    public class VoucherUserInfoVM
    {
        public int VoucherUserId { get; set; }
        public VoucherInfoVM Voucher { get; set; }
        [JsonIgnore]
        public UserInfoVM UserClaim { get; set; }
        public DateTime RedeemedDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        public VoucherUserInfoVM() { }

        public VoucherUserInfoVM(VoucherUser model)
        {
            this.VoucherUserId = model.Id;
            this.Voucher = model.Voucher is null ? new VoucherInfoVM() : new VoucherInfoVM(model.Voucher);
            this.UserClaim = model.UserClaim is null ? new UserInfoVM() : new UserInfoVM(model.UserClaim);
            this.RedeemedDate = model.RedeemedDate;
            this.ExpiryDate= model.ExpiryDate;
        }
    }
}
