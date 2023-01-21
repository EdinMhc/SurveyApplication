namespace Survey.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/{companyId}/answerblock/{surveyId}")]
    public class AnswerBlockController : BaseController<AnswerBlockController>
    {
        private readonly IMapper _mapper;
        private readonly IAnwserBlockService _anwserBlockService;

        public AnswerBlockController(IMapper mapper, IAnwserBlockService anwserBlockService, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _anwserBlockService = anwserBlockService ??
                throw new ArgumentNullException(nameof(anwserBlockService));
        }

        /// <summary>
        /// Gets all answer blocks
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="surveyId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpGet]
        public ActionResult<IEnumerable<AnwserBlockBasicInfoDto>> GetAll(int companyId, int surveyId)
        {
            var surveys = _anwserBlockService.GetAll(companyId, surveyId, UserInfo.role, UserInfo.userId);
            return Ok(_mapper.Map<List<AnwserBlockBasicInfoDto>>(surveys));
        }

        /// <summary>
        /// Gets a specific answer block
        /// </summary>
        /// <param name="answerBlockId"></param>
        /// <param name="companyId"></param>
        /// <param name="surveyId"></param>
        /// <returns></returns>
        [HttpGet("{answerBlockId}")]
        public IActionResult Get(int answerBlockId, int companyId, int surveyId)
        {
            var question = _anwserBlockService.GetById(answerBlockId, companyId, surveyId, UserInfo.role, UserInfo.userId);
            return Ok(_mapper.Map<AnwserBlockBasicInfoDto>(question));
        }

        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpPost]
        public async Task<IActionResult> PostAsync(int companyId, int surveyId, [FromBody] AnwserBlockForCreationDto anwserBlockInfo)
        {
            var mapped = _mapper.Map<AnwserBlock>(anwserBlockInfo);
            var survey = await _anwserBlockService.CreateAsync(mapped, companyId, surveyId, UserInfo.role, UserInfo.userId);
            return Ok(_mapper.Map<AnwserBlockBasicInfoDto>(survey));
        }

        /// <summary>
        /// Updates a answer block
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="surveyId"></param>
        /// <param name="answerBlockId"></param>
        /// <param name="anwserBlockInfo"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpPut("{answerBlockId}")]
        public async Task<IActionResult> PutAsync(int companyId, int surveyId, int answerBlockId, [FromBody] AnwserBlockUpdateDto anwserBlockInfo)
        {
            var mapped = _mapper.Map<AnwserBlock>(anwserBlockInfo);
            var anwserBlock = await _anwserBlockService.UpdateAsync(mapped, companyId, surveyId, answerBlockId, UserInfo.role, UserInfo.userId);
            return Ok(_mapper.Map<AnwserBlockBasicInfoDto>(anwserBlock));
        }

        /// <summary>
        /// Deletes a specific AnswerBlock
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="surveyId"></param>
        /// <param name="answerBlockId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpDelete("{answerBlockId}")]
        public async Task<IActionResult> Delete(int companyId, int surveyId, int answerBlockId)
        {
            return Ok(await _anwserBlockService.DeleteAsync(companyId, surveyId, answerBlockId, UserInfo.role, UserInfo.userId));
        }

    }
}
