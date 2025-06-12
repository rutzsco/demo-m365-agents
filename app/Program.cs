using Microsoft.Agents.Builder;
using Microsoft.SemanticKernel;
using microsoft_agent_sk;
using microsoft_agent_sk.Agents;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}
builder.Services.AddHttpClient();
builder.Services.AddHttpClient("WeatherAPI", client =>
{
    client.BaseAddress = new Uri("https://api.weather.gov/");
});
builder.Services.AddControllers();

// Register Semantic Kernel
builder.Services.AddKernel();

// Register the AI service of your choice. AzureOpenAI and OpenAI are demonstrated...
var deploymentName = builder.Configuration.GetValue<string>("AzureOpenAIDeploymentName") ?? throw new ArgumentNullException("AzureOpenAIDeploymentName");
var endpoint = builder.Configuration.GetValue<string>("AzureOpenAIEndpoint") ?? throw new ArgumentNullException("AzureOpenAIEndpoint");
var apiKey = builder.Configuration.GetValue<string>("AzureOpenAIApiKey") ?? throw new ArgumentNullException("AzureOpenAIApiKey");

builder.Services.AddAzureOpenAIChatCompletion(
    deploymentName: deploymentName,
    endpoint: endpoint,
    apiKey: apiKey);

builder.Services.AddTransient<WeatherAgent>();

builder.AddBot<IAgent, BotHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapGet("/", () => "Microsoft 365 Agent SDK");
    app.UseDeveloperExceptionPage();
    app.MapControllers().AllowAnonymous();
}
else
{
    app.MapControllers();
}
app.Run();