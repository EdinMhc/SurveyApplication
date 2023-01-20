namespace Survey.Domain.Services.IdentityService.Options
{
    public class JwtSettings
    {
        public string Secret { get; set; }

        public TimeSpan TokenLifetime { get; set; }

        public string ValidAt { get; set; }

        public string Issuer { get; set; }
    }
}
