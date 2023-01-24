
namespace Survey.xIntegrationTests.Tests
{
    public class IdentityTests : IdentityFixtures
    {
        private string Email = "test@test.com";
        private string Password = "12345678";


        public IdentityTests(WebApplicationFactory<Program> factory) : base(factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task GetAll_ShouldReturnBadRequest_ForAnonymousUsers()
        {
            var response = await _client.GetAsync("/api/companies");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task LoggedInUser_ShouldReturnOk()
        {
            await using (await CreateUserScope(Role.Admin))
            {
                UserLoginRequest loggedInInfo = new()
                {
                    Email = Email,
                    Password = Password,
                };

                var response = await LoginUser(loggedInInfo, _client);
                var responseString = await response.Content.ReadAsStringAsync();
                var authSuccessResponse = JsonConvert.DeserializeObject<AuthSuccessResponse>(responseString);

                Assert.Multiple(() =>
                {
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                    Assert.NotNull(authSuccessResponse);
                    Assert.NotNull(authSuccessResponse.Token);
                    Assert.NotNull(authSuccessResponse.RefreshToken);
                });
            }
        }

        [Fact]
        public async Task RefreshToken_ShouldReturnBadRequest()
        {
            await using (await CreateUserScope(Role.Admin))
            {
                UserLoginRequest loggedInInfo = new()
                {
                    Email = Email,
                    Password = Password,
                };

                var response = await LoginUser(loggedInInfo, _client);
                var responseString = await response.Content.ReadAsStringAsync();
                var authSuccessResponse = JsonConvert.DeserializeObject<AuthSuccessResponse>(responseString);

                RefreshTokenRequest request = new()
                {
                    Token = authSuccessResponse.Token,
                    RefreshToken = authSuccessResponse.RefreshToken,
                };

                var refreshResponse = await RefreshToken(request);
                var tokenResponse = await refreshResponse.Content.ReadAsStringAsync();
                var tokenAuthFailResponse = JsonConvert.DeserializeObject<AuthFailedResponse>(tokenResponse);
                string expected = "This refresh token has not expired yet";

                Assert.Multiple(() =>
                {
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                    Assert.NotNull(authSuccessResponse);
                    Assert.NotNull(authSuccessResponse.Token);
                    Assert.NotNull(authSuccessResponse.RefreshToken);

                    Assert.Equal(HttpStatusCode.BadRequest, refreshResponse.StatusCode);
                    Assert.NotNull(tokenAuthFailResponse);
                    Assert.NotNull(tokenAuthFailResponse.Errors);
                    Assert.Equal(tokenAuthFailResponse.Errors.First(), expected);

                });
            }
        }

        [Fact]
        public async Task Login_ShouldReturnBadRequest_LoginFails()
        {
            await using (await CreateUserScope(Role.Admin))
            {
                UserLoginRequest loggedInInfo = new()
                {
                    Email = Email,
                    Password = "wrongpassword",
                };

                var response = await LoginUser(loggedInInfo, _client);
                var responseString = await response.Content.ReadAsStringAsync();
                var authFailResponse = JsonConvert.DeserializeObject<AuthFailedResponse>(responseString);

                string expected = "Email/password combination is wrong";

                Assert.Multiple(() =>
                {
                    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                    Assert.NotNull(authFailResponse);
                    Assert.Equal(authFailResponse.Errors.First(), expected);
                });
            }
        }
    }
}