﻿namespace Survey.Domain.Services.IdentityService.Requests
{
    public class UserRegistrationRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Role { get; set; }
    }
}