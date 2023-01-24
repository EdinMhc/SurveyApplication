using Survey.xIntegrationTests.Clients;
using System.Text;

namespace Survey.xIntegrationTests.Fixtures
{
    public class FixtureImp : IClassFixture<WebApplicationFactory<Program>>
    {
        public WebApplicationFactory<Program> _factory;
        public HttpClient _client;
        private string Email = "test@test.com";
        private string Password = "12345678";

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

        public async Task<string> GetToken(HttpClient client)
        {
            UserLoginRequest loggedInInfo = new()
            {
                Email = Email,
                Password = Password,
            };

            IdentityClients endpoint = new();

            var httpContent = CreateJsonContent(loggedInInfo);
            var response = await client.PostAsync(endpoint.Login, httpContent);
            var responseString = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<AuthSuccessResponse>(responseString);

            if (token != null)
            {
                return token.Token;
            }

            return string.Empty;
        }

        public HttpContent CreateJsonContent(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        public async Task<HttpResponseMessage> DeleteAsync(string endpoint, HttpClient client) =>
            await client.DeleteAsync(endpoint);

        public async Task<HttpResponseMessage> UpdateAsync(string endpoint, HttpContent httpContent, HttpClient client) =>
            await client.PutAsync(endpoint, httpContent);

        public async Task<HttpResponseMessage> PostAsync(string endpoint, HttpContent httpContent, HttpClient client) =>
            await client.PostAsync(endpoint, httpContent);

        public async Task<HttpResponseMessage> DeleteByPostAsync(string endpoint, HttpContent httpContent) =>
            await _client.PostAsync(endpoint, httpContent);

        public async Task<HttpResponseMessage> GetAsync(string endpoint, HttpClient client) =>
            await client.GetAsync(endpoint);

        public async Task<HttpResponseMessage> GetAllAsync(string endpoint, HttpClient client) =>
            await client.GetAsync(endpoint);

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
