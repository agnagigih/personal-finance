using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Personal.Finance.Api.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected Guid UserId => Guid.Parse(User.FindFirstValue("userId"));
    }
}
