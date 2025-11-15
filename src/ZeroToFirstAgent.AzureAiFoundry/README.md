# Zero to First Agent - Azure AI Foundry

## 📋 Overview
This project demonstrates how to create and manage persistent AI agents using Azure AI Foundry (formerly Azure AI Studio). Unlike stateless agents, these agents are stored in Azure and maintain their configuration, allowing for more complex, long-running scenarios.

## 🎯 Learning Objectives
- Create persistent agents in Azure AI Foundry
- Manage agent lifecycle (create, use, delete)
- Work with conversation threads
- Understand agent state management
- Use Azure Identity for authentication

## 🎥 Video Tutorial
[Watch on YouTube](https://youtu.be/DoyeSZqim08)

## 📦 Prerequisites
- .NET 8.0 or higher
- Azure subscription
- Azure AI Foundry project (hub + project)
- Deployed model in Azure AI Foundry
- Azure CLI installed and authenticated

## 🔧 NuGet Packages Required
```xml
<PackageReference Include="Azure.AI.Agents.Persistent" />
<PackageReference Include="Azure.Identity" />
<PackageReference Include="Microsoft.Agents.AI.AzureAI" />
```

## 🚀 Quick Start

### Step 1: Create Azure AI Foundry Project
1. Navigate to [Azure AI Foundry](https://ai.azure.com)
2. Create a new Hub (if you don't have one)
3. Create a Project under the Hub
4. Deploy a model (e.g., gpt-4o, gpt-4o-mini)
5. Copy the project endpoint URL

### Step 2: Authenticate with Azure
```bash
az login
```
This allows the application to use your Azure credentials.

### Step 3: Configure the Application
Replace the placeholder values in `Program.cs`:
```csharp
const string endpoint = "<Azure AI Foundry project endpoint>";
const string model = "<your model>";  // Deployment name
```

### Step 4: Run the Application
```bash
dotnet run
```

## 💡 Code Explanation

### Creating the Persistent Client
```csharp
PersistentAgentsClient client = new(endpoint, new AzureCliCredential());
```
- Connects to Azure AI Foundry project
- Uses Azure CLI credentials (no API keys needed)
- Can also use `DefaultAzureCredential()` or `ManagedIdentityCredential()`

### Creating a Persistent Agent
```csharp
Response<PersistentAgent>? aiFoundryAgent = await client.Administration.CreateAgentAsync(
    model, 
    "MyFirstAgent", 
    "Some description", 
    "You are a nice AI"
);
```
- Creates agent stored in Azure
- Agent has an ID that persists
- Can be reused across sessions
- Instructions define agent personality

### Getting a Chat-Ready Agent
```csharp
ChatClientAgent agent = await client.GetAIAgentAsync(aiFoundryAgent.Value.Id);
```
- Converts persistent agent to chat client
- Allows standard agent operations

### Creating a Thread
```csharp
AgentThread thread = agent.GetNewThread();
```
- Thread maintains conversation state
- Multiple threads can exist per agent
- Each thread is independent

### Running the Agent
```csharp
AgentRunResponse response = await agent.RunAsync("What is the capital of France?", thread);
Console.WriteLine(response);
```

### Streaming Response
```csharp
await foreach (AgentRunResponseUpdate update in agent.RunStreamingAsync("How to make soup?", thread))
{
    Console.Write(update);
}
```

### Cleanup
```csharp
await client.Administration.DeleteAgentAsync(aiFoundryAgent.Value.Id);
```
- Important to delete test agents
- Prevents resource buildup
- In production, agents typically persist

## 🎓 Key Concepts

### Persistent vs Stateless Agents
| Persistent Agents | Stateless Agents |
|-------------------|------------------|
| Stored in Azure | Exist only in memory |
| Have unique IDs | Recreated each time |
| Configuration persists | Configuration in code |
| Can be shared across apps | Local to application |
| Support advanced features | Simpler to use |

### Agent Threads
- **Purpose**: Maintain conversation history
- **Isolation**: Each thread is independent
- **Use Cases**: Multiple users, different topics
- **Lifecycle**: Can be saved and resumed

### Azure AI Foundry Architecture
```
Hub (Shared Resources)
  └── Project 1
       ├── Models
       ├── Agents
       └── Data
  └── Project 2
       ├── Models
       └── Agents
```

## 🔒 Authentication Options

### Option 1: Azure CLI (Development)
```csharp
new AzureCliCredential()
```
✅ Easy for local development  
✅ No secrets in code  
❌ Requires `az login`  

### Option 2: Default Azure Credential (Recommended)
```csharp
new DefaultAzureCredential()
```
✅ Works everywhere (local, Azure)  
✅ Tries multiple credential types  
✅ Production-ready  

### Option 3: Managed Identity (Production)
```csharp
new ManagedIdentityCredential()
```
✅ Best for Azure-hosted apps  
✅ No credentials needed  
✅ Automatic credential rotation  

## 📊 Expected Output
```
The capital of France is Paris.
---
To make soup, start by...
```

## 🔐 Security Best Practices
- **Use Azure RBAC** for access control
- **Enable Private Endpoints** for network isolation
- **Use Managed Identities** in production
- **Implement least privilege** access
- **Enable audit logging** in Azure

## 🎯 Use Cases
- Multi-user chat applications
- Long-running agent workflows
- Agent sharing across applications
- Resume conversations after restart
- A/B testing different agent configurations
- Production-grade agent deployment

## 💡 Advanced Features
- **File Search Tool**: Agents can search uploaded files
- **Code Interpreter**: Execute Python code
- **Function Calling**: Custom tool integration
- **Vector Stores**: RAG capabilities built-in

## 💰 Cost Considerations
- Pay for agent execution time
- Storage costs for persistent agents (minimal)
- Separate charges for vector stores and files
- Monitor usage through Azure Cost Management

## 🔗 Related Projects
- **ZeroToFirstAgent.AzureOpenAi** - Simpler stateless version
- **AzureAiFoundry.Administration** - Agent management
- **AzureAiFoundry.FileSearchTool** - File-based RAG
- **AzureAiFoundry.CodeInterpreter** - Python execution
- **ConversationThreads** - Thread management patterns

## 📚 Additional Resources
- [Azure AI Foundry Documentation](https://learn.microsoft.com/azure/ai-studio/)
- [Persistent Agents SDK](https://learn.microsoft.com/dotnet/api/azure.ai.agents.persistent)
- [Azure Identity Documentation](https://learn.microsoft.com/dotnet/api/azure.identity)
- [AI Foundry Pricing](https://azure.microsoft.com/pricing/details/ai-studio/)

## 🐛 Troubleshooting

### "Unauthorized" Error
- Run `az login` and select correct subscription
- Verify you have "Cognitive Services User" role
- Check Azure RBAC permissions on the project

### "Project not found" Error
- Verify endpoint URL is correct
- Ensure project is created in Azure AI Foundry
- Check that you're using project endpoint, not hub endpoint

### Agent Creation Fails
- Verify model is deployed in the project
- Check model name matches deployment name
- Ensure sufficient quota available

### Thread Issues
- Threads are project-scoped
- Threads can be persisted and resumed
- Use thread IDs to continue conversations

## 🎤 Presentation Talking Points
1. **Introduction** - Why persistent agents matter
2. **Architecture** - Azure AI Foundry structure (Hub → Project → Agents)
3. **Agent Lifecycle** - Create, Use, Manage, Delete
4. **Thread Management** - Conversation state and isolation
5. **Authentication** - Azure Identity benefits
6. **Live Demo** - Create and interact with persistent agent
7. **Advanced Features** - File search, code interpreter preview
8. **Cleanup** - Importance of resource management
9. **Production Considerations** - RBAC, monitoring, costs
10. **Next Steps** - Explore advanced AI Foundry features

