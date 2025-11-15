# Microsoft Agent Framework - Comprehensive Samples Repository

## 📋 Overview
This repository contains comprehensive samples demonstrating the Microsoft Agent Framework (formerly Semantic Kernel Agents). Learn how to build AI-powered applications with various providers, implement advanced patterns, and deploy production-ready solutions.

## 🎯 Repository Structure

### 🚀 Getting Started - Zero to First Agent
Learn the basics by creating your first agent with different AI providers:

| Project | Provider | Video | Difficulty |
|---------|----------|-------|------------|
| **ZeroToFirstAgent.OpenAi** | OpenAI API | [▶️](https://youtu.be/CvA69UyqJ7U) | ⭐ Beginner |
| **ZeroToFirstAgent.AzureOpenAi** | Azure OpenAI | [▶️](https://youtu.be/aQD4vhzQRvI) | ⭐ Beginner |
| **ZeroToFirstAgent.AzureAiFoundry** | Azure AI Foundry | [▶️](https://youtu.be/DoyeSZqim08) | ⭐⭐ Intermediate |
| **ZeroToFirstAgent.GoogleGemini** | Google Gemini | [▶️](https://youtu.be/GbyEQWwBMFk) | ⭐ Beginner |
| **ZeroToFirstAgent.AnthropicClaude** | Anthropic Claude | [▶️](https://youtu.be/GbyEQWwBMFk) | ⭐ Beginner |
| **ZeroToFirstAgent.GitHubModels** | GitHub Models | - | ⭐ Beginner |
| **ZeroToFirstAgent.Ollama** | Local AI (Ollama) | [▶️](https://youtu.be/GbyEQWwBMFk) | ⭐ Beginner |
| **ZeroToFirstAgent.HuggingFace** | HuggingFace | - | ⭐⭐ Intermediate |
| **ZeroToFirstAgent.OpenRouter** | OpenRouter | - | ⭐ Beginner |
| **ZeroToFirstAgent.XAIGrok** | X.AI Grok | - | ⭐ Beginner |

### 🛠️ Core Features
Master essential agent capabilities:

| Project | Feature | Video | Key Concepts |
|---------|---------|-------|--------------|
| **ToolCalling.Basics** | Function Calling | [▶️](https://youtu.be/gJTodKpv8Ik) | External tools, APIs |
| **ToolCalling.Advanced** | Advanced Tools | [▶️](https://youtu.be/dCtojrK8bKk) | Complex scenarios |
| **Toolcalling.FromAnMcpServer** | MCP Integration | [▶️](https://youtu.be/Y5IKdt9vdJM) | Model Context Protocol |
| **StructuredOutput** | Typed Responses | [▶️](https://youtu.be/BNB7zO3Uqwc) | JSON schemas, types |
| **ConversationThreads** | Context Management | - | Memory, history |
| **TokenUsage** | Cost Tracking | - | Monitoring, optimization |
| **Telemetry** | Observability | [▶️](https://youtu.be/jeVQo75KcCw) | OpenTelemetry, insights |
| **DependencyInjection** | ASP.NET Core | [▶️](https://youtu.be/q-mHdd6iJJo) | Web integration |

### 👥 Multi-Agent Systems
Build collaborative agent architectures:

| Project | Pattern | Video | Description |
|---------|---------|-------|-------------|
| **MultiAgent.AgentAsTool** | Delegation | [▶️](https://youtu.be/wL4V78s_wI4) | Agents calling agents |
| **MultiAgent.ManualViaStructuredOutput** | Coordination | - | Manual orchestration |
| **Agent2Agent.Client** | Distributed | - | Client-side agents |
| **Agent2Agent.Server** | Distributed | - | Server-side agents |
| **AgentUserInteraction.Client** | Interactive | - | User interaction patterns |
| **AgentUserInteraction.Server** | Interactive | - | Server interaction |

### 🔄 Workflow Patterns
Orchestrate complex agent workflows:

| Project | Type | Video | Use Case |
|---------|------|-------|----------|
| **Workflow.Sequential** | Linear | [▶️](https://youtu.be/nPhpIciKfFs) | Step-by-step processing |
| **Workflow.Concurrent** | Parallel | - | Independent tasks |
| **Workflow.Handoff** | Transfer | - | Agent hand-offs |
| **Workflow.AiAssisted.PizzaSample** | Complex | - | Real-world example |

### 🗄️ RAG (Retrieval-Augmented Generation)
Implement knowledge retrieval and semantic search:

| Project | Focus | Video | Technology |
|---------|-------|-------|------------|
| **UsingRAGInAgentFramework** | Basic RAG | [▶️](https://youtu.be/Vpi5aZJRJmA) | Vector search |
| **AdvancedRAGTechniques** | Advanced | - | Chunking, reranking |

### 🎨 Image Generation
Create images with AI:

| Project | Provider | Video | Models |
|---------|----------|-------|--------|
| **ImageGeneration** | Azure/OpenAI | [▶️](https://youtu.be/F8BxvnpWJ9s) | DALL-E 2/3 |
| **ImageGeneration.GoogleGemini** | Google | - | Imagen |
| **ImageGeneration.XAiGrok** | X.AI | - | Grok Image |

### ☁️ Azure AI Foundry Features
Leverage Azure AI Foundry capabilities:

| Project | Feature | Description |
|---------|---------|-------------|
| **AzureAiFoundry.Administration** | Management | Agent lifecycle |
| **AzureAiFoundry.CodeInterpreter** | Python Execution | Run code in sandbox |
| **AzureAiFoundry.FileSearchTool** | File RAG | Search uploaded files |
| **AzureAiFoundry.WebSearch** | Web Search | Real-time information |
| **AzureAiFoundry.Models** | Model Info | Available models |

### 🔓 OpenAI Responses API
Explore OpenAI's enhanced APIs:

| Project | Feature | Description |
|---------|---------|-------------|
| **OpenAIResponsesApi.CodeInterpreter** | Code Execution | Python interpreter |
| **OpenAIResponsesApi.FileSearchTool** | File Search | Document analysis |
| **OpenAIResponsesApi.WebSearch** | Web Search | Bing integration |
| **OpenAIResponsesApi.ReasoningSummary** | Reasoning | Chain-of-thought |
| **OpenAIResponsesApi.ResumeConversation** | Resume | Continue conversations |
| **OpenAIResponsesApi.SpecialModels** | Special Models | o1, o3-mini |

### 🧰 Agent Framework Toolkits
Ready-to-use tool collections:

| Project | Tools | Description |
|---------|-------|-------------|
| **AgentFramework.Toolkit** | Core Tools | Common utilities |
| **AgentFramework.Toolkit.AzureOpenAI** | Azure Tools | Azure-specific |
| **AgentFramework.Toolkit.Google** | Google Tools | Google-specific |
| **Toolkit.Comparison** | Comparison | Tool comparison |

### 🎓 Advanced Topics
Explore specialized scenarios:

| Project | Topic | Description |
|---------|-------|-------------|
| **TheDifferentAgents** | Agent Types | Compare agent types |
| **SettingsOnAiAgent** | Configuration | Agent settings |
| **WatchRawRequestAndResponse** | Debugging | Inspect requests |
| **LifeOfAnLLMCall** | Internals | Call lifecycle |
| **ReasoningEffort** | Reasoning | Control reasoning |
| **ChatHistoryReducer** | Optimization | Reduce context |
| **AllowBackgroundResponses** | Async | Background processing |
| **AgentInputData** | Data Handling | Input processing |

### 🏢 Real-World Examples
Production-ready sample applications:

| Project | Type | Description |
|---------|------|-------------|
| **E2E.ComicBookStoreSample** | E-Commerce | Full application |
| **Trello.Agent** | Integration | Trello API agent |
| **AspireBlazorDemo** | .NET Aspire | Cloud-native app |

### 🧪 Development Tools
Utilities for development:

| Project | Purpose |
|---------|---------|
| **DevUI** | Development UI |
| **Playground** | Experimentation |
| **Samples.AppHost** | Aspire host |
| **Samples.ServiceDefaults** | Defaults |

## 📚 Learning Path

### 🎯 Beginner (Start Here)
1. **ZeroToFirstAgent.OpenAi** or **ZeroToFirstAgent.AzureOpenAi** - Your first agent
2. **ToolCalling.Basics** - Add capabilities
3. **StructuredOutput** - Typed responses
4. **ConversationThreads** - Add memory
5. **TokenUsage** - Understand costs

### 🎯 Intermediate
1. **MultiAgent.AgentAsTool** - Multi-agent systems
2. **Workflow.Sequential** - Orchestration
3. **UsingRAGInAgentFramework** - Knowledge retrieval
4. **Telemetry** - Monitoring
5. **DependencyInjection** - Web integration

### 🎯 Advanced
1. **AdvancedRAGTechniques** - Advanced retrieval
2. **AzureAiFoundry.CodeInterpreter** - Code execution
3. **Workflow.AiAssisted.PizzaSample** - Complex workflows
4. **E2E.ComicBookStoreSample** - Full application
5. **Agent2Agent.Client/Server** - Distributed systems

## 🔧 Prerequisites

### Required
- .NET 8.0 SDK or higher
- Visual Studio 2022, VS Code, or JetBrains Rider
- Git

### Cloud Services (Choose One or More)
- **Azure OpenAI** or **Azure AI Foundry** subscription
- **OpenAI** API account
- **Google** AI Studio account
- **Anthropic** API account
- **GitHub** account (for GitHub Models)

### Optional
- **Ollama** (for local AI)
- **Docker** (for some samples)
- **Azure subscription** (for Azure services)

## ⚙️ Configuration

### User Secrets Setup
All samples use .NET User Secrets for secure configuration:

```bash
# Navigate to project folder
cd src/YourProject

# Set secrets
dotnet user-secrets set "AzureOpenAiEndpoint" "https://your-resource.openai.azure.com/"
dotnet user-secrets set "AzureOpenAiKey" "your-api-key"
dotnet user-secrets set "ChatDeploymentName" "gpt-4o"
dotnet user-secrets set "OpenAiApiKey" "your-openai-key"
dotnet user-secrets set "GoogleGeminiApiKey" "your-google-key"
# ... etc
```

### Full Configuration Template
See `src/Shared/ConfigurationManager.cs` for all available settings:
- OpenAI API Key
- Azure OpenAI Endpoint & Key
- Chat Deployment Name
- Embedding Model Name
- Azure AI Foundry Endpoint & Agent ID
- Bing API Key
- GitHub PAT Token
- HuggingFace API Key
- OpenRouter API Key
- Application Insights Connection String
- Google Gemini API Key
- X.AI Grok API Key
- Trello API Key & Token

## 🚀 Quick Start

### 1. Clone Repository
```bash
git clone <repository-url>
cd <repository-name>
```

### 2. Open Solution
```bash
# Open main solution
dotnet sln Samples.slnx
```

### 3. Configure Secrets
Choose a sample project and configure secrets (see above)

### 4. Run Sample
```bash
cd src/ZeroToFirstAgent.OpenAi
dotnet run
```

## 🎥 Video Series
Many samples have accompanying video tutorials on YouTube. Look for the 🎥 links in the project tables above.

## 💰 Cost Considerations
- **OpenAI/Azure OpenAI**: Pay per token
- **Google Gemini**: Free tier available
- **GitHub Models**: Free with rate limits
- **Ollama**: Free (local compute)
- **Anthropic**: Pay per token

Monitor usage with the **TokenUsage** sample project.

## 🔒 Security Best Practices
- ✅ Use User Secrets for local development
- ✅ Use Azure Key Vault for production
- ✅ Never commit API keys to source control
- ✅ Implement rate limiting
- ✅ Monitor usage and costs
- ✅ Use Managed Identities when possible

## 📖 Documentation Resources
- [Microsoft Agent Framework](https://learn.microsoft.com/dotnet/ai/agents/)
- [Azure OpenAI](https://learn.microsoft.com/azure/ai-services/openai/)
- [Azure AI Foundry](https://learn.microsoft.com/azure/ai-studio/)
- [OpenAI API](https://platform.openai.com/docs)
- [Google Gemini](https://ai.google.dev/docs)
- [Anthropic Claude](https://docs.anthropic.com/)

## 🤝 Contributing
Contributions are welcome! Please:
1. Fork the repository
2. Create a feature branch
3. Add your sample with comprehensive README
4. Submit a pull request

## 📄 License
See LICENSE file for details.

## 🆘 Support
- Open an issue for bugs or questions
- Check existing README files in each project
- Review video tutorials
- Consult official documentation

## 🗺️ Roadmap
- [ ] More provider integrations
- [ ] Additional workflow patterns
- [ ] Performance optimization samples
- [ ] Security best practices guide
- [ ] Production deployment templates
- [ ] Cost optimization strategies

## 🏆 Sample Highlights

### Best for Learning
- **ZeroToFirstAgent.OpenAi** - Simplest starting point
- **ToolCalling.Basics** - Essential feature
- **MultiAgent.AgentAsTool** - Powerful pattern

### Best for Production
- **DependencyInjection** - Web app integration
- **Telemetry** - Monitoring and observability
- **UsingRAGInAgentFramework** - Cost-effective knowledge

### Most Impressive
- **E2E.ComicBookStoreSample** - Complete application
- **Workflow.AiAssisted.PizzaSample** - Complex orchestration
- **AzureAiFoundry.CodeInterpreter** - Code execution

### Most Cost-Effective
- **ZeroToFirstAgent.Ollama** - Free local AI
- **ZeroToFirstAgent.GitHubModels** - Free tier access
- **UsingRAGInAgentFramework** - Token optimization

---

**Happy Building! 🚀🤖**

*For detailed information about each project, navigate to the project folder and read its README.md file.*

