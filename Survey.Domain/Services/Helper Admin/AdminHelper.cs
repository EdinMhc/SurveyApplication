namespace Survey.Domain.Services.Helper_Admin
{
    public static class AdminHelper
    {
        public const string Admin = "Admin";
        public const string SuperAdmin = "SuperAdmin";
    }

    public enum UserRole
    {
        Admin,
        SuperAdmin,
    }
}
