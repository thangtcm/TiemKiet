using Microsoft.AspNetCore.Mvc.Rendering;
using TiemKiet.Data;

namespace TiemKiet.ViewModel
{
    public class EditUserViewModel
    {
        public ApplicationUser? User { get; set; }
        public IList<SelectListItem>? Roles { get; set; }
    }
}
