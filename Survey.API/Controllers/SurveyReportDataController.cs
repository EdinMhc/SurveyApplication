namespace Survey.API.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Survey.API.DTOs.SurveyReportData;
    using Survey.API.JwtRelated.Helpers;
    using Survey.Domain.Services.Helper_Admin;
    using Survey.Domain.Services.SurveyReportDataService;

    [Authorize]
    [ApiController]
    [Route("api/{companyId}/surveyreportdata/{surveyId}/{surveyReportId}")]
    public class SurveyReportDataController : Controller
    {
        private readonly IMapper mapper;
        private readonly ISurveyReportDataService surveyReportDataService;

        public SurveyReportDataController(IMapper mapper, ISurveyReportDataService surveyReportDataService)
        {
            this.mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            this.surveyReportDataService = surveyReportDataService ??
                throw new ArgumentNullException(nameof(surveyReportDataService));
        }

        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpGet]
        public ActionResult<IEnumerable<SurveyReportDataBasicInfoDto>> GetAll(int companyId, int surveyId, int surveyReportId)
        {
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            var surveys = this.surveyReportDataService.GetAll(companyId, surveyId, surveyReportId, role, userId);
            return this.Ok(this.mapper.Map<List<SurveyReportDataBasicInfoDto>>(surveys));
        }

        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpGet("{respondentId}")]
        public IActionResult Get(int companyId, int surveyId, int surveyReportId, int respondentId)
        {
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            var surveys = this.surveyReportDataService.GetById(companyId, surveyId, surveyReportId, respondentId, role, userId);
            return this.Ok(this.mapper.Map<SurveyReportDataBasicInfoDto>(surveys));
        }

        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpPost]
        public async Task<IActionResult> PostAsync(int companyId, int surveyId, int surveyReportId, [FromBody] SurveyReportDataForCreationDto surveyReportDataInfo)
        {
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            var mapped = this.mapper.Map<Survey.Infrastructure.Entities.SurveyReportData>(surveyReportDataInfo);
            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            var surveyReport = await this.surveyReportDataService.CreateAsync(mapped, companyId, surveyId, surveyReportId, role, userId);

            return this.Ok(this.mapper.Map<SurveyReportDataBasicInfoDto>(surveyReport));
        }

        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpDelete("{respondentId}")]
        public async Task<IActionResult> Delete(int companyId, int surveyId, int surveyReportId, int respondentId)
        {
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            return this.Ok(await this.surveyReportDataService.DeleteAsync(companyId, surveyId, surveyReportId, respondentId, role, userId));
        }

        // PUT: api/companies/5
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpPut("{respondentId}")]
        public async Task<IActionResult> PutAsync(int companyId, int surveyId, int surveyReportId, int respondentId, [FromBody] SurveyReportDataUpdateDto surveyInfo)
        {
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            var mapped = this.mapper.Map<Survey.Infrastructure.Entities.SurveyReportData>(surveyInfo);
            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            var survey = await this.surveyReportDataService.UpdateAsync(mapped, companyId, surveyId, surveyReportId, respondentId, role, userId);
            return this.Ok(this.mapper.Map<SurveyReportDataBasicInfoDto>(survey));
        }
    }
}
