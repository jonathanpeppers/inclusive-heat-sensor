using Inclusive.HeatSensor.Services;
using Microsoft.Extensions.Logging.ApplicationInsights;

var builder = WebApplication.CreateBuilder(args);

// Add configurations
builder.Services.Configure<LLMClientOptions>(
    builder.Configuration.GetSection(nameof(LLMClientOptions)));

// Add services to the container.
builder.Services.AddSingleton<LLMClient>();
builder.Services.AddControllers();
builder.Services.AddApplicationInsightsTelemetry();

builder.Services.AddLogging(loggingBuilder =>
{
    // Add Application Insights Logger
    loggingBuilder.AddApplicationInsights();

    // Optionally, set log levels (e.g., trace, information)
    loggingBuilder.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Information);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthorization();

app.MapControllers();

app.Run();
