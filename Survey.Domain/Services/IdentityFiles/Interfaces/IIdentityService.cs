using Survey.Infrastructure.Entities.JwtRelated;

namespace Survey.Domain.Services.IdentityService.Interfaces
{

    public interface IIdentityService
    {
        Task<AuthenticationResult> Register(Requests.UserRegistrationRequest request);

        Task<AuthenticationResult> Login(Requests.UserLoginRequest request);

        Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);

        Task<bool> DeleteUser(string email);
    }
}
