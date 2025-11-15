# Tool Calling - Basics

## 📋 Overview
This project demonstrates how to give AI agents the ability to call functions (tools) to access external data and perform actions. Tool calling is essential for creating agents that can interact with real-world systems, APIs, and data sources.

## 🎯 Learning Objectives
- Understand tool calling (function calling) concepts
- Create and register custom functions
- Let agents decide when to call tools
- Handle tool execution and results
- Build interactive agent loops

## 🎥 Video Tutorials
- **Basic Tool Calling**: [Watch on YouTube](https://youtu.be/gJTodKpv8Ik)
- **Advanced Tool Calling**: [Watch on YouTube](https://youtu.be/dCtojrK8bKk)
- **MCP Integration**: [Watch on YouTube](https://youtu.be/Y5IKdt9vdJM)

## 📦 Prerequisites
- .NET 8.0 or higher
- Azure OpenAI resource with deployed model
- Understanding of basic agent creation

## 🔧 NuGet Packages Required
```xml
<PackageReference Include="Azure.AI.OpenAI" />
<PackageReference Include="Microsoft.Agents.AI" />
<PackageReference Include="Microsoft.Extensions.AI" />
```

## 🚀 Quick Start

### Step 1: Configure User Secrets
Set up your configuration using .NET User Secrets:
```bash
dotnet user-secrets set "AzureOpenAiEndpoint" "https://your-resource.openai.azure.com/"
dotnet user-secrets set "AzureOpenAiKey" "your-api-key"
dotnet user-secrets set "ChatDeploymentName" "gpt-4o"
```

### Step 2: Run the Application
```bash
dotnet run
```

### Step 3: Interact with the Agent
```
> What time is it?
> What timezone am I in?
> What's the date today?
```

## 💡 Code Explanation

### Creating Tools
```csharp
public static class Tools
{
    public static string CurrentDataAndTime() => DateTime.Now.ToString("F");
    
    public static string CurrentTimezone() => TimeZoneInfo.Local.DisplayName;
}
```
- Simple static methods that return information
- No special attributes needed
- Can have parameters and complex return types

### Registering Tools with Agent
```csharp
ChatClientAgent agent = client
    .GetChatClient(configuration.ChatDeploymentName)
    .CreateAIAgent(
        instructions: "You are a Time Expert",
        tools:
        [
            AIFunctionFactory.Create(Tools.CurrentDataAndTime, "current_date_and_time"),
            AIFunctionFactory.Create(Tools.CurrentTimezone, "current_timezone")
        ]
    );
```
- **AIFunctionFactory.Create**: Wraps C# methods as AI tools
- **First parameter**: Method reference
- **Second parameter**: Tool name (what AI sees)
- **Instructions**: Tells agent its role and when to use tools

### Interactive Loop with Threads
```csharp
AgentThread thread = agent.GetNewThread();

while (true)
{
    Console.Write("> ");
    string? input = Console.ReadLine();
    ChatMessage message = new(ChatRole.User, input);
    AgentRunResponse response = await agent.RunAsync(message, thread);
    Console.WriteLine(response);
}
```
- **Thread**: Maintains conversation history
- **Loop**: Allows continuous interaction
- Agent automatically decides when to call tools

## 🎓 Key Concepts

### How Tool Calling Works
1. **User asks question**: "What time is it?"
2. **Agent analyzes**: Determines it needs the time tool
3. **Tool execution**: Framework calls `CurrentDataAndTime()`
4. **Result to agent**: Time string returned to AI
5. **Agent responds**: "It is currently 3:45 PM on Tuesday..."

### Tool Calling Flow Diagram
```
User Question → Agent Reasoning → Tool Call Decision
                                        ↓
User Response ← Agent Synthesis ← Tool Execution
```

### When Agents Call Tools
Agents decide to call tools when:
- Question requires external data
- Instructions mention using tools
- User explicitly requests tool functionality
- Agent determines tool would help answer

### Tool Function Requirements
✅ **Can be:**
- Static or instance methods
- Synchronous or async
- Have parameters (primitive or complex types)
- Return values (string, objects, etc.)

✅ **Best practices:**
- Clear, descriptive names
- Good XML documentation (AI sees this!)
- Return serializable types
- Handle errors gracefully

## 🛠️ Advanced Tool Examples

### Tool with Parameters
```csharp
public static int Add(int a, int b) => a + b;

// Register:
AIFunctionFactory.Create(Add, "add_numbers")
```

### Async Tool
```csharp
public static async Task<string> GetWeather(string city)
{
    // Call external API
    return await weatherApi.GetCurrentWeather(city);
}
```

### Tool with Complex Types
```csharp
public static List<Product> SearchProducts(string query, int maxResults)
{
    // Return structured data
    return database.Search(query).Take(maxResults).ToList();
}
```

## 📊 Expected Output
```
> What time is it?
It is currently Tuesday, January 15, 2024 3:45:30 PM.

> What timezone am I in?
You are in (UTC-08:00) Pacific Time (US & Canada).
```

## 🎯 Use Cases
- **Data Retrieval**: Fetch real-time information (weather, stocks, news)
- **System Integration**: Query databases, APIs, internal systems
- **Actions**: Send emails, create tickets, update records
- **Calculations**: Perform complex math, data analysis
- **Personalization**: Access user preferences, history
- **Multi-step Workflows**: Chain multiple operations

## 🌟 Tool Calling Best Practices

### 1. Clear Instructions
```csharp
instructions: "You are a helpful assistant. Use the weather tool to get current conditions when users ask about weather."
```

### 2. Descriptive Names
```csharp
// Good
AIFunctionFactory.Create(GetWeather, "get_current_weather")

// Bad
AIFunctionFactory.Create(GetWeather, "gw")
```

### 3. XML Documentation
```csharp
/// <summary>
/// Gets the current weather for a specific city
/// </summary>
/// <param name="city">The city name (e.g., "Seattle")</param>
/// <returns>Current weather conditions</returns>
public static async Task<string> GetWeather(string city) { }
```

### 4. Error Handling
```csharp
public static string GetUserData(int userId)
{
    try
    {
        return database.GetUser(userId).ToString();
    }
    catch (Exception ex)
    {
        return $"Error: Could not retrieve user data - {ex.Message}";
    }
}
```

## 🔒 Security Considerations
- **Validate Input**: Always validate tool parameters
- **Rate Limiting**: Implement limits on expensive operations
- **Authentication**: Verify permissions before executing
- **Audit Logging**: Log all tool executions
- **Sandboxing**: Limit what tools can access
- **User Confirmation**: Require approval for destructive actions

## 💰 Cost Considerations
- Tool calls add extra tokens to requests
- Each tool call = additional API roundtrip
- Multiple tools increase context size
- Consider tool count vs performance trade-off
- Monitor function execution costs (external APIs)

## 🔗 Related Projects
- **ToolCalling.Advanced** - Complex tool scenarios
- **Toolcalling.FromAnMcpServer** - Model Context Protocol integration
- **MultiAgent.AgentAsTool** - Agents calling other agents
- **Workflow.AiAssisted.PizzaSample** - Tools in workflows
- **Trello.Agent** - Real-world tool integration

## 📚 Additional Resources
- [Function Calling Documentation](https://learn.microsoft.com/azure/ai-services/openai/how-to/function-calling)
- [Microsoft.Extensions.AI](https://learn.microsoft.com/dotnet/ai/)
- [Tool Calling Best Practices](https://platform.openai.com/docs/guides/function-calling)

## 🐛 Troubleshooting

### Agent Doesn't Call Tools
- Check instructions mention tools
- Ensure question requires tool
- Verify tool registration
- Try more explicit user prompts

### Tool Execution Errors
- Add error handling to tool functions
- Return error messages to agent
- Check parameter types match
- Verify async tools are properly awaited

### Performance Issues
- Reduce number of tools
- Optimize tool execution time
- Consider caching tool results
- Use parallel tool execution when possible

## 🎤 Presentation Talking Points
1. **Introduction** - Why agents need tools
2. **Concept** - How tool calling works (flow diagram)
3. **Simple Example** - Time and timezone tools
4. **Live Demo** - Interactive Q&A with tool calls
5. **Tool Creation** - How to write tool functions
6. **Registration** - Adding tools to agents
7. **Best Practices** - Naming, documentation, error handling
8. **Security** - Important considerations
9. **Real-world Examples** - Weather, database, API integration
10. **Advanced Patterns** - Async, parameters, complex types
11. **Performance** - Cost and optimization considerations
12. **Next Steps** - Advanced tool calling scenarios

