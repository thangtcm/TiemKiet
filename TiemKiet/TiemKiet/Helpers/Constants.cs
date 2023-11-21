namespace TiemKiet.Helpers
{
    public static class Constants
    {
        public const long ValueStartUser = 1000000000;
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
