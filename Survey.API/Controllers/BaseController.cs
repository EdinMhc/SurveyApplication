namespace Survey.API.Controllers
{
    public abstract class BaseController<T> : ControllerBase
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public (string userId, string role) UserInfo { get; set; }

        public BaseController(IHttpContextAccessor contextAccessor)
        {
            string userId = contextAccessor.HttpContext.GetUserId();
            string role = contextAccessor.HttpContext.GetRole();
            UserInfo = (userId, role);
            _contextAccessor = contextAccessor;
        }
    }
}
