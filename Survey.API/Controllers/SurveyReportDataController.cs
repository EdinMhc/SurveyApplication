using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Survey.API.DTOs.SurveyReportData;
using Survey.Domain.Services.SurveyReportDataService;
using Survey.Infrastructure.Entities;

namespace Survey.API.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/{companyId}/surveyreportdata/{surveyId}/{surveyReportId}")]
    public class SurveyReportDataController : BaseController<SurveyReportController>
    {
        private readonly IMapper _mapper;
        private readonly ISurveyReportDataService _surveyReportDataService;

        public SurveyReportDataController(IMapper mapper, ISurveyReportDataService surveyReportDataService, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _surveyReportDataService = surveyReportDataService ??
                throw new ArgumentNullException(nameof(surveyReportDataService));
        }

        /// <summary>
        /// Gets all survey report data
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="surveyId"></param>
        /// <param name="surveyReportId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpGet]
        public ActionResult<IEnumerable<SurveyReportDataBasicInfoDto>> GetAll(int companyId, int surveyId, int surveyReportId)
        {
            var surveys = _surveyReportDataService.GetAll(companyId, surveyId, surveyReportId, UserInfo.role, UserInfo.userId);
            return Ok(_mapper.Map<List<SurveyReportDataBasicInfoDto>>(surveys));
        }

        /// <summary>
        /// Gets a specific survey report data
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="surveyId"></param>
        /// <param name="surveyReportId"></param>
        /// <param name="respondentId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpGet("{respondentId}")]
        public IActionResult Get(int companyId, int surveyId, int surveyReportId, int respondentId)
        {
            var surveys = _surveyReportDataService.GetById(companyId, surveyId, surveyReportId, respondentId, UserInfo.role, UserInfo.userId);
            return Ok(_mapper.Map<SurveyReportDataBasicInfoDto>(surveys));
        }

        /// <summary>
        /// Creates survey report data
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="surveyId"></param>
        /// <param name="surveyReportId"></param>
        /// <param name="surveyReportDataInfo"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpPost]
        public async Task<IActionResult> PostAsync(int companyId, int surveyId, int surveyReportId, [FromBody] SurveyReportDataForCreationDto surveyReportDataInfo)
        {
            var mapped = _mapper.Map<SurveyReportData>(surveyReportDataInfo);
            var surveyReport = await _surveyReportDataService.CreateAsync(mapped, companyId, surveyId, surveyReportId, UserInfo.role, UserInfo.userId);
            return Ok(_mapper.Map<SurveyReportDataBasicInfoDto>(surveyReport));
        }

        /// <summary>
        /// Delete survey report data
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="surveyId"></param>
        /// <param name="surveyReportId"></param>
        /// <param name="respondentId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpDelete("{respondentId}")]
        public async Task<IActionResult> Delete(int companyId, int surveyId, int surveyReportId, int respondentId)
        {
            return Ok(await _surveyReportDataService.DeleteAsync(companyId, surveyId, surveyReportId, respondentId, UserInfo.role, UserInfo.userId));
        }

        /// <summary>
        /// Updates survey report data
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="surveyId"></param>
        /// <param name="surveyReportId"></param>
        /// <param name="respondentId"></param>
        /// <param name="surveyInfo"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpPut("{respondentId}")]
        public async Task<IActionResult> PutAsync(int companyId, int surveyId, int surveyReportId, int respondentId, [FromBody] SurveyReportDataUpdateDto surveyInfo)
        {
            var mapped = _mapper.Map<SurveyReportData>(surveyInfo);
            var survey = await _surveyReportDataService.UpdateAsync(mapped, companyId, surveyId, surveyReportId, respondentId, UserInfo.role, UserInfo.userId);
            return Ok(_mapper.Map<SurveyReportDataBasicInfoDto>(survey));
        }
    }
}
