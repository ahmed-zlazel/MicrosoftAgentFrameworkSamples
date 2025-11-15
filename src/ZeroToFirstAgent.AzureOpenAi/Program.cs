//YouTube video that cover this sample: https://youtu.be/aQD4vhzQRvI

/* Steps:
 * 1: Create an 'Azure AI Foundry' Resource (or legacy 'Azure OpenAI Resource') + Deploy Model
 * 2: Add Nuget Packages (Azure.AI.OpenAI + Microsoft.Agents.AI.OpenAI)
 * 3: Create an AzureOpenAIClient (API Key or Azure Identity)
 * 4: Get a ChatClient and Create an AI Agent from it
 * 5: Call RunAsync or RunStreamingAsync
 */

using System.ClientModel;
using Azure.AI.OpenAI;
using Microsoft.Agents.AI;
using OpenAI;
using Shared;

var configuration = ConfigurationManager.GetConfiguration();
AzureOpenAIClient client = new(new Uri(configuration.AzureOpenAiEndpoint), new ApiKeyCredential(configuration.AzureOpenAiKey));
ChatClientAgent agent = client.GetChatClient(configuration.ChatDeploymentName).CreateAIAgent();

//Simple Response
AgentRunResponse response = await agent.RunAsync("What is the capital of France?");
Console.WriteLine(response);

Console.WriteLine("---");

//Streaming Result
await foreach (AgentRunResponseUpdate update in agent.RunStreamingAsync("How to make soup?"))
{
    Console.Write(update);
}