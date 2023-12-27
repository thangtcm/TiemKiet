using Newtonsoft.Json;
using TiemKiet.Data;
using TiemKiet.Models;

namespace TiemKiet.Models.ViewModel
{
    public class VoucherUserInfoVM
    {
        public int VoucherUserId { get; set; }
        public VoucherInfoVM Voucher { get; set; }
        [JsonIgnore]
        public UserInfoVM UserClaim { get; set; }
        public string RedeemedDate { get; set; }
        public string ExpiryDate { get; set; }

        public VoucherUserInfoVM() { }

        public VoucherUserInfoVM(VoucherUser model)
        {
            VoucherUserId = model.Id;
            Voucher = model.Voucher is null ? new VoucherInfoVM() : new VoucherInfoVM(model.Voucher);
            UserClaim = model.UserClaim is null ? new UserInfoVM() : new UserInfoVM(model.UserClaim);
            RedeemedDate = model.RedeemedDate.ToString("HH:mm dd-MM-yyyy");
            ExpiryDate = model.ExpiryDate.ToString("HH:mm dd-MM-yyyy");
        }
    }
}
