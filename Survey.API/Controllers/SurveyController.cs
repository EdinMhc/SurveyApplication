namespace Survey.API.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Survey.API.DTOs.SurveyDtos;
    using Survey.API.JwtRelated.Helpers;
    using Survey.Domain.Services.Helper_Admin;
    using Survey.Domain.Services.SurveyService;

    [Authorize]
    [ApiController]
    [Route("api/{companyId}/surveys")]
    public class SurveyController : Controller
    {
        private readonly IMapper mapper;
        private readonly ISurveyService surveyService;

        public SurveyController(IMapper mapper, ISurveyService surveyService)
        {
            this.mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            this.surveyService = surveyService ??
                throw new ArgumentNullException(nameof(surveyService));
        }

        // GET ALL
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpGet]
        public ActionResult<IEnumerable<SurveyBasicInfoDto>> GetAll(int companyId)
        {
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            var surveys = this.surveyService.GetAll(companyId, role, userId);
            return this.Ok(this.mapper.Map<List<SurveyBasicInfoDto>>(surveys));
        }

        // GET SPECIFIC
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpGet("{surveyId}")]
        public IActionResult Get(int surveyId, int companyId)
        {
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;
            var surveys = this.surveyService.GetById(surveyId, companyId, role, userId);
            return this.Ok(this.mapper.Map<SurveyBasicInfoDto>(surveys));
        }

        // CREATE DATA INSIDE TABLE
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpPost]
        public async Task<IActionResult> PostAsync(int companyId, [FromBody] SurveyForCreationDto surveyInfo)
        {
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            var mapped = this.mapper.Map<Survey.Infrastructure.Entities.Surveys>(surveyInfo);
            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            var survey = await this.surveyService.CreateAsync(mapped, companyId, role, userId);

            return this.Ok(this.mapper.Map<SurveyBasicInfoDto>(survey));
        }

        // DELETE DATA INSIDE TABLE
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpDelete("{surveyId}")]
        public async Task<IActionResult> Delete(int surveyId, int companyId)
        {
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            return this.Ok(await this.surveyService.DeleteAsync(surveyId, companyId, role, userId));
        }

        // UPDATE DATA INSIDE TABLE
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpPut("{surveyId}")]
        public async Task<IActionResult> PutAsync(int surveyId, int companyId, [FromBody] SurveyUpdateDto surveyInfo)
        {
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            var mapped = this.mapper.Map<Survey.Infrastructure.Entities.Surveys>(surveyInfo);
            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            var survey = await this.surveyService.UpdateAsync(mapped, surveyId, companyId, role, userId);
            return this.Ok(this.mapper.Map<SurveyBasicInfoDto>(survey));
        }
    }
}
