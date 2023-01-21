namespace Survey.Domain.Services.IdentityService.Requests
{
    public class UserLoginRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
