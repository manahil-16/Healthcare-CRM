namespace HealthcareCRM.Helpers
{
    // Checks if user is logged in via session
    public static class AuthHelper
    {
        // Check if user is logged in
        public static bool IsAuthenticated(IHttpContextAccessor accessor)
        {
            return accessor.HttpContext?.Session.GetInt32("UserId") is not null;
        }

        // Get current user role
        public static string GetRole(IHttpContextAccessor accessor)
        {
            return accessor.HttpContext?
                .Session.GetString("UserRole") ?? "Staff";
        }

        // Check if current user is Admin
        public static bool IsAdmin(IHttpContextAccessor accessor)
        {
            return GetRole(accessor) == "Admin";
        }
    }
}