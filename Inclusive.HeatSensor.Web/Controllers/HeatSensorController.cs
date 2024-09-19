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

        [HttpPost]
        public ActionResult<string> Post([FromBody] HeatSensorRequest requestBoday)
        {
            // Handle the comment here

            return "Comment received: " + requestBoday.Comment;
        }
    }
}
