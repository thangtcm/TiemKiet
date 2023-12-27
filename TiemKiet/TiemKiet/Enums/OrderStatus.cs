using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TiemKiet.Enums
{
    public enum OrderStatus
    {
        [Display(Name = "Đã hủy")]
        Canncel = 0,
        [Display(Name = "Đang chờ xác nhận")]
        WaitingConfirm = 1,
        [Display(Name = "Đang chuẩn bị")]
        Preparing = 2,
        [Display(Name = "Đang đi giao hàng")]
        Delivering = 3,
        [Display(Name = "Hoàn thành")]
        Complete = 4,
    }
}
