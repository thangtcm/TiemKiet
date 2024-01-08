using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using TiemKiet.Enums;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace TiemKiet.Helpers
{
    public class AuthorizeWithMessageAttribute : TypeFilterAttribute
    {
        public AuthorizeWithMessageAttribute() : base(typeof(AuthorizeWithMessageFilter))
        {
        }
    }

    public class AuthorizeWithMessageFilter : IAuthorizationFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthorizeWithMessageFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (_httpContextAccessor.HttpContext == null || !_httpContextAccessor.HttpContext.User?.Identity?.IsAuthenticated == true)
            {
                // HttpContext is null or user is not authenticated, handle accordingly
                // For example, redirect to login page or set TempData for a later request
                if (_httpContextAccessor.HttpContext != null)
                {
                    _httpContextAccessor.HttpContext.Session.SetString("ToastrMessage", "Bạn cần đăng nhập để thực hiện tính năng này.");
                    _httpContextAccessor.HttpContext.Session.SetString("ToastrMessageType", ToastrMessageType.Error.ToString().ToLower());
                }

                // Chuyển hướng đến trang đăng nhập
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
        }
    }
}
