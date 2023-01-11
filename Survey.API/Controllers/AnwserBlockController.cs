namespace Survey.API.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Survey.API.DTOs.AnwserBlockDtos;
    using Survey.API.JwtRelated.Helpers;
    using Survey.Domain.Services.AnwserBlockService;
    using Survey.Domain.Services.Helper_Admin;

    [Authorize]
    [ApiController]
    [Route("api/{companyId}/answerblock/{surveyId}")]
    public class AnwserBlockController : Controller
    {
        private readonly IMapper mapper;
        private readonly IAnwserBlockService anwserBlockService;

        public AnwserBlockController(IMapper mapper, IAnwserBlockService anwserBlockService)
        {
            this.mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            this.anwserBlockService = anwserBlockService ??
                throw new ArgumentNullException(nameof(anwserBlockService));
        }

        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpGet]
        public ActionResult<IEnumerable<AnwserBlockBasicInfoDto>> GetAll(int companyId, int surveyId)
        {
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            var surveys = this.anwserBlockService.GetAll(companyId, surveyId, role, userId);
            return this.Ok(this.mapper.Map<List<AnwserBlockBasicInfoDto>>(surveys));
        }

        //[Authorize(Role.CompanyAdmin, Role.SuperAdmin)]
        [HttpGet("{answerBlockId}")]
        public IActionResult Get(int answerBlockId, int companyId, int surveyId)
        {
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;
            var question = this.anwserBlockService.GetById(answerBlockId, companyId, surveyId, role, userId);
            return this.Ok(this.mapper.Map<AnwserBlockBasicInfoDto>(question));
        }

        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpPost]
        public async Task<IActionResult> PostAsync(int companyId, int surveyId, [FromBody] AnwserBlockForCreationDto anwserBlockInfo)
        {
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            var mapped = this.mapper.Map<Survey.Infrastructure.Entities.AnwserBlock>(anwserBlockInfo);
            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            var survey = await this.anwserBlockService.CreateAsync(mapped, companyId, surveyId, role, userId);

            return this.Ok(this.mapper.Map<Survey.API.DTOs.AnwserBlockDtos.AnwserBlockBasicInfoDto>(survey));
        }

        // PUT: api/companies/5
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpPut("{answerBlockId}")]
        public async Task<IActionResult> PutAsync(int companyId, int surveyId, int answerBlockId, [FromBody] AnwserBlockUpdateDto anwserBlockInfo)
        {
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            var mapped = this.mapper.Map<Survey.Infrastructure.Entities.AnwserBlock>(anwserBlockInfo);

            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            var anwserBlock = await this.anwserBlockService.UpdateAsync(mapped, companyId, surveyId, answerBlockId, role, userId);

            return this.Ok(this.mapper.Map<Survey.API.DTOs.AnwserBlockDtos.AnwserBlockBasicInfoDto>(anwserBlock));
        }

        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpDelete("{answerBlockId}")]
        public async Task<IActionResult> Delete(int companyId, int surveyId, int answerBlockId)
        {
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            return this.Ok(await this.anwserBlockService.DeleteAsync(companyId, surveyId, answerBlockId, role, userId));
        }

    }
}
