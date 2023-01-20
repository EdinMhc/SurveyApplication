namespace Survey.API.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Survey.API.DTOs.AnwserDtos;
    using Survey.Domain.Services.Interfaces;
    using Survey.Infrastructure.Entities;

    [Authorize]
    [ApiController]
    [Route("api/{companyId}/{answerBlockId}/answer")]
    public class AnswerController : BaseController<AnswerController>
    {
        private readonly IMapper _mapper;
        private readonly IAnswerService _anwserService;

        public AnswerController(IMapper mapper, IAnswerService anwserService, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _anwserService = anwserService ??
                throw new ArgumentNullException(nameof(anwserService));
        }

        /// <summary>
        /// Gets all Answers
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="answerBlockId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpGet]
        public ActionResult<IEnumerable<AnswerBasicInfoDto>> GetAll(int companyId, int answerBlockId)
        {
            var surveys = _anwserService.GetAll(companyId, answerBlockId, UserInfo.role, UserInfo.userId);
            return Ok(_mapper.Map<List<AnswerBasicInfoDto>>(surveys));
        }

        /// <summary>
        /// Gets a specific answer
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="answerBlockId"></param>
        /// <param name="answerId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpGet("{answerId}")]
        public IActionResult Get(int companyId, int answerBlockId, int answerId)
        {
            var question = _anwserService.GetById(companyId, answerBlockId, answerId, UserInfo.role, UserInfo.userId);
            return Ok(_mapper.Map<AnswerBasicInfoDto>(question));
        }

        /// <summary>
        /// Creates a answer
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="answerBlockId"></param>
        /// <param name="answerInfo"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpPost]
        public async Task<IActionResult> PostAsync(int companyId, int answerBlockId, [FromBody] AnswerForCreationDto answerInfo)
        {
            var mapped = _mapper.Map<Anwser>(answerInfo);
            var answer = await _anwserService.CreateAsync(mapped, companyId, answerBlockId, UserInfo.role, UserInfo.userId);
            return Ok(_mapper.Map<AnswerBasicInfoDto>(answer));
        }

        /// <summary>
        /// Updates a answer
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="answerBlockId"></param>
        /// <param name="answerId"></param>
        /// <param name="answerInfo"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpPut("{answerId}")]
        public async Task<IActionResult> PutAsync(int companyId, int answerBlockId, int answerId, [FromBody] AnswerUpdateDto answerInfo)
        {
            var mapped = _mapper.Map<Anwser>(answerInfo);
            var answer = await _anwserService.UpdateAsync(mapped, companyId, answerBlockId, answerId, UserInfo.role, UserInfo.userId);
            return Ok(_mapper.Map<AnswerBasicInfoDto>(answer));
        }

        /// <summary>
        /// Deletes a answer
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="answerBlockId"></param>
        /// <param name="answerId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpDelete("{answerId}")]
        public async Task<IActionResult> Delete(int companyId, int answerBlockId, int answerId)
        {
            return Ok(await _anwserService.DeleteAsync(companyId, answerBlockId, answerId, UserInfo.role, UserInfo.userId));
        }
    }
}
