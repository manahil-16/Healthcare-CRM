namespace HealthcareCRM.Helpers
{
    // Checks if user is logged in via session
    public static class AuthHelper
    {
        public static bool IsAuthenticated(IHttpContextAccessor accessor)
        {
            var token = accessor.HttpContext?
                .Session.GetString("jwt_token");
            return !string.IsNullOrEmpty(token);
        }
    }
}