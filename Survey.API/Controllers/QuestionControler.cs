namespace Survey.API.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Survey.API.DTOs.QuestionDtos;
    using Survey.API.JwtRelated.Helpers;
    using Survey.Domain.Services.Helper_Admin;
    using Survey.Domain.Services.QuestionService;

    [Authorize]
    [ApiController]
    [Route("api/{companyId}/questions/{surveyId}")]
    public class QuestionControLler : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IQuestionService questionService;

        public QuestionControLler(IMapper mapper, IQuestionService questionService)
        {
            this.mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            this.questionService = questionService ??
                throw new ArgumentNullException(nameof(questionService));
        }

        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpGet]
        public ActionResult<IEnumerable<QuestionBasicInfoDto>> GetAll(int companyId, int surveyId)
        {
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;
            var surveys = this.questionService.GetAll(companyId, surveyId, role, userId);
            return this.Ok(this.mapper.Map<List<QuestionBasicInfoDto>>(surveys));
        }

        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpGet("{questionId}")]
        public IActionResult Get(int companyId, int surveyId, int questionId)
        {
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            var question = this.questionService.GetById(companyId, surveyId, questionId, role, userId);
            return this.Ok(this.mapper.Map<QuestionBasicInfoDto>(question));
        }

        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpPost]
        public async Task<IActionResult> PostAsync(int companyId, int surveyId, [FromBody] QuestionForCreationDto questionInfo)
        {
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            var mapped = this.mapper.Map<Survey.Infrastructure.Entities.Question>(questionInfo);
            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            var survey = await this.questionService.CreateAsync(mapped, companyId, surveyId, role, userId);
            return this.Ok(this.mapper.Map<Survey.API.DTOs.QuestionDtos.QuestionBasicInfoDto>(survey));
        }

        // DELETE: api/{companyId}/questions/5
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpDelete("{questionId}")]
        public async Task<IActionResult> Delete(int companyId, int surveyId, int questionId)
        {
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            return this.Ok(await this.questionService.DeleteAsync(companyId, surveyId, questionId, role, userId));
        }

        // PUT: api/companies/5
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpPut("{questionId}")]
        public async Task<IActionResult> PutAsync(int companyId, int surveyId, int questionId, [FromBody] QuestionUpdateDto questionInfo)
        {
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            var mapped = this.mapper.Map<Survey.Infrastructure.Entities.Question>(questionInfo);
            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            var survey = await this.questionService.UpdateAsync(mapped, companyId, surveyId, questionId, role, userId);
            return this.Ok(this.mapper.Map<Survey.API.DTOs.QuestionDtos.QuestionBasicInfoDto>(survey));
        }
    }
}
