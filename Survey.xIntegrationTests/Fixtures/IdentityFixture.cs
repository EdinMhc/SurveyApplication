using Survey.xIntegrationTests.Clients;

namespace Survey.xIntegrationTests.Fixtures
{
    public class IdentityFixture : FixtureImp
    {

        public IdentityFixture(WebApplicationFactory<Program> factory) : base(factory)
        {
        }

        public async Task<HttpResponseMessage> LoginUser(UserLoginRequest request, HttpClient client)
        {
            IdentityClients endpoint = new();

            var httpContent = CreateJsonContent(request);
            var response = await client.PostAsync(endpoint.Login, httpContent);
            return response;
        }

        public async Task<HttpResponseMessage> RefreshToken(RefreshTokenRequest request)
        {
            IdentityClients endpoint = new();

            var httpContent = CreateJsonContent(request);
            var response = await _client.PostAsync(endpoint.Refresh, httpContent);
            return response;
        }
    }
}
