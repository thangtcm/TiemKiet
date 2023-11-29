using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiemKiet.Data
{
    public class ApplicationUserRole : IdentityUserRole<long>
    {
    }
}
