namespace Survey.API.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Survey.API.DTOs.AnwserDtos;
    using Survey.API.JwtRelated.Helpers;
    using Survey.Domain.Services.AnwserService;
    using Survey.Domain.Services.Helper_Admin;

    [Authorize]
    [ApiController]
    [Route("api/{companyId}/{answerBlockId}/answer")]
    public class AnswerController : Controller
    {
        private readonly IMapper mapper;
        private readonly IAnswerService anwserService;

        public AnswerController(IMapper mapper, IAnswerService anwserService)
        {
            this.mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            this.anwserService = anwserService ??
                throw new ArgumentNullException(nameof(anwserService));
        }

        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpGet]
        public ActionResult<IEnumerable<AnswerBasicInfoDto>> GetAll(int companyId, int answerBlockId)
        {
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            var surveys = this.anwserService.GetAll(companyId, answerBlockId, role, userId);
            return this.Ok(this.mapper.Map<List<AnswerBasicInfoDto>>(surveys));
        }

        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpGet("{asnwerId}")]
        public IActionResult Get(int companyId, int answerBlockId, int asnwerId)
        {
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            var question = this.anwserService.GetById(companyId, answerBlockId, asnwerId, role, userId);
            return this.Ok(this.mapper.Map<AnswerBasicInfoDto>(question));
        }

        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpPost]
        public async Task<IActionResult> PostAsync(int companyId, int answerBlockId, [FromBody] AnswerForCreationDto answerInfo)
        {
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            var mapped = this.mapper.Map<Survey.Infrastructure.Entities.Anwser>(answerInfo);
            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            var answer = await this.anwserService.CreateAsync(mapped, companyId, answerBlockId, role, userId);
            return this.Ok(this.mapper.Map<Survey.API.DTOs.AnwserDtos.AnswerBasicInfoDto>(answer));
        }

        // PUT: api/companies/5
        //[Authorize(Role.CompanyAdmin, Role.SuperAdmin)]
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpPut("{asnwerId}")]
        public async Task<IActionResult> PutAsync(int companyId, int answerBlockId, int asnwerId, [FromBody] AnswerUpdateDto answerInfo)
        {
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            var mapped = this.mapper.Map<Survey.Infrastructure.Entities.Anwser>(answerInfo);
            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            var answer = await this.anwserService.UpdateAsync(mapped, companyId, answerBlockId, asnwerId, role, userId);
            return this.Ok(this.mapper.Map<Survey.API.DTOs.AnwserDtos.AnswerBasicInfoDto>(answer));
        }

        //[Authorize(Role.CompanyAdmin, Role.SuperAdmin)]
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpDelete("{asnwerId}")]
        public async Task<IActionResult> Delete(int companyId, int answerBlockId, int asnwerId)
        {
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;
            return this.Ok(await this.anwserService.DeleteAsync(companyId, answerBlockId, asnwerId, role, userId));
        }
    }
}
