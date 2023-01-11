using Microsoft.AspNetCore.Authorization;
using Survey.Domain.CustomException;

namespace Survey.API.JwtRelated.Auhthorization.AnonymousUserHandler
{
    public class AllowAnonymousAuthorizationRequirement :
        AuthorizationHandler<AllowAnonymousAuthorizationRequirement>, IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AllowAnonymousAuthorizationRequirement requirement)
        {
            var user = context.User;
            var userIsAnonymous = user?.Identity == null || !user.Identities.Any(i => i.IsAuthenticated);
            // success if user IS anonymous
            if (userIsAnonymous)
            {
                throw new CustomException((ErrorResponseCode)155);
            }

            if (!userIsAnonymous)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}