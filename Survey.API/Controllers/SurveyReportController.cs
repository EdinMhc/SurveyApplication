using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Survey.API.DTOs.SurveyReportDtos;
using Survey.Domain.Services.SurveyReport_Service;
using Survey.Infrastructure.Entities;

namespace Survey.API.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/{companyId}/surveyreport/{surveyId}")]
    public class SurveyReportController : BaseController<SurveyReportController>
    {
        private readonly IMapper _mapper;
        private readonly ISurveyReportService _surveyReportService;

        public SurveyReportController(IMapper mapper, ISurveyReportService surveyReportService, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _surveyReportService = surveyReportService ??
                throw new ArgumentNullException(nameof(surveyReportService));
        }

        /// <summary>
        /// Gets all survey reports
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="surveyId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpGet]
        public ActionResult<IEnumerable<SurveyReportBasicInfoDto>> GetAll(int companyId, int surveyId)
        {
            var surveys = _surveyReportService.GetAll(companyId, surveyId, UserInfo.userId, UserInfo.role);
            return Ok(_mapper.Map<List<SurveyReportBasicInfoDto>>(surveys));
        }

        /// <summary>
        /// Gets a specific survey report
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="surveyId"></param>
        /// <param name="surveyReportId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpGet("{surveyReportId}")]
        public IActionResult Get(int companyId, int surveyId, int surveyReportId)
        {
            var surveys = _surveyReportService.GetById(companyId, surveyId, surveyReportId, UserInfo.role, UserInfo.userId);
            return Ok(_mapper.Map<SurveyReportBasicInfoDto>(surveys));
        }

        /// <summary>
        /// Creates a survey report
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="surveyId"></param>
        /// <param name="surveyReportInfo"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpPost]
        public async Task<IActionResult> PostAsync(int companyId, int surveyId, [FromBody] SurveyReportForCreationDto surveyReportInfo)
        {
            var mapped = _mapper.Map<SurveyReport>(surveyReportInfo);
            var surveyReport = await _surveyReportService.CreateAsync(mapped, companyId, surveyId, UserInfo.role, UserInfo.userId);
            return Ok(_mapper.Map<SurveyReportBasicInfoDto>(surveyReport));
        }

        /// <summary>
        /// Deletes a survey report
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="surveyId"></param>
        /// <param name="surveyReportId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpDelete("{surveyReportId}")]
        public async Task<IActionResult> Delete(int companyId, int surveyId, int surveyReportId)
        {
            return Ok(await _surveyReportService.DeleteAsync(companyId, surveyId, surveyReportId, UserInfo.role, UserInfo.userId));
        }

        /// <summary>
        /// Updates a survey report
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="surveyId"></param>
        /// <param name="surveyReportId"></param>
        /// <param name="surveyInfo"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpPut("{surveyReportId}")]
        public async Task<IActionResult> PutAsync(int companyId, int surveyId, int surveyReportId, [FromBody] SurveyReportUpdateDtos surveyInfo)
        {
            var mapped = _mapper.Map<SurveyReport>(surveyInfo);
            var survey = await _surveyReportService.UpdateAsync(mapped, companyId, surveyId, surveyReportId, UserInfo.role, UserInfo.userId);
            return Ok(_mapper.Map<SurveyReportBasicInfoDto>(survey));
        }
    }
}