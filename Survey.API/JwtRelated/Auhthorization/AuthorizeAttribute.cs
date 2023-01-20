namespace Survey.API.JwtRelated.Auhthorization
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System.Security.Claims;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    //Attribute
    public class Authorize2Attribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly IList<string> roles;

        public Authorize2Attribute(params string[] roles)
        {
            roles = roles ?? new string[] { };
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
            {
                return;
            }

            // authorization
            var user = context.HttpContext.User;
            var userRole = context.HttpContext.User.Claims.Single(x => x.Type == ClaimTypes.Role).Value;

            if (userRole == null || (roles.Any() && !roles.Contains(userRole)))
            {
                // Role is not valid
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
