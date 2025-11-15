# Multi-Agent - Agent as Tool

## 📋 Overview
This project demonstrates how to create multi-agent systems where specialized agents can call other agents as tools. This enables complex workflows where a coordinator agent delegates tasks to expert agents, each focused on specific domains.

## 🎯 Learning Objectives
- Build multi-agent systems with specialization
- Use agents as tools for other agents
- Implement agent delegation patterns
- Create middleware for logging and monitoring
- Coordinate multiple AI agents effectively

## 🎥 Video Tutorial
[Watch on YouTube](https://youtu.be/wL4V78s_wI4)

## 📦 Prerequisites
- .NET 8.0 or higher
- Azure OpenAI resource with deployed model
- Understanding of tool calling basics
- Familiarity with agent creation

## 🔧 NuGet Packages Required
```xml
<PackageReference Include="Azure.AI.OpenAI" />
<PackageReference Include="Microsoft.Agents.AI" />
<PackageReference Include="Microsoft.Extensions.AI" />
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

### Step 3: Interact with the Delegation Agent
```
> Convert "hello world" to uppercase and then reverse it
> Give me a random number
```

## 💡 Code Explanation

### Creating Specialized Agents

#### String Manipulation Agent
```csharp
AIAgent stringAgent = client
    .GetChatClient(configuration.ChatDeploymentName)
    .CreateAIAgent(
        name: "StringAgent",
        instructions: "You are string manipulator",
        tools:
        [
            AIFunctionFactory.Create(StringTools.Reverse),
            AIFunctionFactory.Create(StringTools.Uppercase),
            AIFunctionFactory.Create(StringTools.Lowercase)
        ])
    .AsBuilder()
    .Use(FunctionCallMiddleware)  // Add logging
    .Build();
```

#### Number Expert Agent
```csharp
AIAgent numberAgent = client
    .GetChatClient(configuration.ChatDeploymentName)
    .CreateAIAgent(
        name: "NumberAgent",
        instructions: "You are a number expert",
        tools:
        [
            AIFunctionFactory.Create(NumberTools.RandomNumber),
            AIFunctionFactory.Create(NumberTools.AnswerToEverythingNumber)
        ])
    .AsBuilder()
    .Use(FunctionCallMiddleware)
    .Build();
```

### Creating the Delegation Agent
```csharp
AIAgent delegationAgent = client
    .GetChatClient(configuration.ChatDeploymentName)
    .CreateAIAgent(
        name: "DelegateAgent",
        instructions: "You coordinate tasks by delegating to specialized agents",
        tools:
        [
            AIFunctionFactory.Create(
                async (string task) => await stringAgent.RunAsync(task),
                name: "string_agent",
                description: "Delegate string manipulation tasks"
            ),
            AIFunctionFactory.Create(
                async (string task) => await numberAgent.RunAsync(task),
                name: "number_agent",
                description: "Delegate number-related tasks"
            )
        ]
    );
```

### Middleware for Logging
```csharp
private static async Task<AgentRunResponse> FunctionCallMiddleware(
    AgentRunRequest request,
    Func<AgentRunRequest, Task<AgentRunResponse>> next)
{
    // Log before function call
    Console.WriteLine($"[Middleware] Calling: {request.Message}");
    
    // Execute the actual call
    AgentRunResponse response = await next(request);
    
    // Log after function call
    Console.WriteLine($"[Middleware] Response: {response.Text}");
    
    return response;
}
```

## 🎓 Key Concepts

### Multi-Agent Architecture
```
User Request
    ↓
Delegation Agent (Coordinator)
    ↓
    ├─→ String Agent (Specialist)
    │   └─→ String Tools
    │
    └─→ Number Agent (Specialist)
        └─→ Number Tools
```

### Agent Specialization Benefits
- **Focused Expertise**: Each agent is expert in its domain
- **Easier Maintenance**: Changes isolated to specific agents
- **Better Performance**: Smaller context per agent
- **Reusability**: Agents can be reused across systems
- **Scalability**: Add new agents without changing existing ones

### Agents as Tools Pattern
```csharp
AIFunctionFactory.Create(
    async (string task) => await specializedAgent.RunAsync(task),
    name: "agent_name",
    description: "When to use this agent"
)
```
- Wraps an agent as a callable tool
- Delegation agent decides which agent to call
- Each agent maintains its own context

### Middleware Pattern
Middleware allows you to:
- Log all agent interactions
- Monitor performance
- Add authentication
- Transform requests/responses
- Implement retry logic
- Collect metrics

## 🛠️ Advanced Patterns

### Sequential Agent Calls
```csharp
// Task: "Uppercase 'hello' then reverse it"
1. Delegation agent calls StringAgent with "uppercase 'hello'"
2. StringAgent returns "HELLO"
3. Delegation agent calls StringAgent with "reverse 'HELLO'"
4. StringAgent returns "OLLEH"
5. Delegation agent responds to user
```

### Parallel Agent Calls
```csharp
// Task: "Get a random number and uppercase 'world'"
1. Delegation agent identifies two independent tasks
2. Calls NumberAgent and StringAgent concurrently
3. Combines results
4. Responds to user
```

### Hierarchical Agents
```
CEO Agent
├─→ Finance Agent
│   ├─→ Budget Agent
│   └─→ Invoice Agent
└─→ HR Agent
    ├─→ Recruitment Agent
    └─→ Payroll Agent
```

## 📊 Expected Output
```
DELEGATE AGENT
> Convert "hello world" to uppercase
[Middleware] StringAgent: Calling uppercase tool
[Middleware] StringAgent: Response: HELLO WORLD
Result: HELLO WORLD

> Give me a random number
[Middleware] NumberAgent: Calling random number tool
[Middleware] NumberAgent: Response: 42
Result: The random number is 42
```

## 🎯 Use Cases
- **Customer Service**: Route to specialized support agents
- **Enterprise Automation**: Different agents for different departments
- **Data Processing**: Agents for extraction, transformation, loading
- **Content Creation**: Research agent → Writing agent → Editing agent
- **E-commerce**: Product agent, Inventory agent, Shipping agent
- **Healthcare**: Diagnosis agent, Treatment agent, Billing agent

## 🌟 Best Practices

### 1. Clear Agent Responsibilities
```csharp
// Good: Focused responsibility
instructions: "You handle all string manipulation tasks"

// Bad: Unclear scope
instructions: "You do stuff with text and maybe numbers"
```

### 2. Descriptive Tool Names
```csharp
// Good
AIFunctionFactory.Create(
    async (string task) => await agent.RunAsync(task),
    name: "financial_analysis_agent",
    description: "Use for budgets, forecasts, and financial reports"
)

// Bad
AIFunctionFactory.Create(
    async (string task) => await agent.RunAsync(task),
    name: "agent1",
    description: "Does things"
)
```

### 3. Implement Monitoring
```csharp
.AsBuilder()
.Use(async (request, next) => {
    var sw = Stopwatch.StartNew();
    var response = await next(request);
    Console.WriteLine($"Agent took {sw.ElapsedMilliseconds}ms");
    return response;
})
.Build();
```

### 4. Error Handling
```csharp
AIFunctionFactory.Create(
    async (string task) => {
        try {
            return await specializedAgent.RunAsync(task);
        }
        catch (Exception ex) {
            return $"Agent failed: {ex.Message}";
        }
    },
    name: "agent_name"
)
```

## 🚨 Common Pitfalls

### ❌ Circular Dependencies
```csharp
// Agent A calls Agent B
// Agent B calls Agent A
// = Infinite loop!
```

### ❌ Too Many Agents
- More agents = more complexity
- More agents = higher costs
- Start with 2-3, add only when needed

### ❌ Unclear Delegation
```csharp
// Vague instructions lead to wrong agent selection
instructions: "You are a helper"  // Too vague!
```

### ✅ Better Approach
```csharp
instructions: "You coordinate tasks. Use string_agent for text manipulation, number_agent for calculations."
```

## 💰 Cost Considerations
- Each agent call = separate API call
- Delegation adds overhead (coordinator decides which agent)
- Multiple agents can increase costs 2-3x
- Benefits often outweigh costs (better results, maintainability)
- Monitor and optimize agent selection logic

## 🔗 Related Projects
- **MultiAgent.ManualViaStructuredOutput** - Manual coordination approach
- **Workflow.Sequential** - Orchestrated multi-agent patterns
- **Workflow.Handoff** - Agent handoff patterns
- **Agent2Agent.Client/Server** - Distributed agents
- **ToolCalling.Basics** - Foundation for agent tools

## 📚 Additional Resources
- [Multi-Agent Systems](https://en.wikipedia.org/wiki/Multi-agent_system)
- [Agent Middleware Documentation](https://learn.microsoft.com/dotnet/ai/)
- [Delegation Patterns](https://learn.microsoft.com/azure/ai-services/openai/how-to/function-calling)

## 🐛 Troubleshooting

### Agent Calls Wrong Sub-Agent
- Improve delegation agent instructions
- Make agent descriptions more specific
- Add examples to agent descriptions
- Test with clear, unambiguous requests

### Performance Issues
- Too many delegation levels
- Agents making unnecessary calls
- Consider parallel execution
- Cache agent responses when possible

### Circular Calls
- Review agent tool registrations
- Ensure clear hierarchy
- Add call depth limits
- Implement circuit breakers

### Debugging Multi-Agent Systems
- Use middleware to log all calls
- Track call chains with correlation IDs
- Monitor token usage per agent
- Visualize agent interaction flows

## 🎤 Presentation Talking Points
1. **Introduction** - Why multi-agent systems?
2. **Architecture** - Coordinator + Specialists pattern
3. **Agent Specialization** - Benefits of focused agents
4. **Creating Specialists** - StringAgent and NumberAgent examples
5. **Agents as Tools** - Wrapping agents for delegation
6. **Delegation Agent** - The coordinator's role
7. **Live Demo** - Multi-step task delegation
8. **Middleware** - Logging and monitoring
9. **Advanced Patterns** - Hierarchical, parallel agents
10. **Real-world Use Cases** - Customer service, automation
11. **Best Practices** - Clear responsibilities, monitoring
12. **Common Pitfalls** - Circular deps, over-engineering
13. **Cost Considerations** - When multi-agent makes sense
14. **Next Steps** - Workflows and orchestration

