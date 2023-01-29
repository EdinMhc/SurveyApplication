namespace Survey.API.Controllers
{

    [Authorize]
    [Produces("application/json")]
    [ApiController]
    [Route("api/{companyId}/surveys")]
    public class SurveyController : BaseController<SurveyController>
    {
        private readonly IMapper _mapper;
        private readonly ISurveyService _surveyService;

        public SurveyController(IMapper mapper, ISurveyService surveyService, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _surveyService = surveyService ??
                throw new ArgumentNullException(nameof(surveyService));
        }

        /// <summary>
        /// Gets all surveys 
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpGet]
        public ActionResult<IEnumerable<SurveyDto>> GetAll(int companyId)
        {
            var surveys = _surveyService.GetAll(companyId, UserInfo.role, UserInfo.userId);
            return Ok(_mapper.Map<List<SurveyDto>>(surveys));
        }

        /// <summary>
        /// Get specific survey
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpGet("{surveyId}")]
        public IActionResult Get(int surveyId, int companyId)
        {
            var surveys = _surveyService.GetById(surveyId, companyId, UserInfo.role, UserInfo.userId);
            return Ok(_mapper.Map<SurveyDto>(surveys));
        }

        /// <summary>
        /// Create survey
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="surveyInfo"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpPost]
        public async Task<IActionResult> PostAsync(int companyId, [FromBody] SurveyCreationDto surveyInfo)
        {
            var mapped = _mapper.Map<Surveys>(surveyInfo);
            var survey = await _surveyService.CreateAsync(mapped, companyId, UserInfo.role, UserInfo.userId);
            return Ok(_mapper.Map<SurveyDto>(survey));
        }

        /// <summary>
        /// Delete specific survey
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpDelete("{surveyId}")]
        public async Task<IActionResult> Delete(int surveyId, int companyId)
        {
            return Ok(await _surveyService.DeleteAsync(surveyId, companyId, UserInfo.role, UserInfo.userId));
        }

        /// <summary>
        /// Update survey
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="companyId"></param>
        /// <param name="surveyInfo"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpPut("{surveyId}")]
        public async Task<IActionResult> PutAsync(int surveyId, int companyId, [FromBody] SurveyUpdateDto surveyInfo)
        {
            var mapped = _mapper.Map<Surveys>(surveyInfo);
            var survey = await _surveyService.UpdateAsync(mapped, surveyId, companyId, UserInfo.role, UserInfo.userId);
            return Ok(_mapper.Map<SurveyDto>(survey));
        }
    }
}
