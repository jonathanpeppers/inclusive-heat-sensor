using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inclusive.HeatSensor.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeatSensorController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "hello from Heat Sensor";
        }
    }
}
