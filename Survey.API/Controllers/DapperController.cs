namespace Survey.API.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Survey.API.DTOs.Company;
    using Survey.API.JwtRelated.Helpers;
    using Survey.Domain.DapperServices.DapperCompanyServices;
    using Survey.Domain.Services.Helper_Admin;
    using Survey.Infrastructure.DapperRepository.StoredProcedure.DapperDto;

    [Authorize]
    [Route("api/dapper")]
    public class DapperController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ICompanyServiceDapper companyService;

        public DapperController(IMapper mapper, ICompanyServiceDapper companyService)
        {
            this.mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            this.companyService = companyService ??
                throw new ArgumentNullException(nameof(companyService));
        }

        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyBasicInfoDto>>> GetAll()
        {
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            var companiesAdmin = await this.companyService.GetAllAsync(role, userId);
            return this.Ok(this.mapper.Map<List<CompanyBasicInfoDto>>(companiesAdmin));
        }

        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            var company1 = await this.companyService.GetById(id, role, userId);
            return this.Ok(this.mapper.Map<CompanyBasicInfoDto>(company1));
        }

        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] List<DapperCompanyCreationDto> companyInfo)
        {
            string userId = GeneralExtensions.GetUserId(this.HttpContext);
            string role = this.User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            var company = await this.companyService.UpdateDapper(companyInfo, role, userId);
            return this.Ok(this.mapper.Map<CompanyBasicInfoDto>(company));
        }
    }
}
