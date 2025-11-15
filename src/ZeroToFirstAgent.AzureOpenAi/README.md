# Zero to First Agent - Azure OpenAI

## 📋 Overview
This project demonstrates how to create your first AI agent using Azure OpenAI Service with the Microsoft Agent Framework. Azure OpenAI provides enterprise-grade security, compliance, and regional availability.

## 🎯 Learning Objectives
- Set up Azure OpenAI Service
- Authenticate using API Key or Azure Identity
- Create and run AI agents on Azure infrastructure
- Understand the differences between OpenAI and Azure OpenAI

## 🎥 Video Tutorial
[Watch on YouTube](https://youtu.be/aQD4vhzQRvI)

## 📦 Prerequisites
- .NET 8.0 or higher
- Azure subscription
- Azure OpenAI resource or Azure AI Foundry resource
- Deployed model in Azure

## 🔧 NuGet Packages Required
```xml
<PackageReference Include="Azure.AI.OpenAI" />
<PackageReference Include="Microsoft.Agents.AI.OpenAI" />
```

## 🚀 Quick Start

### Step 1: Create Azure OpenAI Resource
1. Log into [Azure Portal](https://portal.azure.com)
2. Create "Azure OpenAI" resource or "Azure AI Foundry" resource
3. Note the endpoint URL
4. Copy one of the API keys from "Keys and Endpoint" section

### Step 2: Deploy a Model
1. Navigate to your Azure OpenAI resource
2. Go to "Model deployments" or use Azure AI Foundry Studio
3. Deploy a model (e.g., gpt-4o, gpt-4o-mini, gpt-35-turbo)
4. Note the deployment name (you choose this)

### Step 3: Configure the Application
Replace the placeholder values in `Program.cs`:
```csharp
const string endpoint = "https://<yourEndpoint>.openai.azure.com/";
const string apiKey = "<yourApiKey>";
const string model = "<yourModelDeploymentName>";
```

### Step 4: Run the Application
```bash
dotnet run
```

## 💡 Code Explanation

### Creating the Azure Client
```csharp
AzureOpenAIClient client = new(new Uri(endpoint), new ApiKeyCredential(apiKey));
```
- Uses Azure-specific client for regional deployment
- Supports API Key authentication (shown) or Azure Identity

### Alternative: Using Azure Identity
```csharp
AzureOpenAIClient client = new(new Uri(endpoint), new DefaultAzureCredential());
```
- More secure for production environments
- Uses managed identities or Azure CLI credentials
- No API keys to manage

### Creating the Agent
```csharp
ChatClientAgent agent = client.GetChatClient(model).CreateAIAgent();
```
- Gets chat client for your deployed model
- Model name is YOUR deployment name, not the base model name

### Synchronous Response
```csharp
AgentRunResponse response = await agent.RunAsync("What is the capital of France?");
Console.WriteLine(response);
```

### Streaming Response
```csharp
await foreach (AgentRunResponseUpdate update in agent.RunStreamingAsync("How to make soup?"))
{
    Console.Write(update);
}
```

## 🎓 Key Concepts

### Azure OpenAI vs Standard OpenAI
| Azure OpenAI | Standard OpenAI |
|--------------|-----------------|
| Enterprise SLA | Public API |
| Regional data residency | Global |
| Private networking (VNet) | Public internet only |
| Compliance certifications | Limited compliance |
| Custom deployment names | Fixed model names |
| Azure security integration | API key only |

### Deployment Names vs Model Names
- **Base Model**: gpt-4o, gpt-35-turbo (what Microsoft provides)
- **Deployment Name**: my-gpt4-deployment (what YOU name it)
- You reference YOUR deployment name in code

## 🔒 Authentication Options

### Option 1: API Key (Simplest)
```csharp
new ApiKeyCredential(apiKey)
```
✅ Easy to get started  
❌ Key management required  

### Option 2: Azure Identity (Recommended)
```csharp
new DefaultAzureCredential()
```
✅ No secrets in code  
✅ Automatic credential rotation  
✅ Works with managed identities  

## 📊 Expected Output
```
The capital of France is Paris.
---
To make soup, start by choosing your base...
```

## 🔐 Security Best Practices
- **Use Azure Key Vault** for storing API keys
- **Enable Private Endpoints** for network isolation
- **Use Managed Identities** in production
- **Enable diagnostic logging** for audit trails
- **Set up Azure Policy** for compliance

## 🌍 Regional Considerations
- Choose regions close to your users
- Check model availability per region
- Consider data residency requirements
- Some models available only in specific regions

## 💰 Cost Considerations
- Pay-as-you-go or provisioned throughput
- Pricing varies by region and model
- Monitor usage through Azure Cost Management
- Set up spending limits and alerts

## 🎯 Use Cases
- Enterprise chatbots with compliance needs
- Applications requiring data residency
- Integration with Azure services (App Service, Functions)
- Scenarios requiring private networking

## 🔗 Related Projects
- **ZeroToFirstAgent.OpenAi** - Public OpenAI API version
- **ZeroToFirstAgent.AzureAiFoundry** - Using persistent agents
- **ToolCalling.Basics** - Adding function capabilities
- **Telemetry** - Monitoring with Application Insights

## 📚 Additional Resources
- [Azure OpenAI Service Documentation](https://learn.microsoft.com/azure/ai-services/openai/)
- [Azure AI Foundry Documentation](https://learn.microsoft.com/azure/ai-studio/)
- [Pricing Calculator](https://azure.microsoft.com/pricing/calculator/)
- [Regional Availability](https://learn.microsoft.com/azure/ai-services/openai/concepts/models#model-summary-table-and-region-availability)

## 🐛 Troubleshooting

### "Resource not found" Error
- Verify endpoint URL is correct
- Ensure resource is created and active
- Check subscription status

### "Deployment not found" Error
- Verify deployment name (not base model name)
- Ensure model is fully deployed
- Check deployment status in Azure Portal

### Authentication Failures
- Verify API key is correct and not regenerated
- Check Azure RBAC permissions for managed identity
- Ensure network access rules allow your IP

### Rate Limiting
- Check your quota in Azure Portal
- Consider requesting quota increase
- Implement retry logic with exponential backoff

## 🎤 Presentation Talking Points
1. **Why Azure OpenAI?** - Enterprise benefits over public API
2. **Resource Setup** - Step-by-step Azure configuration
3. **Security Features** - Private endpoints, managed identities
4. **Authentication Methods** - API Key vs Azure Identity
5. **Live Demo** - Show both response modes
6. **Cost Management** - Monitoring and optimization
7. **Next Steps** - Advanced features and integrations

