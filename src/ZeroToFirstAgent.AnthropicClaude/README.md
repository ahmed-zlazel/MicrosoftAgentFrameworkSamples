# Zero to First Agent - Anthropic Claude

## 📋 Overview
This project demonstrates how to create AI agents using Anthropic's Claude models with the Microsoft Agent Framework. Claude is known for its strong reasoning capabilities, safety features, and long context understanding.

## 🎯 Learning Objectives
- Set up Anthropic Claude API access
- Integrate Claude with Microsoft Agent Framework
- Configure model-specific options
- Understand Claude's unique features and strengths

## 🎥 Video Tutorial
[Watch on YouTube](https://youtu.be/GbyEQWwBMFk)

## 📦 Prerequisites
- .NET 8.0 or higher
- Anthropic API account
- Anthropic API Key

## 🔧 NuGet Packages Required
```xml
<PackageReference Include="Anthropic.SDK" />
<PackageReference Include="Microsoft.Agents.AI" />
```

## 🚀 Quick Start

### Step 1: Get Anthropic API Key
1. Visit [Anthropic Console](https://console.anthropic.com/)
2. Create an account or sign in
3. Navigate to API Keys section
4. Generate a new API key
5. Copy the key for use

### Step 2: Configure the Application
Replace the placeholder values in `Program.cs`:
```csharp
const string apiKey = "<yourApiKey>";
const string model = AnthropicModels.Claude35Haiku;
```

### Step 3: Run the Application
```bash
dotnet run
```

## 💡 Code Explanation

### Creating the Anthropic Client
```csharp
AnthropicClient anthropicClient = new AnthropicClient(new APIAuthentication(apiKey));
```
- Creates authenticated client for Anthropic API
- Handles authentication and request signing

### Building the Chat Client
```csharp
IChatClient client = anthropicClient.Messages.AsBuilder().Build();
```
- Converts Anthropic client to IChatClient interface
- Compatible with Microsoft.Extensions.AI

### Configuring Run Options
```csharp
ChatClientAgentRunOptions chatClientAgentRunOptions = new(new()
{
    ModelId = model,
    MaxOutputTokens = 1000
});
```
- **Important**: Claude requires explicit model specification
- Controls output length with MaxOutputTokens
- Options required for both sync and streaming

### Creating the Agent
```csharp
ChatClientAgent agent = new(client);
```

### Running with Options
```csharp
AgentRunResponse response = await agent.RunAsync(
    "What is the Capital of Australia?", 
    options: chatClientAgentRunOptions
);
```

### Streaming with Options
```csharp
await foreach (AgentRunResponseUpdate update in agent.RunStreamingAsync(
    "How to make soup?", 
    options: chatClientAgentRunOptions))
{
    Console.Write(update);
}
```

## 🎓 Key Concepts

### Available Claude Models
The SDK provides constants for Claude models:
- `AnthropicModels.Claude35Sonnet` - Most capable, best reasoning
- `AnthropicModels.Claude35Haiku` - Fast and efficient
- `AnthropicModels.Claude3Opus` - Previous flagship model
- `AnthropicModels.Claude3Sonnet` - Balanced performance
- `AnthropicModels.Claude3Haiku` - Previous fast model

### Model Comparison
| Model | Speed | Intelligence | Context | Cost | Best For |
|-------|-------|--------------|---------|------|----------|
| Claude 3.5 Sonnet | Moderate | Highest | 200K | $$$ | Complex tasks |
| Claude 3.5 Haiku | Very Fast | High | 200K | $ | Production apps |
| Claude 3 Opus | Slow | Very High | 200K | $$$$ | Research |
| Claude 3 Sonnet | Fast | High | 200K | $$ | General use |

### Required vs Optional Configuration
**Required**:
- `ModelId` - Must specify which Claude model to use

**Optional but Recommended**:
- `MaxOutputTokens` - Controls response length (default: 4096)
- `Temperature` - Controls randomness (0.0 - 1.0)
- `TopP` - Alternative to temperature
- `StopSequences` - Custom stop conditions

## 🌟 Claude's Unique Strengths

### Constitutional AI
- Built-in safety and ethical guidelines
- Reduced harmful outputs
- More helpful and honest responses

### Extended Thinking
- Strong reasoning capabilities
- Good at breaking down complex problems
- Excellent for analytical tasks

### Context Understanding
- 200K token context window
- Excellent at using full context
- Great for long documents

### Coding Excellence
- Strong code understanding and generation
- Good at explaining code
- Helpful for debugging

## 📊 Expected Output
```
The capital of Australia is Canberra.
---
To make soup, start by gathering your ingredients...
```

## 🔒 Security Best Practices
- **Never commit API keys** to source control
- Use environment variables or secret managers
- Rotate API keys regularly
- Monitor usage through Anthropic Console
- Set up spending limits

## 💰 Cost Considerations
- Pricing varies by model (Opus > Sonnet > Haiku)
- Charged per input and output token
- Haiku is most cost-effective for production
- Monitor usage through Anthropic Console
- [Check current pricing](https://www.anthropic.com/pricing)

## 🎯 Use Cases
- Complex reasoning and analysis
- Code generation and review
- Long document understanding
- Research and writing assistance
- Safety-critical applications
- Customer service with ethical constraints

## 🆚 When to Choose Claude

Choose Claude when:
- Safety and ethics are paramount
- Complex reasoning is required
- Long document analysis is needed
- Code quality is critical
- You want reduced hallucinations

Choose alternatives when:
- You need faster responses (try Haiku first)
- You need Azure integration (use Azure OpenAI)
- You need Google ecosystem (use Gemini)
- You need function calling at scale (OpenAI more mature)

## 🔗 Related Projects
- **ZeroToFirstAgent.OpenAi** - OpenAI alternative
- **ZeroToFirstAgent.GoogleGemini** - Google alternative
- **ZeroToFirstAgent.AzureOpenAi** - Enterprise alternative
- **ToolCalling.Basics** - Adding function capabilities

## 📚 Additional Resources
- [Anthropic Documentation](https://docs.anthropic.com/)
- [Claude Model Specifications](https://docs.anthropic.com/claude/docs/models-overview)
- [API Reference](https://docs.anthropic.com/claude/reference/getting-started-with-the-api)
- [Pricing Information](https://www.anthropic.com/pricing)
- [Constitutional AI Paper](https://www.anthropic.com/constitutional-ai)

## 🐛 Troubleshooting

### "Model ID is required" Error
- Ensure you pass `ChatClientAgentRunOptions` with `ModelId`
- This is required for Claude, unlike some other providers

### "Invalid API Key" Error
- Verify API key is copied correctly (no spaces)
- Check if key has been revoked
- Ensure you have credits/billing set up

### "Rate limit exceeded" Error
- Check your tier limits in Anthropic Console
- Implement exponential backoff
- Consider upgrading your tier
- Space out requests more

### Token Limit Errors
- Reduce `MaxOutputTokens` setting
- Check input length (sum must be < 200K)
- Split large documents into chunks

### Slow Responses
- Try Claude 3.5 Haiku for faster results
- Use streaming for better UX
- Check network connectivity
- Consider geographic latency

## 🎤 Presentation Talking Points
1. **Introduction** - Anthropic's mission and Constitutional AI
2. **Claude Models** - Sonnet vs Haiku vs Opus comparison
3. **Key Strengths** - Safety, reasoning, code quality
4. **Setup Process** - API key and configuration
5. **Configuration Requirements** - Why options are needed
6. **Live Demo** - Show reasoning capabilities
7. **Safety Features** - Demonstrate ethical behavior
8. **Code Capabilities** - Show code generation quality
9. **Cost vs Performance** - When to use which model
10. **Best Practices** - Production considerations
11. **Comparison** - How Claude differs from competitors
12. **Future** - Anthropic's roadmap and vision
# Zero to First Agent - Google Gemini

## 📋 Overview
This project demonstrates how to create AI agents using Google's Gemini models with the Microsoft Agent Framework. Gemini offers competitive performance with unique features like long context windows.

## 🎯 Learning Objectives
- Set up Google Gemini API access
- Integrate Gemini models with Microsoft Agent Framework
- Use Gemini's advanced capabilities
- Compare Gemini with other AI providers

## 🎥 Video Tutorial
[Watch on YouTube](https://youtu.be/GbyEQWwBMFk)

## 📦 Prerequisites
- .NET 8.0 or higher
- Google Cloud account or Google AI Studio access
- Google Gemini API Key

## 🔧 NuGet Packages Required
```xml
<PackageReference Include="Google_GenerativeAI.Microsoft" />
<PackageReference Include="Microsoft.Agents.AI" />
```

## 🚀 Quick Start

### Step 1: Get Google Gemini API Key
1. Visit [Google AI Studio](https://aistudio.google.com/app/api-keys)
2. Create or sign in to your Google account
3. Click "Create API Key"
4. Copy the generated API key

### Step 2: Configure the Application
Replace the placeholder values in `Program.cs`:
```csharp
const string apiKey = "<yourApiKey>";
const string model = GoogleAIModels.Gemini25Pro;  // or other Gemini models
```

### Step 3: Run the Application
```bash
dotnet run
```

## 💡 Code Explanation

### Available Gemini Models
The SDK provides constants for available models:
- `GoogleAIModels.Gemini25Pro` - Most capable, latest version
- `GoogleAIModels.Gemini20FlashExp` - Fast, experimental features
- `GoogleAIModels.Gemini15Flash` - Balanced speed and quality
- `GoogleAIModels.Gemini15Pro` - Previous generation, reliable

### Creating the Gemini Client
```csharp
IChatClient client = new GenerativeAIChatClient(apiKey, model);
```
- Wraps Gemini API in IChatClient interface
- Compatible with Microsoft.Extensions.AI patterns

### Creating the Agent
```csharp
ChatClientAgent agent = new(client);
```
- Simple wrapper around the chat client
- Provides agent-specific functionality

### Running Queries
```csharp
AgentRunResponse response = await agent.RunAsync("What is the Capital of Australia?");
Console.WriteLine(response);
```

### Streaming Responses
```csharp
await foreach (AgentRunResponseUpdate update in agent.RunStreamingAsync("How to make soup?"))
{
    Console.Write(update);
}
```

## 🎓 Key Concepts

### Gemini Model Comparison
| Model | Speed | Quality | Context Window | Best For |
|-------|-------|---------|----------------|----------|
| Gemini 2.5 Pro | Moderate | Highest | 2M tokens | Complex reasoning |
| Gemini 2.0 Flash | Fast | High | 1M tokens | Real-time apps |
| Gemini 1.5 Flash | Very Fast | Good | 1M tokens | High-volume use |
| Gemini 1.5 Pro | Moderate | High | 2M tokens | Production stable |

### Unique Gemini Features
- **Massive Context**: Up to 2 million tokens
- **Multimodal**: Native image, video, audio support
- **Fast Updates**: Frequent model improvements
- **Code Execution**: Built-in code interpreter
- **Grounding**: Web search integration

## 📊 Expected Output
```
The capital of Australia is Canberra.
---
To make soup, you'll need to start by...
```

## 🔒 Security Best Practices
- **Never commit API keys** to source control
- Use environment variables for API keys
- Rotate keys periodically
- Monitor API usage through Google Cloud Console
- Set up usage quotas and alerts

## 🌟 Gemini Advantages
- ✅ Very long context windows (up to 2M tokens)
- ✅ Native multimodal capabilities
- ✅ Competitive pricing
- ✅ Fast inference speeds
- ✅ Regular model updates

## 💰 Cost Considerations
- Free tier available with rate limits
- Pay-as-you-go pricing for production
- Generally competitive with other providers
- Different pricing for Flash vs Pro models
- Check [Google AI Pricing](https://ai.google.dev/pricing)

## 🎯 Use Cases
- Long document analysis (novels, codebases)
- Multimodal applications (image + text)
- Real-time streaming applications
- Cost-sensitive deployments
- Applications requiring latest model features

## 🔗 Related Projects
- **ZeroToFirstAgent.OpenAi** - OpenAI alternative
- **ZeroToFirstAgent.AnthropicClaude** - Claude alternative
- **ImageGeneration.GoogleGemini** - Gemini image generation
- **ToolCalling.Basics** - Adding function calling

## 📚 Additional Resources
- [Google AI Studio](https://aistudio.google.com/)
- [Gemini API Documentation](https://ai.google.dev/docs)
- [Model Specifications](https://ai.google.dev/models/gemini)
- [Pricing Information](https://ai.google.dev/pricing)

## 🐛 Troubleshooting

### "Invalid API Key" Error
- Verify API key is copied correctly
- Check if key has been revoked
- Ensure billing is enabled (for paid usage)

### "Model not available" Error
- Verify model name constant
- Check regional availability
- Some models may be in limited preview

### Rate Limiting
- Free tier has strict rate limits
- Consider upgrading to paid tier
- Implement exponential backoff
- Check quota in Google Cloud Console

### Slow Response Times
- Try Gemini Flash models for faster responses
- Use streaming for better perceived performance
- Check network connectivity
- Consider geographic proximity to Google servers

## 🆚 When to Choose Gemini Over Others

Choose Gemini when:
- You need very long context windows (>128k tokens)
- Working with multimodal data (images, video)
- Want cutting-edge experimental features
- Cost optimization is important
- You prefer Google's ecosystem

Choose alternatives when:
- You need Azure integration (use Azure OpenAI)
- Enterprise compliance is critical (use Azure)
- You prefer OpenAI's ecosystem
- You need Anthropic's safety features (use Claude)

## 🎤 Presentation Talking Points
1. **Introduction** - Google's entry into AI agent market
2. **Key Differentiators** - Context length, multimodal, speed
3. **Model Options** - Pro vs Flash comparison
4. **API Setup** - Quick and easy getting started
5. **Live Demo** - Show both sync and streaming
6. **Context Window Demo** - Show long document handling
7. **Cost Comparison** - Compare with other providers
8. **Use Cases** - When Gemini is the best choice
9. **Future Outlook** - Google's AI roadmap
10. **Integration** - Works seamlessly with MS Agent Framework

