﻿namespace Survey.Infrastructure.Entities.JwtRelated
{
    public class AuthenticationResult
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public bool Success { get; set; }

        public IEnumerable<string> ErrorMessages { get; set; }
    }
}
