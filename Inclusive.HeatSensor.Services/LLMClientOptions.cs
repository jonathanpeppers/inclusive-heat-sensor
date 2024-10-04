namespace Inclusive.HeatSensor.Services
{
    public class LLMClientOptions
    {
        public string OpenAIEndpoint { get; set; } = "https://icropenaiservice2.openai.azure.com/";

        public string? OpenAIApiKey { get; set; }

        public string DeploymentName { get; set; } = "gpt-4o";
    }
}
