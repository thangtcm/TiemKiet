using Microsoft.AspNetCore.Mvc.Rendering;

namespace TiemKiet.Helpers
{
    public static class ManageNavPages
    {
        public static string Home => "Home";
        public static string Product => "Product";
        public static string Blog => "Blog";
        public static string Restaurant => "Restaurant";
        public static string InformationProfile => "InformationProfile";
        public static string AddressBook => "AddressBook";
        public static string ChangePassword => "ChangePassword";


        public static string MainNavClass(ViewContext viewContext, string controllerName, string? actionName = null)
        {
            var currentController = viewContext.RouteData.Values["controller"] as string;
            var currentAction = viewContext.RouteData.Values["action"] as string;

            if (string.Equals(currentController, controllerName, StringComparison.OrdinalIgnoreCase) &&
                (actionName == null || string.Equals(currentAction, actionName, StringComparison.OrdinalIgnoreCase)))
            {
                return "active";
            }

            return "";
        }
    }
}
