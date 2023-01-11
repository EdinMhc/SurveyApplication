namespace Survey.API.JwtRelated.Helpers
{
    using System.Security.Claims;

    public static class ClaimsPrincipalExtensions
    {
        public static string GetEmail(this System.Security.Claims.ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.FindFirstValue(ClaimTypes.Email);

        public static string GetFirstName(this System.Security.Claims.ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.FindFirstValue(ClaimTypes.Name);

        public static string GetLastName(this System.Security.Claims.ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.FindFirstValue(ClaimTypes.Surname);

        public static string GetPhoneNumber(this System.Security.Claims.ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.FindFirstValue(ClaimTypes.MobilePhone);

        public static string GetUserId(this System.Security.Claims.ClaimsPrincipal claimsPrincipal)
           => claimsPrincipal.FindFirstValue("UserId");
    }

    public static class GeneralExtensions
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return string.Empty;
            }

            string? getId = httpContext.User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;

            return getId;
        }

        public static string GetRole(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return string.Empty;
            }

            var role = httpContext.User.Claims.Single(x => x.Type == ClaimTypes.Role).Value;

            return role;
        }
    }
}
