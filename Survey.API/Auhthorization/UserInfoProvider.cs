using System.Security.Claims;

namespace Survey.API.Auhthorization
{
    public static class UserInfoProvider
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
