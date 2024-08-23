using Azure.AI.OpenAI;
using Microsoft.Extensions.Logging;
using OpenAI.Chat;
using System.ClientModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Inclusive.HeatSensor.Functions;

public class LLMClient
{
    readonly ILogger<LLMClient> _logger;
    readonly AzureOpenAIClient _client;
    readonly ChatClient _chat;

    public LLMClient(ILogger<LLMClient> logger)
    {
        _logger = logger;

        var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        if (string.IsNullOrEmpty(apiKey))
        {
            ArgumentNullException.ThrowIfNull(apiKey, nameof(apiKey));
        }
        _client = new AzureOpenAIClient(new Uri("https://icropenaiservice2.openai.azure.com/"), new ApiKeyCredential(apiKey));
        _chat = _client.GetChatClient("gpt-4o");
    }

    const string Prompt = """
        You are expert software engineer that is particularly good at writing inclusive, well-written, thoughtful code reviews.
        Rate the following code review comment with values between 0 and 10 for how offensive and how angry the comment is.
        Respond using JSON syntax, using the format: { "offensive": 1, "anger": 2 }
        """;

    public async Task<Rating> RateComment(string comment)
    {
        _logger.LogInformation("Comment: {comment}", comment);
        var response = await _chat.CompleteChatAsync(
            [
                new SystemChatMessage(Prompt),
                new UserChatMessage(comment),
            ],
            new ChatCompletionOptions
            {
                Temperature = 0.5f,
                ResponseFormat = ChatResponseFormat.JsonObject,
            });
        var json = response.Value.Content[0].ToString();
        _logger.LogInformation("Rating: {json}", json);
        var rating = JsonSerializer.Deserialize(json, RatingContext.Default.Rating);
        ArgumentNullException.ThrowIfNull(rating, nameof(rating));
        return rating;
    }
}

[JsonSerializable(typeof(Rating))]
internal partial class RatingContext : JsonSerializerContext
{
}

public class Rating
{
    [JsonPropertyName("offensive")]
    public int Offensive { get; set; }
    [JsonPropertyName("anger")]
    public int Anger { get; set; }
}
