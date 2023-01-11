namespace MinimalAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Survey.Domain.Services.IdentityService.Contracts;
    using Survey.Domain.Services.IdentityService.Interfaces;
    using Survey.Domain.Services.IdentityService.Requests;
    using Survey.Domain.Services.IdentityService.Responses;

    public class IdentityContoller : Controller
    {
        private readonly IIdentityService identityService;

        public IdentityContoller(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            var authReposne = await this.identityService.Register(request);
            if (!authReposne.Success)
            {
                return this.BadRequest(new AuthFailedResponse
                {
                    Errors = authReposne.ErrorMessages,
                });
            }

            return this.Ok(new AuthSuccessResponse
            {
                Token = authReposne.Token,
                RefreshToken = authReposne.RefreshToken,
            });
        }

        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var authReposne = await this.identityService.Login(request);
            if (!authReposne.Success)
            {
                return this.BadRequest(new AuthFailedResponse
                {
                    Errors = authReposne.ErrorMessages,
                });
            }

            return this.Ok(new AuthSuccessResponse
            {
                Token = authReposne.Token,
                RefreshToken = authReposne.RefreshToken,
            });
        }

        [HttpPost(ApiRoutes.Identity.Refresh)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var authReposne = await this.identityService.RefreshTokenAsync(request.Token, request.RefreshToken);

            if (!authReposne.Success)
            {
                return this.BadRequest(new AuthFailedResponse
                {
                    Errors = authReposne.ErrorMessages,
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authReposne.Token,
                RefreshToken = authReposne.RefreshToken,
            });
        }
    }
}
