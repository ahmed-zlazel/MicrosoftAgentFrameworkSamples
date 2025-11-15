# Token Usage

## 📋 Overview
This project demonstrates how to monitor and track token usage with AI agents. Understanding token consumption is critical for cost management, performance optimization, and staying within API limits.

## 🎯 Learning Objectives
- Track input and output tokens
- Calculate API costs
- Monitor token usage patterns
- Optimize for token efficiency
- Stay within rate limits

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
```bash
dotnet user-secrets set "AzureOpenAiEndpoint" "https://your-resource.openai.azure.com/"
dotnet user-secrets set "AzureOpenAiKey" "your-api-key"
dotnet user-secrets set "ChatDeploymentName" "gpt-4o"
```

### Step 2: Run the Application
```bash
dotnet run
```

## 💡 Code Explanation

### Accessing Token Usage
```csharp
AgentRunResponse response = await agent.RunAsync("What is the capital of France?");

// Access usage information
UsageDetails usage = response.Usage;

Console.WriteLine($"Input Tokens: {usage.InputTokenCount}");
Console.WriteLine($"Output Tokens: {usage.OutputTokenCount}");
Console.WriteLine($"Total Tokens: {usage.TotalTokenCount}");
```

### Token Usage with Streaming
```csharp
UsageDetails? totalUsage = null;

await foreach (AgentRunResponseUpdate update in agent.RunStreamingAsync("Tell me a story"))
{
    Console.Write(update.Text);
    
    if (update.Usage != null)
    {
        totalUsage = update.Usage;
    }
}

if (totalUsage != null)
{
    Console.WriteLine($"\nTotal tokens used: {totalUsage.TotalTokenCount}");
}
```

### Accumulating Usage Across Conversation
```csharp
AgentThread thread = agent.GetNewThread();
int totalTokens = 0;

for (int i = 0; i < 5; i++)
{
    var response = await agent.RunAsync($"Message {i}", thread);
    totalTokens += response.Usage.TotalTokenCount;
    Console.WriteLine($"Cumulative tokens: {totalTokens}");
}
```

## 🎓 Key Concepts

### What Are Tokens?
- **Token**: Basic unit of text processing
- **Not characters**: 1 token ≈ 4 characters (English)
- **Not words**: 1 token ≈ 0.75 words (English)
- **Model specific**: Different models tokenize differently

### Token Examples
```
"Hello" = 1 token
"Hello, world!" = 4 tokens
"artificial intelligence" = 2 tokens
"AI" = 1 token
"ChatGPT" = 2 tokens (Chat + GPT)
```

### Token Types

**Input Tokens (Prompt):**
- Your question/message
- System instructions
- Conversation history
- Tool definitions
- You pay for these

**Output Tokens (Completion):**
- AI's response
- Generated text
- Usually more expensive than input tokens

**Total Tokens:**
```
Total = Input + Output
```

### Pricing Models (Approximate)

**GPT-4o:**
- Input: $2.50 per 1M tokens
- Output: $10.00 per 1M tokens

**GPT-4o-mini:**
- Input: $0.15 per 1M tokens
- Output: $0.60 per 1M tokens

**GPT-3.5-turbo:**
- Input: $0.50 per 1M tokens
- Output: $1.50 per 1M tokens

## 💰 Cost Calculation

### Calculate Cost
```csharp
public class TokenCostCalculator
{
    private const decimal GPT4O_INPUT_COST_PER_1M = 2.50m;
    private const decimal GPT4O_OUTPUT_COST_PER_1M = 10.00m;
    
    public decimal CalculateCost(UsageDetails usage)
    {
        decimal inputCost = (usage.InputTokenCount / 1_000_000m) * GPT4O_INPUT_COST_PER_1M;
        decimal outputCost = (usage.OutputTokenCount / 1_000_000m) * GPT4O_OUTPUT_COST_PER_1M;
        return inputCost + outputCost;
    }
    
    public string FormatCost(UsageDetails usage)
    {
        decimal cost = CalculateCost(usage);
        return $"${cost:F6} (Input: {usage.InputTokenCount}, Output: {usage.OutputTokenCount})";
    }
}
```

### Usage Tracking Service
```csharp
public class UsageTracker
{
    private int _totalInputTokens = 0;
    private int _totalOutputTokens = 0;
    private decimal _totalCost = 0;
    
    public void Track(UsageDetails usage)
    {
        _totalInputTokens += usage.InputTokenCount;
        _totalOutputTokens += usage.OutputTokenCount;
        _totalCost += CalculateCost(usage);
    }
    
    public void PrintSummary()
    {
        Console.WriteLine($"=== Usage Summary ===");
        Console.WriteLine($"Total Input Tokens: {_totalInputTokens:N0}");
        Console.WriteLine($"Total Output Tokens: {_totalOutputTokens:N0}");
        Console.WriteLine($"Total Tokens: {_totalInputTokens + _totalOutputTokens:N0}");
        Console.WriteLine($"Estimated Cost: ${_totalCost:F4}");
    }
}
```

## 📊 Token Usage Patterns

### Short Query
```
Input: "What is 2+2?" (5 tokens)
Output: "2+2 equals 4." (7 tokens)
Total: 12 tokens
Cost: ~$0.00001
```

### Medium Conversation
```
Input: Question + history (200 tokens)
Output: Detailed response (300 tokens)
Total: 500 tokens
Cost: ~$0.003
```

### Long Document Analysis
```
Input: Document + question (5,000 tokens)
Output: Analysis (1,000 tokens)
Total: 6,000 tokens
Cost: ~$0.025
```

### With Thread History
```
Turn 1: 50 tokens total
Turn 2: 100 tokens total (includes turn 1)
Turn 3: 150 tokens total (includes turns 1-2)
Turn 4: 200 tokens total (includes turns 1-3)
```

## 🎯 Token Optimization Strategies

### 1. Use Shorter Prompts
```csharp
// Verbose (30 tokens)
"Could you please tell me what the current time is right now?"

// Concise (8 tokens)
"What time is it?"
```

### 2. Limit Output Length
```csharp
var options = new ChatOptions
{
    MaxOutputTokens = 100  // Limit response size
};
await agent.RunAsync(message, options: options);
```

### 3. Truncate Conversation History
```csharp
// Keep only last N messages
var recentMessages = conversationHistory.TakeLast(10);
```

### 4. Use Cheaper Models
```csharp
// Expensive: GPT-4o for simple tasks
var expensiveAgent = client.GetChatClient("gpt-4o").CreateAIAgent();

// Efficient: GPT-4o-mini for simple tasks
var efficientAgent = client.GetChatClient("gpt-4o-mini").CreateAIAgent();
```

### 5. Summarize Long Conversations
```csharp
if (thread.MessageCount > 20)
{
    var summary = await SummarizeConversationAsync(thread);
    thread = StartNewThreadWithSummary(summary);
}
```

### 6. Cache Responses
```csharp
private Dictionary<string, string> _cache = new();

public async Task<string> GetResponseAsync(string question)
{
    if (_cache.ContainsKey(question))
    {
        return _cache[question]; // No tokens used!
    }
    
    var response = await agent.RunAsync(question);
    _cache[question] = response.Text;
    return response.Text;
}
```

### 7. Batch Similar Queries
```csharp
// Instead of 3 separate calls:
"What is the capital of France?"
"What is the capital of Germany?"
"What is the capital of Italy?"

// One call:
"What are the capitals of France, Germany, and Italy?"
```

## 🚨 Rate Limits

### Token-Based Limits
- **Tokens Per Minute (TPM)**: Maximum tokens per minute
- **Requests Per Minute (RPM)**: Maximum requests per minute

### Example Limits
| Model | TPM | RPM |
|-------|-----|-----|
| GPT-4o | 30,000 | 500 |
| GPT-4o-mini | 200,000 | 2,000 |
| GPT-3.5-turbo | 90,000 | 3,500 |

### Handling Rate Limits
```csharp
public async Task<AgentRunResponse> RunWithRetryAsync(string message)
{
    int maxRetries = 3;
    int retryDelay = 1000; // ms
    
    for (int i = 0; i < maxRetries; i++)
    {
        try
        {
            return await agent.RunAsync(message);
        }
        catch (Exception ex) when (ex.Message.Contains("rate limit"))
        {
            if (i == maxRetries - 1) throw;
            await Task.Delay(retryDelay * (int)Math.Pow(2, i));
        }
    }
    
    throw new Exception("Max retries exceeded");
}
```

## 📈 Monitoring and Alerting

### Real-time Monitoring
```csharp
public class TokenMonitor
{
    private int _dailyTokens = 0;
    private const int DAILY_LIMIT = 1_000_000;
    
    public bool CheckAndTrack(UsageDetails usage)
    {
        _dailyTokens += usage.TotalTokenCount;
        
        if (_dailyTokens > DAILY_LIMIT * 0.9)
        {
            Console.WriteLine("⚠️ Warning: 90% of daily token limit reached!");
        }
        
        if (_dailyTokens > DAILY_LIMIT)
        {
            Console.WriteLine("🛑 Daily token limit exceeded!");
            return false;
        }
        
        return true;
    }
}
```

### Budget Alerts
```csharp
public class BudgetMonitor
{
    private decimal _monthlySpend = 0;
    private const decimal MONTHLY_BUDGET = 100m; // $100
    
    public void TrackSpend(UsageDetails usage)
    {
        decimal cost = CalculateCost(usage);
        _monthlySpend += cost;
        
        if (_monthlySpend > MONTHLY_BUDGET * 0.8m)
        {
            SendAlert($"80% of budget used: ${_monthlySpend:F2}");
        }
    }
}
```

## 🎯 Use Cases
- **Cost Management**: Track spending per user/feature
- **Performance Optimization**: Identify token-heavy operations
- **Rate Limit Management**: Stay within API quotas
- **Billing**: Charge customers based on usage
- **Analytics**: Understand usage patterns
- **Budget Alerts**: Prevent overspending

## 🔗 Related Projects
- **Telemetry** - Comprehensive monitoring with OpenTelemetry
- **ConversationThreads** - Token growth in conversations
- **UsingRAGInAgentFramework** - Token optimization with RAG
- **StructuredOutput** - Predictable token usage

## 📚 Additional Resources
- [Token Counting](https://platform.openai.com/tokenizer)
- [Pricing Information](https://azure.microsoft.com/pricing/details/cognitive-services/openai-service/)
- [Rate Limits](https://learn.microsoft.com/azure/ai-services/openai/quotas-limits)
- [Cost Management](https://learn.microsoft.com/azure/cost-management-billing/)

## 🐛 Troubleshooting

### Missing Usage Data
- Verify model supports usage reporting
- Check if response object has Usage property
- Ensure you're using latest SDK version

### Unexpected High Token Count
- Check conversation history length
- Review system instructions size
- Count tool definitions
- Use tokenizer to debug

### Rate Limit Errors
- Implement exponential backoff
- Reduce concurrent requests
- Request quota increase
- Switch to higher-tier model

## 🎤 Presentation Talking Points
1. **Introduction** - Why token tracking matters
2. **What Are Tokens?** - Explanation and examples
3. **Cost Structure** - Input vs output pricing
4. **Live Demo** - Show token usage in action
5. **Cost Calculation** - Real-world pricing examples
6. **Optimization Strategies** - 7 ways to reduce tokens
7. **Rate Limits** - Understanding quotas
8. **Monitoring** - Track usage over time
9. **Budget Management** - Alerts and controls
10. **Best Practices** - Production considerations
# Conversation Threads

## 📋 Overview
This project demonstrates how to manage conversation threads with AI agents. Threads allow agents to maintain context across multiple interactions, remember previous messages, and provide coherent, context-aware responses in multi-turn conversations.

## 🎯 Learning Objectives
- Create and manage conversation threads
- Maintain conversation history
- Handle multi-turn dialogues
- Understand thread lifecycle
- Implement conversation persistence

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
```bash
dotnet user-secrets set "AzureOpenAiEndpoint" "https://your-resource.openai.azure.com/"
dotnet user-secrets set "AzureOpenAiKey" "your-api-key"
dotnet user-secrets set "ChatDeploymentName" "gpt-4o"
```

### Step 2: Run the Application
```bash
dotnet run
```

## 💡 Code Explanation

### Creating an Agent
```csharp
ChatClientAgent agent = client
    .GetChatClient(configuration.ChatDeploymentName)
    .CreateAIAgent(
        name: "AssistantAgent",
        instructions: "You are a helpful assistant that remembers context"
    );
```

### Creating a New Thread
```csharp
AgentThread thread = agent.GetNewThread();
```
- Thread stores conversation history
- Each thread is independent
- Maintains context across messages

### Single Conversation with Thread
```csharp
AgentRunResponse response1 = await agent.RunAsync("Hi, my name is John", thread);
// Agent: "Hello John! Nice to meet you..."

AgentRunResponse response2 = await agent.RunAsync("What's my name?", thread);
// Agent: "Your name is John."  (remembers from thread)
```

### Without Thread (No Memory)
```csharp
AgentRunResponse response1 = await agent.RunAsync("Hi, my name is John");
// Agent: "Hello! Nice to meet you..."

AgentRunResponse response2 = await agent.RunAsync("What's my name?");
// Agent: "I don't know your name." (no context)
```

### Multiple Threads
```csharp
AgentThread thread1 = agent.GetNewThread();  // Conversation 1
AgentThread thread2 = agent.GetNewThread();  // Conversation 2

await agent.RunAsync("My favorite color is blue", thread1);
await agent.RunAsync("My favorite color is red", thread2);

// Each thread maintains separate context
```

## 🎓 Key Concepts

### What is a Thread?
- **Container** for conversation history
- **Isolated** from other threads
- **Persistent** across agent calls
- **Stateful** maintains context

### Thread vs No Thread

| With Thread | Without Thread |
|-------------|----------------|
| Remembers context | Each call is independent |
| Multi-turn conversation | Single-shot queries |
| Maintains history | No memory |
| Coherent dialogue | Isolated responses |
| Higher token usage | Lower token usage |

### Thread Lifecycle
```
Create Thread → Add Messages → Agent Processes → More Messages → End Thread
```

### When to Use Threads

✅ **Use Threads For:**
- Chat applications
- Customer support conversations
- Interactive assistants
- Multi-step tasks
- Personalized experiences
- Debugging conversations

❌ **Don't Use Threads For:**
- Independent queries
- Batch processing
- Stateless APIs
- One-off questions
- When context isn't needed

## 🛠️ Advanced Patterns

### Thread Per User Session
```csharp
public class ConversationService
{
    private readonly Dictionary<string, AgentThread> _userThreads = new();
    private readonly ChatClientAgent _agent;
    
    public AgentThread GetOrCreateThread(string userId)
    {
        if (!_userThreads.ContainsKey(userId))
        {
            _userThreads[userId] = _agent.GetNewThread();
        }
        return _userThreads[userId];
    }
    
    public async Task<string> SendMessageAsync(string userId, string message)
    {
        var thread = GetOrCreateThread(userId);
        var response = await _agent.RunAsync(message, thread);
        return response.Text;
    }
}
```

### Thread with Expiration
```csharp
public class ThreadManager
{
    private class ThreadInfo
    {
        public AgentThread Thread { get; set; }
        public DateTime LastUsed { get; set; }
    }
    
    private readonly Dictionary<string, ThreadInfo> _threads = new();
    
    public AgentThread GetThread(string userId)
    {
        CleanupExpiredThreads();
        
        if (_threads.TryGetValue(userId, out var info))
        {
            info.LastUsed = DateTime.UtcNow;
            return info.Thread;
        }
        
        var newThread = _agent.GetNewThread();
        _threads[userId] = new ThreadInfo 
        { 
            Thread = newThread, 
            LastUsed = DateTime.UtcNow 
        };
        return newThread;
    }
    
    private void CleanupExpiredThreads()
    {
        var expiredKeys = _threads
            .Where(kvp => DateTime.UtcNow - kvp.Value.LastUsed > TimeSpan.FromHours(1))
            .Select(kvp => kvp.Key)
            .ToList();
            
        foreach (var key in expiredKeys)
        {
            _threads.Remove(key);
        }
    }
}
```

### Persistent Threads (Database Storage)
```csharp
public class PersistentThreadService
{
    private readonly IDatabase _database;
    
    public async Task SaveThreadAsync(string userId, AgentThread thread)
    {
        var messages = thread.GetMessages(); // Hypothetical method
        await _database.SaveConversationAsync(userId, messages);
    }
    
    public async Task<AgentThread> LoadThreadAsync(string userId)
    {
        var messages = await _database.LoadConversationAsync(userId);
        var thread = _agent.GetNewThread();
        
        foreach (var message in messages)
        {
            thread.AddMessage(message); // Hypothetical method
        }
        
        return thread;
    }
}
```

### Thread with System Messages
```csharp
var thread = agent.GetNewThread();

// Add system context
var systemMessage = new ChatMessage(
    ChatRole.System, 
    "The user is a premium customer. Provide VIP support."
);

// Use in conversation
var messages = new List<ChatMessage> 
{ 
    systemMessage, 
    new ChatMessage(ChatRole.User, "I need help") 
};

var response = await agent.RunAsync(messages, thread);
```

## 📊 Example Conversation Flow

### With Thread:
```
User: "Hi, I'm planning a trip to Paris"
Agent: "How exciting! Paris is wonderful. How long will you be staying?"

User: "About a week"
Agent: "A week in Paris is perfect! When are you planning to go?"

User: "What should I pack?"
Agent: "For your week-long trip to Paris, I recommend..." 
(Agent remembers: Paris, week-long trip)
```

### Without Thread:
```
User: "Hi, I'm planning a trip to Paris"
Agent: "How exciting! How can I help with your Paris trip?"

User: "What should I pack?"
Agent: "Where are you traveling to and for how long?"
(Agent doesn't remember Paris or duration)
```

## 💰 Cost Considerations

### Token Usage with Threads
- **Each message**: Includes full conversation history
- **Growing cost**: More messages = more tokens per request
- **Example**:
  - First message: 50 tokens
  - Second message: 50 + previous = 100 tokens
  - Third message: 50 + previous = 150 tokens

### Optimization Strategies

**1. Truncate Old Messages:**
```csharp
var recentMessages = thread.GetMessages().TakeLast(10);
```

**2. Summarization:**
```csharp
if (thread.MessageCount > 20)
{
    var summary = await SummarizeThreadAsync(thread);
    thread = StartNewThreadWithSummary(summary);
}
```

**3. Token Limits:**
```csharp
var options = new ChatOptions
{
    MaxTokens = 4096  // Limit response size
};
```

**4. Thread Expiration:**
```csharp
// Auto-cleanup inactive threads
if (DateTime.UtcNow - thread.LastUsed > TimeSpan.FromHours(1))
{
    _threads.Remove(threadId);
}
```

## 🎯 Use Cases
- **Customer Support**: Track full support conversations
- **Chat Applications**: Messenger, Slack bots
- **Virtual Assistants**: Multi-turn task completion
- **Educational Apps**: Tutoring with context
- **Healthcare**: Patient consultation tracking
- **Sales**: Lead qualification conversations
- **Gaming**: NPC dialogue systems

## 🌟 Best Practices

### 1. Thread Isolation Per User
```csharp
// Good: Each user has own thread
Dictionary<string, AgentThread> userThreads;

// Bad: Shared thread across users
AgentThread sharedThread;
```

### 2. Clear Thread Purpose
```csharp
// Good: Specific purpose
var supportThread = agent.GetNewThread();
var salesThread = agent.GetNewThread();

// Track purpose
threads.Add("support-user123", supportThread);
```

### 3. Implement Timeouts
```csharp
var threadTimeout = TimeSpan.FromMinutes(30);
if (DateTime.UtcNow - thread.LastActivity > threadTimeout)
{
    // Start new thread or ask if user wants to continue
}
```

### 4. Monitor Thread Length
```csharp
if (thread.MessageCount > 50)
{
    // Warn user, summarize, or start fresh
    Console.WriteLine("Long conversation detected. Consider summarizing.");
}
```

### 5. Handle Thread Errors
```csharp
try
{
    await agent.RunAsync(message, thread);
}
catch (Exception)
{
    // Fallback: Create new thread
    thread = agent.GetNewThread();
}
```

## 🔗 Related Projects
- **DependencyInjection** - Thread management in web apps
- **Agent2Agent.Client/Server** - Distributed thread handling
- **Telemetry** - Monitor thread usage
- **ZeroToFirstAgent.AzureAiFoundry** - Persistent agent threads

## 📚 Additional Resources
- [Conversation Management](https://learn.microsoft.com/azure/ai-services/openai/how-to/chatgpt)
- [Managing Context](https://platform.openai.com/docs/guides/chat)
- [Token Optimization](https://help.openai.com/en/articles/4936856-what-are-tokens-and-how-to-count-them)

## 🐛 Troubleshooting

### Context Loss Issues
- Verify thread is being reused
- Check if thread is being recreated
- Ensure same thread object is used

### High Token Usage
- Implement message truncation
- Add summarization
- Set thread expiration
- Monitor conversation length

### Memory Leaks
- Clean up expired threads
- Implement max thread limit
- Use weak references for large apps
- Monitor memory usage

### Thread Confusion (Multi-User)
- Isolate threads per user/session
- Use unique thread identifiers
- Implement proper thread storage
- Add logging for debugging

## 🎤 Presentation Talking Points
1. **Introduction** - Why conversation memory matters
2. **The Problem** - Agents without context
3. **Threads Solution** - Maintaining conversation state
4. **Demo: Without Thread** - Show memory loss
5. **Demo: With Thread** - Show context retention
6. **Architecture** - How threads work internally
7. **Multiple Threads** - Isolation and use cases
8. **Token Economics** - Cost implications of threads
9. **Best Practices** - User isolation, timeouts, monitoring
10. **Advanced Patterns** - Persistence, expiration, summarization
11. **Real-world Use Cases** - Customer support, chat apps
12. **Optimization** - Managing costs and performance

