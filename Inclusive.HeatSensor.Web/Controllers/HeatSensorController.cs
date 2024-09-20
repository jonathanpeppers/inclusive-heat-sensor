using Inclusive.HeatSensor.Services;
using Microsoft.AspNetCore.Mvc;

namespace Inclusive.HeatSensor.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeatSensorController : ControllerBase
    {
        readonly ILogger<HeatSensorController> _logger;
        readonly LLMClient _client;

        public HeatSensorController(ILogger<HeatSensorController> logger, LLMClient client)
        {
            _logger = logger;
            _client = client;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return "hello from Heat Sensor";
        }

        [HttpPost]
        public async Task<ActionResult<string>> Post([FromBody] HeatSensorRequest requestBoday)
        {
            // Handle the comment here
            _logger.LogInformation("heatsensor request");

            if (string.IsNullOrEmpty(requestBoday.Comment))
            {
                _logger.LogInformation("comment is blank");
                return new BadRequestObjectResult("Please provide a comment to rate.");
            }

            _logger.LogDebug("comment: " + requestBoday.Comment);
            var rating = await _client.RateComment(requestBoday.Comment);
            return new OkObjectResult(rating);
        }
    }
}
