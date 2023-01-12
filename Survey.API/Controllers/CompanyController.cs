namespace Survey.API.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Survey.API.DTOs.Company;
    using Survey.API.JwtRelated.Helpers;
    using Survey.Domain.Services.CompanyService;
    using Survey.Domain.Services.Helper_Admin;
    using Survey.Infrastructure.Entities;

    [Authorize]
    [ApiController]
    [Route("api/companies")]
    public class CompanyController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ICompanyService companyService;

        public CompanyController(IMapper mapper, ICompanyService companyService, UserManager<User> userManager)
        {
            this.mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            this.companyService = companyService ??
                throw new ArgumentNullException(nameof(companyService));
        }

        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpGet]
        public ActionResult<IEnumerable<CompanyBasicInfoDto>> GetAll()
        {
            string userId = GeneralExtensions.GetUserId(this.HttpContext);

            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            var companiesSuperAdmin = this.companyService.GetAll(role, userId);
            return this.Ok(this.mapper.Map<List<CompanyBasicInfoDto>>(companiesSuperAdmin));
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
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            var company1 = this.companyService.GetById(companyId, role, userId);
            return this.Ok(this.mapper.Map<CompanyBasicInfoDto>(company1));
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
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            var mapped = this.mapper.Map<Company>(companyInfo);
            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            var company = await this.companyService.CreateAsync(mapped, role, userId);
            return this.Ok(this.mapper.Map<CompanyBasicInfoDto>(company));
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
            string userId = GeneralExtensions.GetUserId(this.HttpContext);

            var mapped = this.mapper.Map<Company>(companyInfo);
            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            var company1 = await this.companyService.UpdateAsync(mapped, role, companyId, userId);
            return this.Ok(this.mapper.Map<CompanyBasicInfoDto>(company1));
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
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            return this.Ok(await this.companyService.DeleteAsync(companyId, role, userId));
        }
    }
}
