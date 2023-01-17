namespace Survey.Domain.Services.IdentityService.Requests
{
    public class UserRegistrationRequestDto
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
