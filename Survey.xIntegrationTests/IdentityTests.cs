using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Survey.Domain.Services.IdentityService.Requests;
using Survey.Domain.Services.IdentityService.Responses;
using Survey.xIntegrationTests.Fixtures;
using System.Net;

namespace Survey.xIntegrationTests
{
    public class IdentityTests : IdentityFixtures
    {
        private string Email = "test@test.com";
        private string Password = "12345678";

        public IdentityTests(WebApplicationFactory<Program> factory) : base(factory) { }

        [Fact]
        public async Task GetAll_ShouldReturnBadRequest_ForAnonymousUsers()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync("/api/companies");

            // Assert
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

                var response = await LoginUser(loggedInInfo);
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

                var response = await LoginUser(loggedInInfo);
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
                    Assert.Equal(tokenAuthFailResponse.Errors.FirstOrDefault(), expected);

                });
            }
        }
    }
}