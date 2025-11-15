# Zero to First Agent - Ollama (Local AI)

## 📋 Overview
This project demonstrates how to create AI agents using Ollama for running AI models locally on your machine. Ollama makes it incredibly easy to run open-source LLMs without cloud dependencies, API keys, or internet connectivity.

## 🎯 Learning Objectives
- Run AI models locally on your machine
- Work offline without API keys
- Understand local vs cloud AI trade-offs
- Use Ollama for privacy-sensitive applications
- Experiment with open-source models

## 🎥 Video Tutorial
[Watch on YouTube](https://youtu.be/GbyEQWwBMFk)

## 📦 Prerequisites
- .NET 8.0 or higher
- Ollama installed on your machine
- At least one model downloaded in Ollama
- Sufficient RAM (8GB minimum, 16GB+ recommended)
- GPU optional but recommended for better performance

## 🔧 NuGet Packages Required
```xml
<PackageReference Include="OllamaSharp" />
<PackageReference Include="Microsoft.Agents.AI" />
```

## 🚀 Quick Start

### Step 1: Install Ollama
1. Visit [Ollama.com](https://ollama.com/)
2. Download installer for your OS (Windows, macOS, Linux)
3. Run the installer
4. Verify installation: `ollama --version`

### Step 2: Download a Model
```bash
ollama pull llama3.2:1b        # Small, fast model (1.3GB)
ollama pull llama3.2           # Default 3B model (2GB)
ollama pull llama3.1:8b        # Larger, more capable (4.7GB)
ollama pull phi3:mini          # Microsoft's small model (2.3GB)
ollama pull deepseek-coder     # Code-specialized model
```

### Step 3: Verify Ollama is Running
```bash
ollama list                    # Show downloaded models
ollama serve                   # Start Ollama server (usually auto-starts)
```

### Step 4: Configure the Application
Update `Program.cs` with your preferred model:
```csharp
IChatClient client = new OllamaApiClient("http://localhost:11434", "llama3.2:1b");
```

### Step 5: Run the Application
```bash
dotnet run
```

## 💡 Code Explanation

### Creating the Ollama Client
```csharp
IChatClient client = new OllamaApiClient("http://localhost:11434", "llama3.2:1b");
```
- **Endpoint**: Default Ollama runs on `localhost:11434`
- **Model**: Must match a model you've downloaded
- **No API Key**: Everything runs locally

### Creating the Agent
```csharp
ChatClientAgent agent = new(client);
```

### Running Queries
```csharp
AgentRunResponse response = await agent.RunAsync("What is the Capital of Sweden?");
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

### Popular Ollama Models

**Small Models (1-3B parameters):**
- `llama3.2:1b` - Very fast, basic tasks (1.3GB)
- `phi3:mini` - Microsoft's efficient model (2.3GB)
- `qwen2.5:1.5b` - Multilingual, fast (934MB)

**Medium Models (7-8B parameters):**
- `llama3.1:8b` - Great balance (4.7GB)
- `mistral:7b` - Strong performance (4.1GB)
- `gemma2:9b` - Google's model (5.4GB)

**Large Models (70B+ parameters):**
- `llama3.1:70b` - Very capable (40GB)
- `qwen2.5:72b` - High quality (41GB)

**Specialized Models:**
- `deepseek-coder:6.7b` - Code generation
- `codellama:13b` - Meta's code model
- `llava` - Vision + language
- `mistral-openorca` - Reasoning focused

### Model Size vs Performance
| Size | Speed | Quality | RAM Required | Best For |
|------|-------|---------|--------------|----------|
| 1-3B | Very Fast | Basic | 4-8GB | Testing, simple tasks |
| 7-8B | Fast | Good | 8-16GB | Development, general use |
| 13-15B | Moderate | Very Good | 16-32GB | Production, complex tasks |
| 70B+ | Slow | Excellent | 64GB+ | High-quality needs |

## 🌟 Ollama Advantages
- ✅ **100% Local**: No internet required after download
- ✅ **Privacy**: Data never leaves your machine
- ✅ **No API Keys**: No authentication needed
- ✅ **No Costs**: Free to use, no token charges
- ✅ **Full Control**: Complete ownership of models
- ✅ **Fast Iteration**: No network latency for local requests
- ✅ **Open Source**: Transparent and customizable

## 🚫 Ollama Limitations
- ❌ **Hardware Requirements**: Needs good CPU/RAM (GPU helps)
- ❌ **Model Quality**: Open models lag behind GPT-4, Claude
- ❌ **Setup Complexity**: More steps than cloud APIs
- ❌ **Storage**: Models are large (1-40GB each)
- ❌ **Maintenance**: You manage updates and versions

## 📊 Expected Output
```
The capital of Sweden is Stockholm.
---
To make soup, you can follow these steps...
```

## 🔒 Security & Privacy Benefits
- **Data Privacy**: Sensitive data never leaves your network
- **Compliance**: Easier to meet data residency requirements
- **No Logging**: Your queries aren't stored by third parties
- **Offline Operation**: Works in air-gapped environments
- **Perfect For**:
  - Healthcare applications (HIPAA)
  - Financial services
  - Government projects
  - Internal tools
  - Confidential research

## ⚙️ Performance Optimization

### Hardware Recommendations
**Minimum:**
- 8GB RAM
- 4-core CPU
- 10GB free disk space

**Recommended:**
- 16GB+ RAM
- 8-core CPU
- NVIDIA GPU with 8GB+ VRAM
- SSD storage

**Optimal:**
- 32GB+ RAM
- High-end CPU
- NVIDIA GPU with 24GB+ VRAM
- NVMe SSD

### Model Selection Tips
```bash
# For development/testing (fast)
ollama pull llama3.2:1b

# For production (balanced)
ollama pull llama3.1:8b

# For quality (if you have resources)
ollama pull llama3.1:70b
```

### GPU Acceleration
Ollama automatically uses GPU if available:
- NVIDIA GPUs: CUDA support built-in
- Apple Silicon: Metal acceleration
- AMD GPUs: ROCm support (Linux)

## 💰 Cost Considerations
- **Free**: No API costs, ever
- **Hardware**: One-time investment in good hardware
- **Electricity**: Local computation uses power
- **ROI**: Pays off quickly for high-volume use
- **Scaling**: Harder to scale than cloud (need more hardware)

## 🎯 Use Cases
- **Perfect For:**
  - Privacy-sensitive applications
  - Offline/air-gapped environments
  - High-volume development/testing
  - Learning and experimentation
  - Internal tools and demos
  - Cost-sensitive projects

- **Not Ideal For:**
  - Highest quality requirements (use GPT-4)
  - Serverless/elastic scaling
  - Low-resource environments
  - Mobile applications
  - When you lack good hardware

## 🔗 Related Projects
- **ZeroToFirstAgent.OpenAi** - Cloud alternative
- **ZeroToFirstAgent.AzureOpenAi** - Enterprise cloud
- **ToolCalling.Basics** - Works with Ollama too
- **UsingRAGInAgentFramework** - Local RAG possible

## 📚 Additional Resources
- [Ollama Official Site](https://ollama.com/)
- [Ollama GitHub](https://github.com/ollama/ollama)
- [Model Library](https://ollama.com/library)
- [OllamaSharp Documentation](https://github.com/awaescher/OllamaSharp)
- [Model Comparison](https://ollama.com/library)

## 🐛 Troubleshooting

### "Connection refused" Error
```bash
# Check if Ollama is running
ollama list

# Start Ollama manually if needed
ollama serve
```

### "Model not found" Error
```bash
# List downloaded models
ollama list

# Pull the model you need
ollama pull llama3.2:1b
```

### Slow Response Times
- Use smaller model (e.g., `llama3.2:1b`)
- Ensure GPU acceleration is working
- Close other applications
- Check if model fits in RAM
- Consider quantized versions

### Out of Memory Errors
- Use smaller model
- Close other applications
- Increase system swap/page file
- Upgrade RAM
- Use quantized models (Q4, Q5)

### Poor Quality Responses
- Try larger model
- Adjust temperature settings
- Use specialized model for your task
- Consider cloud API for critical tasks

## 🎤 Presentation Talking Points
1. **Introduction** - Run AI models on your own hardware
2. **Why Local AI?** - Privacy, cost, control benefits
3. **Ollama Demo** - Installation and model download
4. **Model Options** - Small vs large comparison
5. **Live Demo** - Show local inference in action
6. **Performance** - Speed comparison with/without GPU
7. **Privacy Use Case** - Sensitive data handling
8. **Cost Analysis** - When local saves money
9. **Quality Trade-offs** - Honest comparison with GPT-4
10. **Hardware Requirements** - What you need to get started
11. **Production Considerations** - Scaling and maintenance
12. **Future** - Open source AI progress and trajectory
# Zero to First Agent - GitHub Models

## 📋 Overview
This project demonstrates how to create AI agents using GitHub Models with the Microsoft Agent Framework. GitHub Models provides free access to various AI models directly through GitHub, making it perfect for developers already in the GitHub ecosystem.

## 🎯 Learning Objectives
- Access AI models through GitHub
- Use GitHub PAT tokens for authentication
- Explore multiple model providers via one platform
- Leverage free tier for development and testing

## 📦 Prerequisites
- .NET 8.0 or higher
- GitHub account
- GitHub Personal Access Token (PAT) with Models access

## 🔧 NuGet Packages Required
```xml
<PackageReference Include="Azure.AI.Inference" />
<PackageReference Include="Microsoft.Extensions.AI.AzureAIInference" />
<PackageReference Include="Microsoft.Agents.AI" />
```

## 🚀 Quick Start

### Step 1: Get GitHub Models Access
1. Visit [GitHub Marketplace - Models](https://github.com/marketplace?type=models)
2. Choose a model (e.g., DeepSeek, GPT-4o, Llama, Phi)
3. Click on the model to see setup instructions
4. Accept the terms and get access

### Step 2: Create GitHub PAT Token
1. Go to GitHub Settings → Developer settings → Personal access tokens
2. Generate new token (classic or fine-grained)
3. Enable "GitHub Models" scope
4. Copy the token immediately (won't be shown again)

### Step 3: Configure the Application
Replace the placeholder values in `Program.cs`:
```csharp
const string gitHubPatToken = "<YourGitHubPATTokenWithModelAccess>";
const string model = "<yourModelName>";  // Example: deepseek/DeepSeek-V3-0324
```

### Step 4: Run the Application
```bash
dotnet run
```

## 💡 Code Explanation

### Creating the Client
```csharp
ChatClientAgent agent = new ChatCompletionsClient(
    new Uri("https://models.github.ai/inference"),
    new AzureKeyCredential(gitHubPatToken),
    new AzureAIInferenceClientOptions())
    .AsIChatClient(model)
    .CreateAIAgent();
```
- Uses Azure AI Inference SDK (GitHub Models uses compatible API)
- Endpoint is always `https://models.github.ai/inference`
- PAT token acts as the credential
- Model name includes provider prefix (e.g., `deepseek/`, `openai/`)

### Running Queries
```csharp
AgentRunResponse response = await agent.RunAsync("What is the Capital of Denmark?");
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

### Available Models on GitHub
GitHub Models provides access to various providers:

**OpenAI Models:**
- `openai/gpt-4o`
- `openai/gpt-4o-mini`
- `openai/o1-preview`
- `openai/o1-mini`

**Meta Models:**
- `meta/llama-3.3-70b-instruct`
- `meta/llama-3.1-405b-instruct`
- `meta/llama-3.1-8b-instruct`

**Microsoft Models:**
- `microsoft/phi-4`
- `microsoft/phi-3.5-mini-instruct`

**DeepSeek Models:**
- `deepseek/deepseek-v3-0324`
- `deepseek/deepseek-coder-v2`

**Other Providers:**
- Mistral AI models
- Cohere models
- AI21 Labs models

### Model Naming Convention
```
provider/model-name-version
```
Examples:
- `openai/gpt-4o` → OpenAI's GPT-4o
- `meta/llama-3.3-70b-instruct` → Meta's Llama 3.3
- `deepseek/deepseek-v3-0324` → DeepSeek V3

## 🌟 GitHub Models Advantages
- ✅ **Free Tier**: Rate-limited free access to premium models
- ✅ **Multiple Providers**: Try different models without multiple accounts
- ✅ **Developer Friendly**: Integrated with GitHub workflow
- ✅ **No Separate Billing**: Use existing GitHub account
- ✅ **Easy Testing**: Perfect for prototyping and learning
- ✅ **Quick Setup**: Minutes to get started

## 📊 Expected Output
```
The capital of Denmark is Copenhagen.
---
To make soup, begin by selecting your ingredients...
```

## 🔒 Security Best Practices
- **Never commit PAT tokens** to source control
- Use environment variables or GitHub Secrets
- Set minimal scopes on PAT token
- Rotate tokens periodically
- Revoke tokens when not needed
- Use fine-grained tokens when possible

## ⚡ Rate Limits

### Free Tier Limits
- **Requests per minute**: Varies by model (typically 10-60)
- **Tokens per minute**: Varies by model (typically 50K-150K)
- **Concurrent requests**: Limited
- **Perfect for**: Development, testing, learning

### Production Use
- GitHub Models is primarily for development/testing
- For production, consider:
  - Azure OpenAI
  - Direct provider APIs
  - Azure AI Foundry

## 💰 Cost Considerations
- **Free Tier**: Available for all models
- **No Credit Card Required**: Use with GitHub account
- **Rate Limited**: Not suitable for high-volume production
- **Great for**:
  - Learning and exploration
  - Prototyping
  - Small-scale projects
  - Model comparison

## 🎯 Use Cases
- **Perfect For:**
  - Learning AI development
  - Prototyping applications
  - Comparing different models
  - Small-scale personal projects
  - Demo applications

- **Not Ideal For:**
  - High-volume production apps
  - Low-latency requirements
  - Enterprise SLA requirements
  - Applications requiring guaranteed uptime

## 🆚 Model Comparison on GitHub

| Provider | Strengths | Best For |
|----------|-----------|----------|
| OpenAI GPT-4o | Versatile, high quality | General purpose |
| Meta Llama | Open source, customizable | Fine-tuning |
| DeepSeek | Cost-effective, fast | High throughput |
| Microsoft Phi | Small, efficient | Edge deployment |
| Mistral | Multilingual | International apps |

## 🔗 Related Projects
- **ZeroToFirstAgent.OpenAi** - Direct OpenAI API
- **ZeroToFirstAgent.AzureOpenAi** - Production alternative
- **ZeroToFirstAgent.GoogleGemini** - Alternative platform
- **ToolCalling.Basics** - Adding capabilities

## 📚 Additional Resources
- [GitHub Models Marketplace](https://github.com/marketplace?type=models)
- [GitHub Models Documentation](https://docs.github.com/en/github-models)
- [Azure AI Inference SDK](https://learn.microsoft.com/azure/ai-studio/how-to/develop/sdk-overview)
- [Model Comparison Guide](https://github.com/marketplace?type=models)

## 🐛 Troubleshooting

### "Authentication failed" Error
- Verify PAT token is correct
- Ensure "GitHub Models" scope is enabled
- Check if token has expired
- Regenerate token if needed

### "Model not found" Error
- Verify model name includes provider prefix
- Check model availability in your region
- Ensure you've accepted model terms
- Try a different model

### Rate Limit Errors
- Implement exponential backoff
- Reduce request frequency
- Use smaller models for testing
- Consider upgrading to paid tier (when available)
- For production, migrate to direct provider

### Slow Response Times
- GitHub Models is shared infrastructure
- Response times vary by load
- Not guaranteed for production
- Consider direct provider APIs for SLA

### "Region not supported" Error
- Some models have geographic restrictions
- Try different models
- Use VPN for testing (not for production)
- Check model availability in your region

## 🎤 Presentation Talking Points
1. **Introduction** - GitHub's AI offering for developers
2. **Why GitHub Models?** - Free access, multiple providers, easy setup
3. **Available Models** - Overview of providers and models
4. **Setup Process** - PAT token creation and configuration
5. **Live Demo** - Quick model switching demonstration
6. **Model Comparison** - Compare GPT-4o vs Llama vs DeepSeek live
7. **Rate Limits** - Understanding free tier constraints
8. **Use Cases** - When to use GitHub Models
9. **Production Path** - Migration strategy to production APIs
10. **Best Practices** - Security and token management
11. **Ecosystem Integration** - GitHub Copilot, Actions, etc.

