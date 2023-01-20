using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Survey.API.DTOs.Company;
using Survey.API.JwtRelated.Helpers;
using Survey.Domain.DapperServices.DapperCompanyServices;
using Survey.Domain.Services;
using Survey.Infrastructure.DapperRepository.StoredProcedure.DapperDto;

namespace Survey.API.Controllers
{

    [Authorize]
    [Route("api/dapper")]
    public class DapperController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ICompanyServiceDapper companyService;

        public DapperController(IMapper mapper, ICompanyServiceDapper companyService)
        {
            mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            companyService = companyService ??
                throw new ArgumentNullException(nameof(companyService));
        }

        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyBasicInfoDto>>> GetAll()
        {
            string userId = GeneralExtensions.GetUserId(HttpContext);
            string role = User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            var companiesAdmin = await companyService.GetAllAsync(role, userId);
            return Ok(mapper.Map<List<CompanyBasicInfoDto>>(companiesAdmin));
        }

        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            string userId = GeneralExtensions.GetUserId(HttpContext);
            string role = User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            var company1 = await companyService.GetById(id, role, userId);
            return Ok(mapper.Map<CompanyBasicInfoDto>(company1));
        }

        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] List<DapperCompanyCreationDto> companyInfo)
        {
            string userId = GeneralExtensions.GetUserId(HttpContext);
            string role = User.IsInRole(AdminHelper.Admin) ? AdminHelper.Admin : AdminHelper.SuperAdmin;

            var company = await companyService.UpdateDapper(companyInfo, role, userId);
            return Ok(mapper.Map<CompanyBasicInfoDto>(company));
        }
    }
}
