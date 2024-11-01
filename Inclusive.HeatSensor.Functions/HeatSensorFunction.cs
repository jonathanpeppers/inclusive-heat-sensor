using Inclusive.HeatSensor.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Inclusive.HeatSensor.Functions;

public class HeatSensorFunction
{
    readonly ILogger<HeatSensorFunction> _logger;
    readonly LLMClient _client;

    public HeatSensorFunction(ILogger<HeatSensorFunction> logger, LLMClient client)
    {
        _logger = logger;
        _client = client;
    }

    [Function("heatsensor")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req, [FromBody] string comment, [FromBody] string? url)
    {
        _logger.LogDebug("heatsensor request");
        if (string.IsNullOrEmpty(comment))
        {
            _logger.LogInformation("comment is blank");
            return new BadRequestObjectResult("Please provide a comment to rate.");
        }

        // LogDebug shouldn't save the message
        _logger.LogDebug("Request Comment: " + comment);
        var rating = await _client.RateComment(comment, url);
        return new OkObjectResult(rating);
    }
}
