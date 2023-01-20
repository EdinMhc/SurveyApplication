namespace Survey.Domain.Services.IdentityService.Interfaces
{
    using Survey.Infrastructure.Entities.JwtRelated;

    public interface IIdentityService
    {
        Task<AuthenticationResult> Register(Requests.UserRegistrationRequest request);

        Task<AuthenticationResult> Login(Requests.UserLoginRequest request);

        Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);
    }
}
