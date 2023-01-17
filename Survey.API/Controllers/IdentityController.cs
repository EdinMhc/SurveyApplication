using Microsoft.AspNetCore.Mvc;
using Survey.Domain.Services.IdentityService.Contracts;
using Survey.Domain.Services.IdentityService.Interfaces;
using Survey.Domain.Services.IdentityService.Requests;
using Survey.Domain.Services.IdentityService.Responses;

namespace MinimalAPI.Controllers
{

    public class IdentityContoller : Controller
    {
        private readonly IIdentityService _identityService;

        public IdentityContoller(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register(Role role, [FromBody] UserRegistrationRequestDto request)
        {
            UserRegistrationRequest userRegisterRequest = new()
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = request.Password,
                Role = role.ToString(),
            };

            var authReposne = await _identityService.Register(userRegisterRequest);
            if (!authReposne.Success)
            {
                return BadRequest(new AuthFailedResponse
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

        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var authReposne = await _identityService.Login(request);
            if (!authReposne.Success)
            {
                return BadRequest(new AuthFailedResponse
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

        [HttpPost(ApiRoutes.Identity.Refresh)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var authReposne = await _identityService.RefreshTokenAsync(request.Token, request.RefreshToken);

            if (!authReposne.Success)
            {
                return BadRequest(new AuthFailedResponse
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
