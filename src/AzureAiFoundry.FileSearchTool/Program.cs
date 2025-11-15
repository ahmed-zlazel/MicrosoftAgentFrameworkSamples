//YouTube video that cover this sample: https://youtu.be/OeAqD7lCSGg

using Azure;
using Azure.AI.Agents.Persistent;
using Azure.Identity;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Shared;

Console.Clear();

Configuration configuration = ConfigurationManager.GetConfiguration();

var credentialOptions = new InteractiveBrowserCredentialOptions
{
    TenantId = "bf388af6-2845-418a-ab23-708197d06f00", // Your tenant ID
    RedirectUri = new Uri("http://localhost")          // Standard redirect URI for interactive auth
};

PersistentAgentsClient client = new(configuration.AzureAiFoundryAgentEndpoint, new InteractiveBrowserCredential(credentialOptions));

Response<PersistentAgent>? aiFoundryAgent = null;
ChatClientAgentThread? chatClientAgentThread = null;
string? vectorStoreId = null;
try
{
    string filename = "secretData.pdf";
    Response<PersistentAgentFileInfo> file = await client.Files.UploadFileAsync(Path.Combine("Data", filename), PersistentAgentFilePurpose.Agents);

    Response<PersistentAgentsVectorStore> vectorStore = await client.VectorStores.CreateVectorStoreAsync(name: "MyVectorStore");
    vectorStoreId = vectorStore.Value.Id;
    await client.VectorStores.CreateVectorStoreFileAsync(vectorStore.Value.Id, file.Value.Id);

    aiFoundryAgent = await client.Administration.CreateAgentAsync(
        configuration.ChatDeploymentName,
        "FileAgent",
        "",
        "You are a File-expert. ALWAYS use tools to answer all questions (do not use you world-knowledge)",
        toolResources: new ToolResources
        {
            FileSearch = new FileSearchToolResource
            {
                VectorStoreIds = { vectorStore.Value.Id },
            }
        },
        tools: new List<ToolDefinition>
        {
            new FileSearchToolDefinition
            {
                FileSearch = new FileSearchToolDefinitionDetails
                {
                    MaxNumResults = 10
                }
            }
        });

    AIAgent agent = (await client.GetAIAgentAsync(aiFoundryAgent.Value.Id));

    AgentThread thread = agent.GetNewThread();
    while (true)
    {

        Console.Write("> ");
        string? input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
        {
            input = "What is word of the day?, and what the secret in the file " + filename;
        }
        AgentRunResponse response = await agent.RunAsync( input, thread);
        Console.WriteLine(response);
        foreach (ChatMessage message in response.Messages)
        {
            foreach (AIContent content in message.Contents)
            {
                foreach (AIAnnotation annotation in content.Annotations ?? [])
                {
                    if (annotation is CitationAnnotation citationAnnotation)
                    {
                        string? fileId = citationAnnotation.FileId;
                        Response<PersistentAgentFileInfo> fileReferenced = await client.Files.GetFileAsync(fileId);
                        Utils.WriteLineYellow("File referenced: " + fileReferenced.Value.Filename);
                    }
                }
            }
        }
    }
}
finally
{
    if (vectorStoreId != null)
    {
        await client.VectorStores.DeleteVectorStoreAsync(vectorStoreId);
    }

    if (chatClientAgentThread != null)
    {
        await client.Threads.DeleteThreadAsync(chatClientAgentThread.ConversationId);
    }

    if (aiFoundryAgent != null)
    {
        await client.Administration.DeleteAgentAsync(aiFoundryAgent.Value.Id);
    }
}