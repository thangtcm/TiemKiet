using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TiemKiet.Enums
{
    public enum DiscountType
    {
        [Display(Name = "Giảm giá theo phần trăm")]
        Percentage = 0,
        [Display(Name = "Giảm giá theo số tiền")]
        FixedAmount = 1,
    }
}
