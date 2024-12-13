using Azure.AI.OpenAI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using System.ClientModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Inclusive.HeatSensor.Services;

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

    public LLMClient(IOptions<LLMClientOptions> llmClientOptions, ILogger<LLMClient> logger)
    {
        var _llmClientOptions = llmClientOptions.Value;
        _logger = logger;

        if (string.IsNullOrEmpty(_llmClientOptions.OpenAIApiKey))
        {
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            if (string.IsNullOrEmpty(apiKey))
            {
                ArgumentNullException.ThrowIfNull(apiKey, nameof(apiKey));
            }

            _llmClientOptions.OpenAIApiKey = apiKey;
        }

        _client = new AzureOpenAIClient(new Uri(_llmClientOptions.OpenAIEndpoint), new ApiKeyCredential(_llmClientOptions.OpenAIApiKey));
        _chat = _client.GetChatClient(_llmClientOptions.DeploymentName);
    }

    const string Prompt = """
        You are expert software engineer that is particularly good at writing inclusive, well-written, thoughtful code reviews.
        Rate the following code review comment with values between 0 and 10 for how offensive and how angry the comment is.
        Respond using JSON syntax, using the format: { "offensive": 1, "anger": 2 }
        """;

    public async Task<Rating> RateComment(string comment, string? url)
    {
        // If we have the URL, no need to log the comment text.
        // This is probably better, as it gives users an option to delete their comment and we have no record of it.
        if (string.IsNullOrEmpty(url))
        {
            _logger.LogInformation("Comment: {comment}", comment);
        }

        Rating? rating;
        try
        {
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
            _logger.LogInformation("OpenAI reponse: {json}", json);
            rating = JsonSerializer.Deserialize(json, RatingContext.Default.Rating);
            ArgumentNullException.ThrowIfNull(rating, nameof(rating));
        }
        catch (ClientResultException ex)
        {
            _logger.LogInformation("OpenAI exception: {ex}", ex);
            if (ex.Message.Contains("content_filter", StringComparison.OrdinalIgnoreCase))
            {
                rating = new Rating { Offensive = -1, Anger = -1, Url = url };
            }
            else
            {
                throw;
            }
        }

        rating.Url = url;
        _logger.LogInformation("Rating: {rating}", rating);
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

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    public override string ToString() =>
        JsonSerializer.Serialize(this, RatingContext.Default.Rating);
}
