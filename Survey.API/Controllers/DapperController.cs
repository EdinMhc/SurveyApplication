namespace Survey.API.Controllers
{

    [Authorize]
    [Route("api/dapper")]
    public class DapperController : BaseController<DapperController>
    {
        private readonly IMapper _mapper;
        private readonly ICompanyServiceDapper _companyService;

        public DapperController(IMapper mapper, ICompanyServiceDapper companyService, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _companyService = companyService ??
                throw new ArgumentNullException(nameof(companyService));
        }

        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetAll()
        {
            var company = await _companyService.GetAllAsync(UserInfo.role, UserInfo.userId);
            return Ok(_mapper.Map<List<CompanyDto>>(company));
        }

        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var company = await _companyService.GetById(id, UserInfo.role, UserInfo.userId);
            return Ok(_mapper.Map<CompanyDto>(company));
        }

        [Authorize(Roles = "Admin, SuperAdmin", Policy = "IsAnonymousUser")]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] List<DapperCompanyCreationDto> companyInfo)
        {
            var company = await _companyService.UpdateDapper(companyInfo, UserInfo.role, UserInfo.userId);
            return Ok(_mapper.Map<CompanyDto>(company));
        }
    }
}
