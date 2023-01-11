namespace Survey.Domain.Services.IdentityService.Interfaces
{
    using Survey.Domain.Services.IdentityService.Requests;
    using Survey.Infrastructure.Entities.JwtRelated;

    public interface IIdentityService
    {
        Task<AuthenticationResult> Register(UserRegistrationRequest request);

        Task<AuthenticationResult> Login(UserLoginRequest request);

        Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);
    }
}
