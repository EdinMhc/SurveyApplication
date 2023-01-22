using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Survey.Domain.Services.IdentityService.Requests;
using Survey.xIntegrationTests.Clients;
using System.Text;

namespace Survey.xIntegrationTests.Fixtures
{
    public class FixtureImp : IClassFixture<WebApplicationFactory<Program>>, IAsyncDisposable
    {
        public readonly WebApplicationFactory<Program> _factory;
        public readonly HttpClient _client;

        public FixtureImp(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        public async Task<UserScope> CreateUserScope(Role role)
        {
            var identityEndpoints = new IdentityClients(role);
            var requestDto = new UserRegistrationRequestDto
            {
                Email = "test@test.com",
                FirstName = "Test",
                LastName = "User",
                Password = "12345678"
            };

            var bodyRegistration = CreateJsonContent(requestDto);
            var response = await _client.PostAsync(identityEndpoints.Register, bodyRegistration);

            return new UserScope(this, requestDto.Email, role);
        }

        public HttpContent CreateJsonContent(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        public async Task<HttpResponseMessage> PostAsync(string endpoint, HttpContent httpContent) =>
            await _client.PostAsync(endpoint, httpContent);

        public async Task<HttpResponseMessage> DeleteByPostAsync(string endpoint, HttpContent httpContent) =>
            await _client.PostAsync(endpoint, httpContent);

        public async ValueTask DeleteUserAsync(string email, Role role)
        {
            var identityEndpoints = new IdentityClients(role);
            var body = CreateJsonContent(email);
            await DeleteByPostAsync(identityEndpoints.Delete, body);
        }

        public async ValueTask DisposeAsync()
        {
            _client.Dispose();
        }
    }
}
