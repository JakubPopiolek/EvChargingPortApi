using Microsoft.AspNetCore.Mvc;

namespace EvApplicationApi.Controllers
{
    [Route("api/ping")]
    [ApiController]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public ActionResult<DateTime> Get()
        {
            return Ok(DateTime.Now);
        }
    }
}
