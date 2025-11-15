# Dependency Injection

## 📋 Overview
This project demonstrates how to integrate AI agents into ASP.NET Core applications using dependency injection. Build scalable web applications with properly managed AI agent lifecycles, configuration, and best practices for production deployment.

## 🎯 Learning Objectives
- Use dependency injection with AI agents
- Integrate agents into ASP.NET Core
- Manage agent lifecycle and configuration
- Build interactive Blazor applications with AI
- Apply production-ready patterns

## 🎥 Video Tutorial
[Watch on YouTube](https://youtu.be/q-mHdd6iJJo)

## 📦 Prerequisites
- .NET 8.0 or higher
- Azure OpenAI resource with deployed model
- Understanding of ASP.NET Core and dependency injection
- Basic Blazor knowledge (helpful)

## 🔧 NuGet Packages Required
```xml
<PackageReference Include="Azure.AI.OpenAI" />
<PackageReference Include="Microsoft.Agents.AI" />
```

## 🚀 Quick Start

### Step 1: Configure User Secrets
```bash
dotnet user-secrets set "AzureOpenAiEndpoint" "https://your-resource.openai.azure.com/"
dotnet user-secrets set "AzureOpenAiKey" "your-api-key"
dotnet user-secrets set "ChatDeploymentName" "gpt-4o"
```

### Step 2: Run the Application
```bash
dotnet run
```

### Step 3: Open Browser
Navigate to `https://localhost:5001` (or displayed URL)

## 💡 Code Explanation

### Service Registration in Program.cs
```csharp
// Get configuration from user secrets
Configuration configuration = ConfigurationManager.GetConfiguration();

// Register Azure OpenAI Client as Singleton
AzureOpenAIClient client = new AzureOpenAIClient(
    new Uri(configuration.AzureOpenAiEndpoint), 
    new ApiKeyCredential(configuration.AzureOpenAiKey)
);
builder.Services.AddSingleton(client);

// Register ChatClient with key for specific model
ChatClient chatClient = client.GetChatClient("gpt-4o");
builder.Services.AddKeyedSingleton("gpt-4o", chatClient);

// Register Agent with key
ChatClientAgent agent = chatClient.CreateAIAgent();
builder.Services.AddKeyedSingleton("gpt-4o", agent);
```

### Injecting into Blazor Components
```csharp
@inject IKeyedServiceProvider KeyedServiceProvider

@code {
    private ChatClientAgent? agent;
    
    protected override void OnInitialized()
    {
        agent = KeyedServiceProvider.GetKeyedService<ChatClientAgent>("gpt-4o");
    }
    
    private async Task SendMessage()
    {
        var response = await agent!.RunAsync(userMessage);
        // Handle response
    }
}
```

### Alternative: Direct Injection
```csharp
@inject ChatClientAgent Agent

@code {
    private async Task SendMessage()
    {
        var response = await Agent.RunAsync(userMessage);
    }
}
```

## 🎓 Key Concepts

### Service Lifetimes

**Singleton** (Recommended for AI Clients):
```csharp
builder.Services.AddSingleton<AzureOpenAIClient>(client);
```
- Created once, shared across all requests
- Best for clients and agents (expensive to create)
- Thread-safe

**Scoped** (For Request-Specific State):
```csharp
builder.Services.AddScoped<ConversationManager>();
```
- Created per HTTP request
- Good for conversation threads
- Isolated per user request

**Transient** (Usually Not Needed):
```csharp
builder.Services.AddTransient<Helper>();
```
- Created every time requested
- Not ideal for agents (overhead)
- Use for lightweight services

### Keyed Services
```csharp
// Register multiple models
builder.Services.AddKeyedSingleton("gpt-4o", chatClient1);
builder.Services.AddKeyedSingleton("gpt-4o-mini", chatClient2);

// Inject specific model
var agent = KeyedServiceProvider.GetKeyedService<ChatClientAgent>("gpt-4o");
```
- Supports multiple agents/models
- Select at runtime
- Clean configuration

## 🛠️ Advanced Patterns

### Configuration Options Pattern
```csharp
public class AiOptions
{
    public string Endpoint { get; set; }
    public string ApiKey { get; set; }
    public string ModelName { get; set; }
}

// In Program.cs
builder.Services.Configure<AiOptions>(
    builder.Configuration.GetSection("AzureOpenAI")
);

// In component
@inject IOptions<AiOptions> Options

@code {
    private void Initialize()
    {
        var config = Options.Value;
    }
}
```

### Factory Pattern
```csharp
public interface IAgentFactory
{
    ChatClientAgent CreateAgent(string modelName);
}

public class AgentFactory : IAgentFactory
{
    private readonly AzureOpenAIClient _client;
    
    public AgentFactory(AzureOpenAIClient client)
    {
        _client = client;
    }
    
    public ChatClientAgent CreateAgent(string modelName)
    {
        return _client.GetChatClient(modelName).CreateAIAgent();
    }
}

// Registration
builder.Services.AddSingleton<IAgentFactory, AgentFactory>();
```

### Scoped Conversation Management
```csharp
public class ConversationService
{
    private readonly ChatClientAgent _agent;
    private readonly AgentThread _thread;
    
    public ConversationService(ChatClientAgent agent)
    {
        _agent = agent;
        _thread = agent.GetNewThread();
    }
    
    public async Task<string> SendMessageAsync(string message)
    {
        var response = await _agent.RunAsync(message, _thread);
        return response.Text;
    }
}

// Register as Scoped (per request)
builder.Services.AddScoped<ConversationService>();
```

### Health Checks
```csharp
public class AiAgentHealthCheck : IHealthCheck
{
    private readonly ChatClientAgent _agent;
    
    public AiAgentHealthCheck(ChatClientAgent agent)
    {
        _agent = agent;
    }
    
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _agent.RunAsync("Health check");
            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(ex.Message);
        }
    }
}

// Registration
builder.Services.AddHealthChecks()
    .AddCheck<AiAgentHealthCheck>("ai-agent");
```

## 📊 Example Application Structure
```
Program.cs              - Service registration
Components/
  ├── Chat.razor        - Chat UI component
  ├── AgentSelector.razor - Model selection
  └── ConversationList.razor - History
Services/
  ├── ConversationService.cs - Scoped conversation
  ├── AgentFactory.cs   - Agent creation
  └── HistoryService.cs - Conversation history
```

## 🎯 Use Cases
- **Web Chatbots**: Customer-facing chat applications
- **Internal Tools**: Employee assistance portals
- **APIs**: RESTful AI endpoints
- **Blazor Apps**: Interactive web applications
- **Minimal APIs**: Lightweight AI services
- **Background Services**: Scheduled AI tasks

## 🌟 Best Practices

### 1. Use Singleton for Clients
```csharp
// Good: Reuse expensive clients
builder.Services.AddSingleton<AzureOpenAIClient>(client);

// Bad: Recreate every request
builder.Services.AddTransient<AzureOpenAIClient>(...);
```

### 2. Scoped Conversations
```csharp
// Good: Isolate per request
builder.Services.AddScoped<ConversationService>();

// Bad: Shared state across users
builder.Services.AddSingleton<ConversationService>();
```

### 3. Configuration from Settings
```csharp
// appsettings.json
{
  "AzureOpenAI": {
    "Endpoint": "...",
    "ModelName": "gpt-4o"
  }
}

// Use IConfiguration, not hardcoded values
var endpoint = builder.Configuration["AzureOpenAI:Endpoint"];
```

### 4. Error Handling
```csharp
@code {
    private async Task SendMessage()
    {
        try
        {
            var response = await agent.RunAsync(userMessage);
        }
        catch (Exception ex)
        {
            // Log error, show user-friendly message
            errorMessage = "Failed to get response. Please try again.";
        }
    }
}
```

### 5. Loading States
```csharp
@if (isLoading)
{
    <div>Loading...</div>
}
else if (!string.IsNullOrEmpty(errorMessage))
{
    <div class="alert alert-danger">@errorMessage</div>
}
else
{
    <div>@response</div>
}
```

## 💰 Cost Considerations
- Singleton clients reduce connection overhead
- Properly dispose of resources
- Implement rate limiting for public endpoints
- Monitor usage per user/session
- Consider caching frequent queries

## 🔗 Related Projects
- **Agent2Agent.Client/Server** - Distributed agent architecture
- **AgentUserInteraction.Client/Server** - User interaction patterns
- **AspireBlazorDemo** - .NET Aspire integration
- **DevUI** - Development UI tools
- **Telemetry** - Add monitoring to DI apps

## 📚 Additional Resources
- [Dependency Injection in .NET](https://learn.microsoft.com/aspnet/core/fundamentals/dependency-injection)
- [Blazor Documentation](https://learn.microsoft.com/aspnet/core/blazor/)
- [Keyed Services](https://learn.microsoft.com/dotnet/core/extensions/dependency-injection#keyed-services)
- [ASP.NET Core Best Practices](https://learn.microsoft.com/aspnet/core/fundamentals/best-practices)

## 🐛 Troubleshooting

### Service Not Found
- Verify service is registered in Program.cs
- Check service lifetime matches usage
- Ensure correct service type

### Memory Leaks
- Use Singleton for clients (not Transient)
- Dispose of resources properly
- Monitor memory usage
- Check for unclosed threads

### Concurrency Issues
- Agents are not thread-safe by default
- Use scoped services for user-specific state
- Implement proper locking if needed

### Configuration Not Loading
- Check user secrets are set
- Verify appsettings.json format
- Ensure configuration section names match
- Check environment variables

## 🎤 Presentation Talking Points
1. **Introduction** - AI in ASP.NET Core applications
2. **Dependency Injection Benefits** - Testability, maintainability
3. **Service Registration** - Singleton, Scoped, Transient
4. **Keyed Services** - Multiple models support
5. **Live Demo** - Blazor chat application
6. **Lifecycle Management** - When to use which lifetime
7. **Advanced Patterns** - Factory, options, health checks
8. **Best Practices** - Configuration, error handling
9. **Production Considerations** - Scaling, monitoring
10. **Real-world Architecture** - Multi-layer applications
# Telemetry

## 📋 Overview
This project demonstrates how to add observability to your AI agents using OpenTelemetry. Track agent interactions, monitor performance, debug issues, and send telemetry to Azure Application Insights or other observability platforms.

## 🎯 Learning Objectives
- Implement OpenTelemetry in AI agents
- Track agent calls and performance
- Send telemetry to Azure Application Insights
- Monitor token usage and costs
- Debug agent behavior in production

## 🎥 Video Tutorial
[Watch on YouTube](https://youtu.be/jeVQo75KcCw)

## 📦 Prerequisites
- .NET 8.0 or higher
- Azure OpenAI resource with deployed model
- Azure Application Insights (optional but recommended)
- Understanding of basic agent creation

## 🔧 NuGet Packages Required
```xml
<PackageReference Include="Azure.AI.OpenAI" />
<PackageReference Include="Microsoft.Agents.AI" />
<PackageReference Include="OpenTelemetry" />
<PackageReference Include="OpenTelemetry.Exporter.Console" />
<PackageReference Include="Azure.Monitor.OpenTelemetry.Exporter" />
```

## 🚀 Quick Start

### Step 1: Configure User Secrets
```bash
dotnet user-secrets set "AzureOpenAiEndpoint" "https://your-resource.openai.azure.com/"
dotnet user-secrets set "AzureOpenAiKey" "your-api-key"
dotnet user-secrets set "ChatDeploymentName" "gpt-4o"
dotnet user-secrets set "ApplicationInsightsConnectionString" "InstrumentationKey=..."
```

### Step 2: Run the Application
```bash
dotnet run
```

## 💡 Code Explanation

### Setting Up Telemetry
```csharp
string sourceName = "AiSource";
var tracerProviderBuilder = Sdk.CreateTracerProviderBuilder()
    .AddSource(sourceName)
    .AddConsoleExporter();  // View in console

if (!string.IsNullOrWhiteSpace(configuration.ApplicationInsightsConnectionString))
{
    tracerProviderBuilder.AddAzureMonitorTraceExporter(options => 
        options.ConnectionString = configuration.ApplicationInsightsConnectionString
    );
}

using TracerProvider tracerProvider = tracerProviderBuilder.Build();
```

### Creating Observed Agent
```csharp
AIAgent agent = client
    .GetChatClient(configuration.ChatDeploymentName)
    .CreateAIAgent(
        name: "MyObservedAgent",
        instructions: "You are a Friendly AI Bot, answering questions")
    .AsBuilder()
    .UseOpenTelemetry(sourceName, options =>
    {
        options.EnableSensitiveData = true;  // Log actual messages
    })
    .Build();
```

### Using the Agent with Telemetry
```csharp
AgentThread thread = agent.GetNewThread();

AgentRunResponse response1 = await agent.RunAsync("Hello, My name is Rasmus", thread);
// Automatically logged to telemetry

AgentRunResponse response2 = await agent.RunAsync("What is the capital of France?", thread);
// All interactions tracked

AgentRunResponse response3 = await agent.RunAsync("What was my name?", thread);
// Full conversation history visible in traces
```

## 🎓 Key Concepts

### What is OpenTelemetry?
- **Industry standard** for observability
- **Vendor-neutral** (works with many platforms)
- **Three pillars**: Traces, Metrics, Logs
- **Automatic instrumentation** for AI agents

### Telemetry Data Captured
- **Request/Response**: Full conversation details
- **Timestamps**: Request start/end times
- **Duration**: How long each call took
- **Token Usage**: Input/output token counts
- **Model Information**: Which model was used
- **Thread Context**: Conversation thread IDs
- **Errors**: Exceptions and failures
- **Custom Events**: Your own tracking

### Sensitive Data Option
```csharp
options.EnableSensitiveData = true;
```
- **True**: Logs actual messages (for debugging)
- **False**: Logs metadata only (for privacy)

### Console Exporter
```csharp
.AddConsoleExporter()
```
- Writes telemetry to console output
- Great for development and debugging
- See traces in real-time

### Azure Application Insights
```csharp
.AddAzureMonitorTraceExporter(options => 
    options.ConnectionString = connectionString
)
```
- Production-grade telemetry platform
- Powerful querying and visualization
- Alerts and dashboards
- Cost tracking and analysis

## 📊 Telemetry Output

### Console Output Example
```
Activity: Agent.Run
  Duration: 1.234s
  Attributes:
    - agent.name: MyObservedAgent
    - model: gpt-4o
    - thread.id: thread_abc123
    - prompt.tokens: 45
    - completion.tokens: 28
    - total.tokens: 73
    - user.message: "Hello, My name is Rasmus"
    - agent.response: "Hello Rasmus! Nice to meet you..."
```

### Application Insights Queries
```kusto
// Find slow requests
traces
| where customDimensions.duration_ms > 2000
| order by timestamp desc

// Track token usage
traces
| summarize TotalTokens = sum(toint(customDimensions.total_tokens)) by bin(timestamp, 1h)
| render timechart

// Error analysis
traces
| where severityLevel >= 3
| summarize count() by customDimensions.agent_name
```

## 🎯 Use Cases
- **Performance Monitoring**: Track response times
- **Cost Analysis**: Monitor token usage and costs
- **Debugging**: Understand agent behavior
- **Quality Assurance**: Review conversations
- **Compliance**: Audit trails for regulated industries
- **Optimization**: Identify bottlenecks
- **Alerting**: Get notified of failures

## 🌟 Best Practices

### 1. Separate Sources by Environment
```csharp
string sourceName = $"AiAgent-{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}";
```

### 2. Control Sensitive Data
```csharp
// Development: See everything
options.EnableSensitiveData = isDevelopment;

// Production: Privacy-conscious
options.EnableSensitiveData = false;
```

### 3. Add Custom Attributes
```csharp
using var activity = activitySource.StartActivity("CustomOperation");
activity?.SetTag("user.id", userId);
activity?.SetTag("session.id", sessionId);
activity?.SetTag("feature.flag", featureName);
```

### 4. Sampling for High Volume
```csharp
.SetSampler(new TraceIdRatioBasedSampler(0.1))  // 10% sampling
```

### 5. Multiple Exporters
```csharp
.AddConsoleExporter()              // Local debugging
.AddAzureMonitorTraceExporter()    // Production monitoring
.AddOtlpExporter()                 // Custom backend
```

## 🔒 Privacy and Compliance

### GDPR Considerations
```csharp
// Don't log sensitive user data
options.EnableSensitiveData = false;

// Implement data retention policies
// Anonymize user identifiers
// Allow data deletion requests
```

### HIPAA Compliance
```csharp
// Healthcare data requires strict controls
// Use Azure private endpoints
// Enable encryption at rest
// Implement access controls
// Regular audit log reviews
```

### PCI-DSS
```csharp
// Never log payment information
// Mask sensitive fields
// Implement strong access controls
```

## 💰 Cost Considerations

### Application Insights Pricing
- **Free tier**: 5GB/month included
- **Pay-as-you-go**: ~$2.30/GB after free tier
- **Data retention**: 90 days (default, configurable)
- **High-volume discounts** available

### Cost Optimization
```csharp
// Sampling reduces data volume
.SetSampler(new TraceIdRatioBasedSampler(0.1))

// Filter out noisy traces
.AddProcessor(new FilteringProcessor())

// Batch exports
.AddBatchExportProcessor()
```

## 🔗 Related Projects
- **DependencyInjection** - Telemetry in ASP.NET Core apps
- **Agent2Agent.Client/Server** - Distributed tracing
- **MultiAgent.AgentAsTool** - Multi-agent telemetry
- **Workflow.Sequential** - Workflow observability

## 📚 Additional Resources
- [OpenTelemetry .NET](https://opentelemetry.io/docs/languages/net/)
- [Application Insights Documentation](https://learn.microsoft.com/azure/azure-monitor/app/app-insights-overview)
- [Kusto Query Language](https://learn.microsoft.com/azure/data-explorer/kusto/query/)
- [Distributed Tracing](https://learn.microsoft.com/azure/azure-monitor/app/distributed-tracing)

## 🐛 Troubleshooting

### No Telemetry Appearing
- Verify TracerProvider is built
- Check source name matches
- Ensure .UseOpenTelemetry() is called
- Verify exporter configuration

### Application Insights Not Receiving Data
- Check connection string format
- Verify network connectivity
- Check Azure firewall rules
- Wait 2-3 minutes for ingestion delay

### Too Much Data
- Enable sampling
- Disable sensitive data logging
- Filter unnecessary traces
- Adjust log levels

### Performance Impact
- Telemetry adds minimal overhead (<5%)
- Use async exporters
- Batch processing
- Consider sampling for high-volume

## 🎤 Presentation Talking Points
1. **Introduction** - Why observability matters
2. **The Problem** - Black box AI agents
3. **OpenTelemetry** - Industry standard solution
4. **Setup** - Quick and easy integration
5. **Console Exporter** - See traces locally
6. **Azure Application Insights** - Production monitoring
7. **Live Demo** - Watch telemetry in real-time
8. **Sensitive Data** - Privacy considerations
9. **Application Insights Queries** - Powerful analysis
10. **Use Cases** - Performance, cost, debugging
11. **Best Practices** - Sampling, filtering, privacy
12. **Cost Management** - Optimization strategies
13. **Next Steps** - Distributed tracing, custom metrics

