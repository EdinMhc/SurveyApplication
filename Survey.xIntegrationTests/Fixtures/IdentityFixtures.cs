using Microsoft.AspNetCore.Mvc.Testing;
using Survey.Domain.Services.IdentityService.Requests;
using Survey.xIntegrationTests.Clients;

namespace Survey.xIntegrationTests.Fixtures
{


    public class IdentityFixtures : FixtureImp
    {

        public IdentityFixtures(WebApplicationFactory<Program> factory) : base(factory)
        {
        }

        public async Task<HttpResponseMessage> LoginUser(UserLoginRequest request)
        {
            IdentityClients endpoint = new();

            var httpContent = CreateJsonContent(request);
            var response = await _client.PostAsync(endpoint.Login, httpContent);
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
