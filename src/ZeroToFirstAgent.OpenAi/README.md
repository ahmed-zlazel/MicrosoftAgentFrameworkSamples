# Zero to First Agent - OpenAI

## 📋 Overview
This project demonstrates how to create your first AI agent using OpenAI's API with the Microsoft Agent Framework. It's the simplest starting point for building AI-powered applications.

## 🎯 Learning Objectives
- Understand the basics of AI agent creation
- Learn how to authenticate with OpenAI API
- Execute synchronous and streaming chat completions
- Get familiar with Microsoft.Agents.AI framework

## 🎥 Video Tutorial
[Watch on YouTube](https://youtu.be/CvA69UyqJ7U)

## 📦 Prerequisites
- .NET 8.0 or higher
- OpenAI API Account
- OpenAI API Key

## 🔧 NuGet Packages Required
```xml
<PackageReference Include="OpenAI" />
<PackageReference Include="Microsoft.Agents.AI.OpenAI" />
```

## 🚀 Quick Start

### Step 1: Get OpenAI API Key
1. Create an account at [OpenAI Platform](https://platform.openai.com/)
2. Navigate to API Keys section
3. Generate a new API key
4. Copy the key for use in your application

### Step 2: Configure the Application
Replace the placeholder values in `Program.cs`:
```csharp
const string apiKey = "<YourAPIKey>";        // Your OpenAI API Key
const string model = "<yourModelName>";      // Example: "gpt-4o" or "gpt-4o-mini"
```

### Step 3: Run the Application
```bash
dotnet run
```

## 💡 Code Explanation

### Creating the Client
```csharp
OpenAIClient client = new(apiKey);
```
- Creates an authenticated client for OpenAI API

### Creating the Agent
```csharp
ChatClientAgent agent = client.GetChatClient(model).CreateAIAgent();
```
- Gets a chat client for the specified model
- Creates an AI agent wrapper for easier interaction

### Synchronous Response
```csharp
AgentRunResponse response = await agent.RunAsync("What is the Capital of Germany?");
Console.WriteLine(response);
```
- Sends a question and waits for complete response
- Best for simple, one-off queries

### Streaming Response
```csharp
await foreach (AgentRunResponseUpdate update in agent.RunStreamingAsync("How to make soup?"))
{
    Console.Write(update);
}
```
- Receives response in real-time chunks
- Better user experience for long responses
- Lower perceived latency

## 🎓 Key Concepts

### What is a ChatClientAgent?
A `ChatClientAgent` is a wrapper around an `IChatClient` that provides:
- Simplified API for common agent operations
- Support for threads and conversation management
- Built-in streaming capabilities
- Tool calling support

### RunAsync vs RunStreamingAsync
| RunAsync | RunStreamingAsync |
|----------|-------------------|
| Returns complete response | Returns response in chunks |
| Simple to use | Better UX for long responses |
| Higher perceived latency | Lower perceived latency |
| Single await | Async enumeration |

## 📊 Expected Output
```
The capital of Germany is Berlin.
---
To make soup, start by...
```

## 🔒 Security Best Practices
- **Never commit API keys to source control**
- Use environment variables or Azure Key Vault for production
- Rotate API keys regularly
- Monitor API usage and set spending limits

## 🎯 Use Cases
- Simple chatbots
- Q&A systems
- Text generation
- Content summarization
- Code explanation

## 🔗 Related Projects
- **ZeroToFirstAgent.AzureOpenAi** - Using Azure-hosted OpenAI models
- **ZeroToFirstAgent.AnthropicClaude** - Alternative AI provider
- **ZeroToFirstAgent.GoogleGemini** - Google's AI models
- **ToolCalling.Basics** - Add function calling capabilities

## 📚 Additional Resources
- [OpenAI API Documentation](https://platform.openai.com/docs)
- [Microsoft.Extensions.AI](https://learn.microsoft.com/dotnet/ai/)
- [Agent Framework Documentation](https://github.com/microsoft/agents)

## 💰 Cost Considerations
- OpenAI charges per token (input + output)
- GPT-4 models are more expensive than GPT-3.5
- Monitor usage through OpenAI dashboard
- Consider using cheaper models for development/testing

## 🐛 Troubleshooting

### "Unauthorized" Error
- Verify your API key is correct
- Check if API key has been revoked
- Ensure billing is set up on your OpenAI account

### "Model not found" Error
- Verify the model name is correct
- Check if you have access to the model
- Some models require waitlist approval

### Slow Response Times
- Try using streaming for better perceived performance
- Consider using faster models (e.g., gpt-3.5-turbo)
- Check your internet connection

## 🎤 Presentation Talking Points
1. **Introduction** - Simplest way to start with AI agents
2. **Authentication** - How OpenAI API authentication works
3. **Two Response Modes** - Sync vs Streaming comparison
4. **Live Demo** - Show both response types
5. **Best Practices** - Security and cost optimization
6. **Next Steps** - Where to go from here (tool calling, Azure, etc.)

