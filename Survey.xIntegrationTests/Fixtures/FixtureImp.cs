using Survey.API.DTOs.Company;
using Survey.xIntegrationTests.Clients;
using System.Net.Http.Headers;
using System.Text;

namespace Survey.xIntegrationTests.Fixtures
{
    public class FixtureImp : IClassFixture<WebApplicationFactory<Program>>
    {
        public WebApplicationFactory<Program> _factory;
        public HttpClient _client;
        public string Email;
        public string Password = "12345678";

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
                Email = $"test@test" + Guid.NewGuid().ToString().Substring(0, 8) + ".com",
                FirstName = "Test",
                LastName = "User",
                Password = "12345678"
            };
            Email = requestDto.Email;
            var bodyRegistration = CreateJsonContent(requestDto);
            await _client.PostAsync(identityEndpoints.Register, bodyRegistration);

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

        public async Task<HttpResponseMessage> UpdateAsync(string endpoint, object convertJson, HttpClient client)
        {
            var json = CreateJsonContent(convertJson);

            return await client.PutAsync(endpoint, json);
        }

        public async Task<HttpResponseMessage> PostAsync(string endpoint, object convertJson, HttpClient client)
        {
            var json = CreateJsonContent(convertJson);
            return await client.PostAsync(endpoint, json);
        }

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

        public async Task AuthenticateUser(HttpClient client)
        {
            var token = await GetToken(client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<CompanyDto> CreateCompany(HttpClient client)
        {
            var endpoint = new CompanyClients();

            var companyInfo = new CompanyEditDto()
            {
                CompanyName = "EdinsCompany",
                Email = "test@test",
                Address = "testAddress No.1",
            };

            var response = await PostAsync(endpoint.GetOrCreateCompany, companyInfo, client);
            var company = JsonConvert.DeserializeObject<CompanyDto>(await response.Content.ReadAsStringAsync());

            if (company == null)
            {
                return null;
            }

            return company;
        }
    }
}
