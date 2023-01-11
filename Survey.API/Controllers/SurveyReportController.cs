namespace Survey.API.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Survey.API.DTOs.SurveyReportDtos;
    using Survey.API.JwtRelated.Helpers;
    using Survey.Domain.Services.Helper_Admin;
    using Survey.Domain.Services.SurveyReport_Service;

    [Authorize]
    [ApiController]
    [Route("api/{companyId}/surveyreport/{surveyId}")]
    public class SurveyReportControlLer : Controller
    {
        private readonly IMapper mapper;
        private readonly ISurveyReportService surveyReportService;

        public SurveyReportControlLer(IMapper mapper, ISurveyReportService surveyReportService)
        {
            this.mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            this.surveyReportService = surveyReportService ??
                throw new ArgumentNullException(nameof(surveyReportService));
        }

        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpGet]
        public ActionResult<IEnumerable<SurveyReportBasicInfoDto>> GetAll(int companyId, int surveyId)
        {
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            var surveys = this.surveyReportService.GetAll(companyId, surveyId, userId, role);
            return this.Ok(this.mapper.Map<List<SurveyReportBasicInfoDto>>(surveys));
        }

        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpGet("{surveyReportId}")]
        public IActionResult Get(int companyId, int surveyId, int surveyReportId)
        {
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            var surveys = this.surveyReportService.GetById(companyId, surveyId, surveyReportId, role, userId);
            return this.Ok(this.mapper.Map<SurveyReportBasicInfoDto>(surveys));
        }

        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpPost]
        public async Task<IActionResult> PostAsync(int companyId, int surveyId, [FromBody] SurveyReportForCreationDto surveyReportInfo)
        {
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            var mapped = this.mapper.Map<Survey.Infrastructure.Entities.SurveyReport>(surveyReportInfo);

            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;
            var surveyReport = await this.surveyReportService.CreateAsync(mapped, companyId, surveyId, role, userId);
            return this.Ok(this.mapper.Map<SurveyReportBasicInfoDto>(surveyReport));
        }

        // DELETE: api/companies/5
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpDelete("{surveyReportId}")]
        public async Task<IActionResult> Delete(int companyId, int surveyId, int surveyReportId)
        {
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;
            return Ok(await this.surveyReportService.DeleteAsync(companyId, surveyId, surveyReportId, role, userId));
        }

        // PUT: api/companies/5
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpPut("{surveyReportId}")]
        public async Task<IActionResult> PutAsync(int companyId, int surveyId, int surveyReportId, [FromBody] SurveyReportUpdateDtos surveyInfo)
        {
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            var mapped = this.mapper.Map<Survey.Infrastructure.Entities.SurveyReport>(surveyInfo);

            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            var survey = await this.surveyReportService.UpdateAsync(mapped, companyId, surveyId, surveyReportId, role, userId);

            return this.Ok(this.mapper.Map<SurveyReportBasicInfoDto>(survey));
        }
    }
}