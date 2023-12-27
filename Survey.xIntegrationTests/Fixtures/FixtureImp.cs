using Survey.API.DTOs.AnwserBlockDtos;
using Survey.API.DTOs.AnwserDtos;
using Survey.API.DTOs.Company;
using Survey.API.DTOs.QuestionDtos;
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
        public string SecondUserEmail = "test@test.com";

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

            return new UserScope(this, requestDto.Email);
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

        public async ValueTask DeleteUserAsync(string email)
        {
            var identityEndpoints = new IdentityClients();
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

            var response = await PostAsync(endpoint.GetAllOrCreateCompany, companyInfo, client);
            var company = JsonConvert.DeserializeObject<CompanyDto>(await response.Content.ReadAsStringAsync());

            if (company == null)
            {
                return null;
            }

            return company;
        }

        public async Task<SurveyDto> CreateSurvey(HttpClient client, int companyId)
        {
            var surveyEndpoint = new SurveyClients(companyId);

            var createdSurvey = new SurveyCreationDto()
            {
                SurveyName = "TestSurveyName",
                IsActive = true,
            };

            var surveyResponse = await PostAsync(surveyEndpoint.CreateOrGetAllSurvey, createdSurvey, client);
            var survey = JsonConvert.DeserializeObject<SurveyDto>(await surveyResponse.Content.ReadAsStringAsync());

            if (survey == null)
                return null;
            else
                return survey;
        }

        public async Task<AnwserBlockBasicInfoDto> CreateAnswerBlock(HttpClient client, int companyId, int surveyId, int answerBlockId = 0)
        {
            var answerBlock = new AnwserBlockForCreationDto()
            {
                AnwserBlockName = "WebStorage",
                CodeOfAnwserBlock = 1,
                BlockType = "Texts"
            };
            var answerBlockEndpoints = new AnswerBlockClients(companyId, surveyId, answerBlockId);

            var answerBlockResponse = await PostAsync(answerBlockEndpoints.GetAllOrPostAnswerBlock, answerBlock, client);
            var answerBlockCreated = JsonConvert.DeserializeObject<AnwserBlockBasicInfoDto>(await answerBlockResponse.Content.ReadAsStringAsync());

            if (answerBlockCreated == null)
            {
                return null;
            }

            return answerBlockCreated;
        }

        public async Task<QuestionBasicInfoDto> CreateQuestionAsync(HttpClient client, int companyId, int surveyId, int answerBlockId)
        {
            var questionEndpoint = new QuestionClients(companyId, surveyId);

            var question = new QuestionForCreationDto()
            {
                AnwserBlockID = answerBlockId,
                Code = Guid.NewGuid().ToString(),
                QuestionText = "Extraordinary question?",
                QuestionType = "Text",
            };

            var questionResponse = await PostAsync(questionEndpoint.GetAllOrPost, question, client);
            var createdQuestion = JsonConvert.DeserializeObject<QuestionBasicInfoDto>(await questionResponse.Content.ReadAsStringAsync());

            if (createdQuestion == null) { return null; }
            return createdQuestion;
        }

        public async Task<AnswerBasicInfoDto> CreateAnswer(HttpClient client, int companyId, int answerBlockId)
        {
            var answerEndpoints = new AnswerClients(companyId, answerBlockId);

            var answer = new AnswerForCreationDto()
            {
                AnwserText = "CreativeAnswerToQuestion",
            };

            var answerResponse = await PostAsync(answerEndpoints.PostGetAll, answer, client);
            var answerCreated = JsonConvert.DeserializeObject<AnswerBasicInfoDto>(await answerResponse.Content.ReadAsStringAsync());
            if (answerCreated == null) return null;
            return answerCreated;
        }

        public async Task<HttpClient> CreateAndAuthorizeSecondUser()
        {
            var factory = new WebApplicationFactory<Program>();
            var secondClient = factory.CreateClient();
            var identityEndpoints = new IdentityClients(Role.Admin);
            var requestDto = new UserRegistrationRequestDto
            {
                Email = "test@test.com",
                FirstName = "Test",
                LastName = "User",
                Password = "12345678"
            };

            var bodyRegistration = CreateJsonContent(requestDto);
            await secondClient.PostAsync(identityEndpoints.Register, bodyRegistration);

            UserLoginRequest loggedInInfo = new()
            {
                Email = "test@test.com",
                Password = "12345678",
            };

            IdentityClients endpoint = new();

            var httpContent = CreateJsonContent(loggedInInfo);
            var response = await secondClient.PostAsync(endpoint.Login, httpContent);

            var responseString = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<AuthSuccessResponse>(responseString);
            secondClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

            return secondClient;
        }
    }
}
