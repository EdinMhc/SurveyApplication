namespace Survey.xIntegrationTests.Clients
{
    public class IdentityClients
    {
        public string Register { get; set; }
        public string Login { get; set; }
        public string Refresh { get; set; }
        public string Delete { get; set; }

        /// <summary>
        /// To register a user, specify role
        /// </summary>
        /// <param name="role"></param>
        public IdentityClients(Role role = Role.Admin)
        {
            Register = $"/api/v1/identity/register/{role}";
        }

        public IdentityClients()
        {
            Login = "/api/v1/identity/login";
            Refresh = "/api/v1/identity/refresh";
            Delete = "/api/v1/identity/delete";
        }
    }
}
