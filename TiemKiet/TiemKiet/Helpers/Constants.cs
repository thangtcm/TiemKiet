namespace TiemKiet.Helpers
{
    public static class Constants
    {
        public const long ValueStartUser = 1000000000;
        public const string UserLatitude = "UserLatitude";
        public const string UserLongitude = "UserLongitude";
        public const string UserAddressCurrent = "UserAddressCurrent";
        public const string UserCart = "UserCart";

        public static class Roles
        {
            public const string Admin = "Administrator";
            public const string Staff = "Staff";
        }

        public static class Policies
        {
            public const string RequireAdmin = "RequireAdmin";
            public const string RequireStaff = "RequireStaff";
        }
    }
}
