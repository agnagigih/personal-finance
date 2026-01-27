using Microsoft.AspNetCore.Mvc;

namespace Finance.Api.Controllers
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
