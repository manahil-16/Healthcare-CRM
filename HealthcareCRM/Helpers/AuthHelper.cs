namespace HealthcareCRM.Helpers
{
    // Checks if user is logged in via session
    public static class AuthHelper
    {
        public static bool IsAuthenticated(IHttpContextAccessor accessor)
        {
            return accessor.HttpContext?.Session.GetInt32("UserId") is not null;
        }
    }
}
