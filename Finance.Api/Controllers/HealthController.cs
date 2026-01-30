using Microsoft.AspNetCore.Mvc;

namespace Personal.Finance.Api.Controllers
{
    public class HealthController : ControllerBase
    {
        [HttpGet]
        [Route("api/health")]
        public IActionResult Get()
        {
            return Ok(new {status = "OK"});
        }

    }
}
