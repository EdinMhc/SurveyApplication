using Survey.Domain.Services.IdentityService.Requests;

namespace Survey.xIntegrationTests.Clients
{
    public class IdentityClients
    {
        public string Register { get; set; }
        public string Login { get; set; }
        public string Refresh { get; set; }
        public string Delete { get; set; }

        public IdentityClients(Role role = Role.Admin)
        {
            Register = $"/api/v1/identity/register/{role}";
            Login = "/api/v1/identity/login";
            Refresh = "/api/v1/identity/refresh";
            Delete = "/api/v1/identity/delete";
        }
    }
}
