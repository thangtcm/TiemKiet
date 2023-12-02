using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TiemKiet.Enums
{
    public enum ProductType
    {
        [Display(Name = "CÀ PHÊ/COFFEE")]
        Coffee = 1,
        [Display(Name = "CHOCOLATE")]
        Chocolate = 2,
        [Display(Name = "TRÀ/TEA")]
        Tea = 3,
        [Display(Name = "NƯỚC ÉP/JUICE")]
        Juice = 4,
        [Display(Name = "SINH TỐ/SMOOTHIE")]
        Smoothie = 5
    }
}
