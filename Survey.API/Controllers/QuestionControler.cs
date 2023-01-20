using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Survey.API.DTOs.QuestionDtos;
using Survey.Domain.Services.Interfaces;
using Survey.Infrastructure.Entities;

namespace Survey.API.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/{companyId}/questions/{surveyId}")]
    public class QuestionController : BaseController<QuestionController>
    {
        private readonly IMapper _mapper;
        private readonly IQuestionService _questionService;

        public QuestionController(IMapper mapper, IQuestionService questionService, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _questionService = questionService ??
                throw new ArgumentNullException(nameof(questionService));
        }

        /// <summary>
        /// Gets all Questions
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="surveyId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpGet]
        public ActionResult<IEnumerable<QuestionBasicInfoDto>> GetAll(int companyId, int surveyId)
        {
            var surveys = _questionService.GetAll(companyId, surveyId, UserInfo.role, UserInfo.userId);
            return Ok(_mapper.Map<List<QuestionBasicInfoDto>>(surveys));
        }

        /// <summary>
        /// Gets specific question
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="surveyId"></param>
        /// <param name="questionId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpGet("{questionId}")]
        public IActionResult Get(int companyId, int surveyId, int questionId)
        {
            var question = _questionService.GetById(companyId, surveyId, questionId, UserInfo.role, UserInfo.userId);
            return Ok(_mapper.Map<QuestionBasicInfoDto>(question));
        }

        /// <summary>
        /// Creates a question
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="surveyId"></param>
        /// <param name="questionInfo"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpPost]
        public async Task<IActionResult> PostAsync(int companyId, int surveyId, [FromBody] QuestionForCreationDto questionInfo)
        {
            var mapped = _mapper.Map<Question>(questionInfo);
            var survey = await _questionService.CreateAsync(mapped, companyId, surveyId, UserInfo.role, UserInfo.userId);
            return Ok(_mapper.Map<QuestionBasicInfoDto>(survey));
        }

        /// <summary>
        /// Deletes a question
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="surveyId"></param>
        /// <param name="questionId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpDelete("{questionId}")]
        public async Task<IActionResult> Delete(int companyId, int surveyId, int questionId)
        {
            return Ok(await _questionService.DeleteAsync(companyId, surveyId, questionId, UserInfo.role, UserInfo.userId));
        }

        /// <summary>
        /// Updates a question
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="surveyId"></param>
        /// <param name="questionId"></param>
        /// <param name="questionInfo"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpPut("{questionId}")]
        public async Task<IActionResult> PutAsync(int companyId, int surveyId, int questionId, [FromBody] QuestionUpdateDto questionInfo)
        {
            var mapped = _mapper.Map<Question>(questionInfo);
            var survey = await _questionService.UpdateAsync(mapped, companyId, surveyId, questionId, UserInfo.role, UserInfo.userId);
            return Ok(_mapper.Map<QuestionBasicInfoDto>(survey));
        }
    }
}
