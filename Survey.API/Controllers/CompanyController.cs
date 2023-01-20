namespace Survey.API.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Survey.API.DTOs.Company;
    using Survey.Domain.Services.Interfaces;
    using Survey.Infrastructure.Entities;

    [Authorize]
    [ApiController]
    [Route("api/companies")]
    public class CompanyController : BaseController<CompanyController>
    {
        private readonly IMapper _mapper;
        private readonly ICompanyService _companyService;

        public CompanyController(IMapper mapper, ICompanyService companyService, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _companyService = companyService ??
                throw new ArgumentNullException(nameof(companyService));
        }

        /// <summary>
        /// Gets all Companies
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpGet]
        public ActionResult<IEnumerable<CompanyBasicInfoDto>> GetAll()
        {
            var companiesSuperAdmin = _companyService.GetAll(UserInfo.userId, UserInfo.userId);
            return Ok(_mapper.Map<List<CompanyBasicInfoDto>>(companiesSuperAdmin));
        }

        /// <summary>
        /// Get company by companyId
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpGet("{companyId}")]
        public IActionResult Get(int companyId)
        {
            var company1 = _companyService.GetById(companyId, UserInfo.userId, UserInfo.userId);
            return Ok(_mapper.Map<CompanyBasicInfoDto>(company1));
        }

        /// <summary>
        /// Create company
        /// </summary>
        /// <param name="companyInfo"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CompanyCreationDto companyInfo)
        {
            var mapped = _mapper.Map<Company>(companyInfo);
            var company = await _companyService.CreateAsync(mapped, UserInfo.userId, UserInfo.userId);
            return Ok(_mapper.Map<CompanyBasicInfoDto>(company));
        }

        /// <summary>
        /// Update company information
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="companyInfo"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpPut("{companyId}")]
        public async Task<IActionResult> PutAsync(int companyId, [FromBody] CompanyCreationDto companyInfo)
        {
            var mapped = _mapper.Map<Company>(companyInfo);
            var company1 = await _companyService.UpdateAsync(mapped, UserInfo.role, companyId, UserInfo.userId);
            return Ok(_mapper.Map<CompanyBasicInfoDto>(company1));
        }

        /// <summary>
        /// Delete a company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpDelete("{companyId}")]
        public async Task<IActionResult> Delete(int companyId)
        {
            return Ok(await _companyService.DeleteAsync(companyId, UserInfo.userId, UserInfo.userId));
        }
    }
}
