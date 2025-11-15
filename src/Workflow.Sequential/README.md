# Workflow - Sequential

## 📋 Overview
This project demonstrates how to create sequential workflows where multiple AI agents work together in a defined order. Each agent processes the output of the previous agent, enabling complex multi-step transformations and processing pipelines.

## 🎯 Learning Objectives
- Build sequential multi-agent workflows
- Chain agent outputs as inputs
- Use workflow orchestration
- Process data through multiple transformation steps
- Handle streaming in workflows

## 🎥 Video Tutorial
[Watch on YouTube](https://youtu.be/nPhpIciKfFs)

## 📦 Prerequisites
- .NET 8.0 or higher
- Azure OpenAI resource with deployed model
- Understanding of agent creation
- Familiarity with multi-agent concepts

## 🔧 NuGet Packages Required
```xml
<PackageReference Include="Azure.AI.OpenAI" />
<PackageReference Include="Microsoft.Agents.AI" />
<PackageReference Include="Microsoft.Agents.AI.Workflows" />
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

### Creating Specialized Agents
```csharp
ChatClientAgent summaryAgent = chatClient.CreateAIAgent(
    name: "SummaryAgent", 
    instructions: "Summarize the text you are given to max 20 words"
);

ChatClientAgent translationAgent = chatClient.CreateAIAgent(
    name: "TranslationAgent", 
    instructions: "Given a text Translate it to French"
);
```
- Each agent has a specific, focused task
- Clear instructions for consistent behavior
- Agents are reusable across workflows

### Building Sequential Workflow
```csharp
Workflow workflow = AgentWorkflowBuilder.BuildSequential(summaryAgent, translationAgent);
```
- Agents execute in order: Summary → Translation
- Output of first agent becomes input to second
- Automatic orchestration by framework

### Executing the Workflow
```csharp
var messages = new List<ChatMessage> { new(ChatRole.User, legalText) };

StreamingRun run = await InProcessExecution.StreamAsync(workflow, messages);
await run.TrySendMessageAsync(new TurnToken(emitEvents: true));
```

### Watching Workflow Events
```csharp
List<ChatMessage> result = [];
await foreach (WorkflowEvent evt in run.WatchStreamAsync().ConfigureAwait(false))
{
    if (evt is WorkflowOutputEvent completed)
    {
        result.AddRange(completed.History);
        Console.WriteLine($"Final Result: {completed.History.Last().Text}");
    }
    else if (evt is WorkflowStepEvent step)
    {
        Console.WriteLine($"Step {step.StepNumber}: {step.AgentName}");
    }
}
```

## 🎓 Key Concepts

### Sequential Workflow Pattern
```
Input Text
    ↓
Agent 1: Summarize (20 words max)
    ↓
"Brief summary of input..."
    ↓
Agent 2: Translate to French
    ↓
"Résumé bref de l'entrée..."
    ↓
Final Output
```

### Workflow Events
- **WorkflowStepEvent**: Agent started/completed
- **WorkflowOutputEvent**: Final result available
- **WorkflowErrorEvent**: Error occurred
- **WorkflowCancelledEvent**: Workflow cancelled

### When to Use Sequential Workflows
✅ **Good For:**
- Multi-step transformations
- Data processing pipelines
- Content creation workflows
- Quality improvement chains
- Step-by-step refinement

❌ **Not Ideal For:**
- Independent parallel tasks
- Real-time interactive chat
- Single-agent scenarios
- Tasks requiring backtracking

## 🛠️ Workflow Examples

### Content Creation Pipeline
```csharp
var researchAgent = CreateAgent("Research the topic");
var draftAgent = CreateAgent("Write first draft");
var editAgent = CreateAgent("Edit and improve");
var formatAgent = CreateAgent("Format for publication");

Workflow contentWorkflow = AgentWorkflowBuilder.BuildSequential(
    researchAgent, draftAgent, editAgent, formatAgent
);
```

### Data Processing Pipeline
```csharp
var extractAgent = CreateAgent("Extract key data");
var validateAgent = CreateAgent("Validate data quality");
var transformAgent = CreateAgent("Transform to target format");
var enrichAgent = CreateAgent("Enrich with additional data");

Workflow dataWorkflow = AgentWorkflowBuilder.BuildSequential(
    extractAgent, validateAgent, transformAgent, enrichAgent
);
```

### Customer Support Workflow
```csharp
var classifyAgent = CreateAgent("Classify the inquiry");
var researchAgent = CreateAgent("Find relevant information");
var draftAgent = CreateAgent("Draft response");
var reviewAgent = CreateAgent("Review for quality");

Workflow supportWorkflow = AgentWorkflowBuilder.BuildSequential(
    classifyAgent, researchAgent, draftAgent, reviewAgent
);
```

## 📊 Expected Output
```
Input: [Long legal text about pet duck regulations]

Step 1: SummaryAgent
Output: "Legal agreement for pet duck ownership covering care, regulations, and owner responsibilities."

Step 2: TranslationAgent
Output: "Accord légal pour la propriété de canards domestiques couvrant les soins, réglementations et responsabilités du propriétaire."

Final Result: "Accord légal pour la propriété de canards domestiques..."
```

## 🌟 Best Practices

### 1. Clear Agent Instructions
```csharp
// Good: Specific and measurable
instructions: "Summarize to exactly 20 words maximum"

// Bad: Vague
instructions: "Make it shorter"
```

### 2. Appropriate Agent Order
```csharp
// Good: Logical progression
Summarize → Translate → Format

// Bad: Illogical order
Translate → Summarize (loses translation quality)
```

### 3. Error Handling
```csharp
await foreach (WorkflowEvent evt in run.WatchStreamAsync())
{
    if (evt is WorkflowErrorEvent error)
    {
        Console.WriteLine($"Error in {error.AgentName}: {error.Error}");
        // Handle error, retry, or abort
    }
}
```

### 4. Monitoring Progress
```csharp
if (evt is WorkflowStepEvent step)
{
    Console.WriteLine($"Processing step {step.StepNumber}/{totalSteps}");
    UpdateProgressBar(step.StepNumber, totalSteps);
}
```

### 5. Intermediate Results
```csharp
// Save intermediate outputs for debugging/analysis
if (evt is WorkflowStepCompletedEvent completed)
{
    await SaveIntermediateResult(completed.AgentName, completed.Output);
}
```

## 🎯 Advanced Patterns

### Conditional Steps
```csharp
// Skip translation if already in target language
if (DetectLanguage(input) != "French")
{
    workflow = BuildSequential(summaryAgent, translationAgent);
}
else
{
    workflow = BuildSequential(summaryAgent);
}
```

### Dynamic Workflow Building
```csharp
var agents = new List<ChatClientAgent>();

if (needsSummary) agents.Add(summaryAgent);
if (needsTranslation) agents.Add(translationAgent);
if (needsFormatting) agents.Add(formatAgent);

Workflow workflow = AgentWorkflowBuilder.BuildSequential(agents.ToArray());
```

### Workflow with Validation
```csharp
var processAgent = CreateAgent("Process the data");
var validateAgent = CreateAgent("Validate result quality");
var fixAgent = CreateAgent("Fix any issues");

Workflow workflow = BuildSequential(processAgent, validateAgent, fixAgent);
```

## 💰 Cost Considerations
- Each agent in sequence = separate API call
- Total cost = sum of all agent costs
- Longer workflows = higher costs
- Consider caching intermediate results
- Monitor token usage per step

### Cost Optimization
```csharp
// Use smaller models for simple steps
var summaryAgent = smallModel.CreateAIAgent(...);  // gpt-4o-mini
var complexAgent = largeModel.CreateAIAgent(...);  // gpt-4o
```

## 🔗 Related Projects
- **Workflow.Concurrent** - Parallel agent execution
- **Workflow.Handoff** - Agent handoff patterns
- **Workflow.AiAssisted.PizzaSample** - Complex workflow example
- **MultiAgent.AgentAsTool** - Alternative coordination approach
- **Agent2Agent.Client/Server** - Distributed workflows

## 📚 Additional Resources
- [Workflow Documentation](https://learn.microsoft.com/dotnet/ai/agents/workflows)
- [Agent Orchestration Patterns](https://learn.microsoft.com/azure/architecture/ai-ml/guide/agent-orchestration)
- [Multi-Agent Systems](https://en.wikipedia.org/wiki/Multi-agent_system)

## 🐛 Troubleshooting

### Workflow Hangs
- Check for infinite loops
- Verify all agents respond
- Add timeout configuration
- Monitor WorkflowEvents for stalls

### Agent Order Issues
- Review logical flow
- Test agents individually first
- Verify output format compatibility
- Check if agents understand previous output

### Quality Degradation
- Information loss through chain
- Solution: Add validation agents
- Consider fewer steps
- Use higher quality models

### Performance Problems
- Too many sequential steps
- Solution: Parallelize independent steps
- Use faster models where appropriate
- Cache intermediate results

## 🎤 Presentation Talking Points
1. **Introduction** - Multi-step agent processing
2. **Use Case** - Legal text: Summarize → Translate
3. **Sequential Pattern** - Output becomes input
4. **Agent Creation** - Specialized, focused agents
5. **Workflow Building** - BuildSequential API
6. **Execution** - Streaming workflow runs
7. **Event Monitoring** - Track progress and results
8. **Live Demo** - Watch transformation steps
9. **Real-world Examples** - Content, data, support pipelines
10. **Best Practices** - Clear instructions, error handling
11. **Cost Optimization** - Model selection per step
12. **Next Steps** - Concurrent workflows, handoffs
# Using RAG in Agent Framework

## 📋 Overview
This project demonstrates how to implement Retrieval-Augmented Generation (RAG) with AI agents. RAG allows agents to access and retrieve relevant information from large datasets efficiently, avoiding the need to load all data into context and significantly reducing costs.

## 🎯 Learning Objectives
- Understand RAG (Retrieval-Augmented Generation) concepts
- Implement vector search for semantic retrieval
- Compare "preload all data" vs RAG approaches
- Use embeddings for efficient data retrieval
- Optimize token usage and costs with RAG

## 🎥 Video Tutorial
[Watch on YouTube](https://youtu.be/Vpi5aZJRJmA)

## 📦 Prerequisites
- .NET 8.0 or higher
- Azure OpenAI resource with chat and embedding models
- Understanding of basic agent creation
- Familiarity with vector concepts (helpful but not required)

## 🔧 NuGet Packages Required
```xml
<PackageReference Include="Azure.AI.OpenAI" />
<PackageReference Include="Microsoft.Agents.AI" />
<PackageReference Include="Microsoft.Extensions.AI" />
<PackageReference Include="Microsoft.Extensions.VectorData" />
<PackageReference Include="Microsoft.SemanticKernel.Connectors.InMemory" />
```

## 🚀 Quick Start

### Step 1: Configure User Secrets
```bash
dotnet user-secrets set "AzureOpenAiEndpoint" "https://your-resource.openai.azure.com/"
dotnet user-secrets set "AzureOpenAiKey" "your-api-key"
dotnet user-secrets set "ChatDeploymentName" "gpt-4o"
dotnet user-secrets set "EmbeddingModelName" "text-embedding-ada-002"
```

### Step 2: Run the Application
```bash
dotnet run
```

## 💡 Code Explanation

### Sample Data Setup
```csharp
string jsonWithMovies = await File.ReadAllTextAsync("made_up_movies.json");
Movie[] movieDataForRag = JsonSerializer.Deserialize<Movie[]>(jsonWithMovies)!;
```
- Loads dataset (100+ movies in this example)
- In production, could be from database, API, etc.

### Approach 1: Preload All Data (Expensive!)
```csharp
List<ChatMessage> preloadEverythingChatMessages = [
    new(ChatRole.Assistant, "Here are all the movies")
];

foreach (Movie movie in movieDataForRag)
{
    preloadEverythingChatMessages.Add(
        new ChatMessage(ChatRole.Assistant, movie.GetTitleAndDetails())
    );
}

preloadEverythingChatMessages.Add(question);
AgentRunResponse response1 = await agent.RunAsync(preloadEverythingChatMessages);
```

**Problems:**
- ❌ Uses massive amounts of tokens
- ❌ Expensive (all data in context)
- ❌ Slower (large context to process)
- ❌ May exceed token limits

### Approach 2: RAG (Efficient!)
```csharp
// 1. Create embedding generator
IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator = client
    .GetEmbeddingClient(configuration.EmbeddingModelName)
    .AsBuilder()
    .Build();

// 2. Create vector store
InMemoryVectorStore vectorStore = new();
IVectorStoreRecordCollection<string, MovieVector> collection = 
    vectorStore.GetCollection<string, MovieVector>("movies");
await collection.CreateCollectionIfNotExistsAsync();

// 3. Generate and store embeddings
foreach (Movie movie in movieDataForRag)
{
    GeneratedEmbeddings<Embedding<float>> embeddings = 
        await embeddingGenerator.GenerateAsync(movie.GetTitleAndDetails());
    
    await collection.UpsertAsync(new MovieVector
    {
        Key = movie.Id.ToString(),
        TitleAndDetails = movie.GetTitleAndDetails(),
        TitleAndDetailsEmbedding = embeddings[0].Vector
    });
}

// 4. Search for relevant data
GeneratedEmbeddings<Embedding<float>> questionEmbedding = 
    await embeddingGenerator.GenerateAsync(question.Text);

var searchResults = await collection.VectorizedSearchAsync(
    questionEmbedding[0].Vector, 
    new() { Top = 3 }  // Only retrieve top 3 relevant items
);

// 5. Add only relevant data to context
List<ChatMessage> ragMessages = [new(ChatRole.Assistant, "Here are the relevant movies")];
await foreach (VectorSearchResult<MovieVector> result in searchResults.Results)
{
    ragMessages.Add(new ChatMessage(ChatRole.Assistant, result.Record.TitleAndDetails));
}
ragMessages.Add(question);

// 6. Run agent with minimal context
AgentRunResponse response2 = await agent.RunAsync(ragMessages);
```

## 🎓 Key Concepts

### What is RAG?
**Retrieval-Augmented Generation** combines:
1. **Retrieval**: Find relevant information from large datasets
2. **Augmentation**: Add that information to context
3. **Generation**: AI generates answer using retrieved data

### How RAG Works
```
User Question
    ↓
Convert to Embedding (Vector)
    ↓
Search Vector Store
    ↓
Retrieve Top K Similar Items
    ↓
Add to Agent Context
    ↓
Agent Generates Answer
```

### Embeddings Explained
- **Embedding**: Numerical representation of text (e.g., 1536 dimensions)
- **Semantic Similarity**: Similar meanings = similar vectors
- **Vector Search**: Find items with similar embeddings
- **Example**: 
  - "puppy" and "dog" have similar embeddings
  - "puppy" and "computer" have different embeddings

### Vector Store Options
**In-Memory** (This Sample):
- Fast for development
- Lost on restart
- Limited by RAM

**Production Options:**
- Azure AI Search
- PostgreSQL with pgvector
- Pinecone
- Qdrant
- Redis with vector support

## 📊 Cost Comparison

### Preload All Data
- **Input Tokens**: 50,000+ (all movies)
- **Cost per query**: $$$
- **Speed**: Slow (large context)

### RAG Approach
- **Input Tokens**: 500-1,000 (top 3 movies)
- **Cost per query**: $
- **Speed**: Fast (small context)
- **Savings**: 95%+ token reduction

## 🎯 Use Cases
- **Knowledge Bases**: Company documentation, wikis
- **Product Catalogs**: Search across thousands of products
- **Customer Support**: Access to support articles
- **Legal/Medical**: Search through regulations, research papers
- **Code Search**: Find relevant code snippets
- **Long Documents**: Books, manuals, reports

## 🌟 Best Practices

### 1. Chunking Strategy
```csharp
// Split large documents into chunks
public static List<string> ChunkDocument(string document, int maxChunkSize = 500)
{
    // Split by paragraphs, sentences, or fixed size
    // Overlap chunks slightly for context continuity
}
```

### 2. Embedding Model Selection
- **text-embedding-ada-002**: Most common, balanced
- **text-embedding-3-small**: Cheaper, smaller
- **text-embedding-3-large**: Higher quality, more expensive

### 3. Optimal Top K
```csharp
new VectorSearchOptions { Top = 3 }  // Start here
```
- Too few (1-2): May miss relevant data
- Good range (3-5): Balance relevance and cost
- Too many (10+): Defeats RAG purpose

### 4. Metadata Filtering
```csharp
var searchResults = await collection.VectorizedSearchAsync(
    embedding.Vector,
    new VectorSearchOptions 
    { 
        Top = 5,
        Filter = new VectorSearchFilter()
            .EqualTo("Category", "Adventure")
            .GreaterThan("Rating", 8.0)
    }
);
```

### 5. Reranking Results
```csharp
// After vector search, rerank by:
// - Recency (newer items first)
// - User preferences
// - Business rules
// - Cross-encoder models
```

## 🛠️ Advanced Techniques

### Hybrid Search
```csharp
// Combine vector search + keyword search
var vectorResults = await VectorSearch(query);
var keywordResults = await FullTextSearch(query);
var combined = Merge(vectorResults, keywordResults);
```

### Query Expansion
```csharp
// Expand user query before searching
original: "best action films"
expanded: "best action films movies high-rated blockbuster"
```

### Contextual Embeddings
```csharp
// Add metadata to embedding text
string textToEmbed = $"Category: {movie.Category}\n" +
                    $"Rating: {movie.Rating}\n" +
                    $"Title: {movie.Title}\n" +
                    $"Plot: {movie.Plot}";
```

## 💰 Cost Optimization

### Embedding Costs
- One-time cost per document
- Cache embeddings (don't regenerate)
- Batch embedding generation
- Use cheaper embedding models for less critical use cases

### Storage Costs
- In-memory: Free but ephemeral
- Managed vector stores: Pay for storage
- Self-hosted: Infrastructure costs

### Query Costs
- Embedding generation per query: ~$0.0001
- Chat completion: Reduced by 90%+ with RAG
- Total: Massive savings on high-volume systems

## 🔗 Related Projects
- **AdvancedRAGTechniques** - Advanced RAG patterns
- **AzureAiFoundry.FileSearchTool** - Built-in RAG in Azure AI Foundry
- **OpenAIResponsesApi.FileSearchTool** - OpenAI's RAG implementation
- **ToolCalling.Basics** - RAG as a tool

## 📚 Additional Resources
- [RAG Overview](https://learn.microsoft.com/azure/search/retrieval-augmented-generation-overview)
- [Vector Search Concepts](https://learn.microsoft.com/azure/search/vector-search-overview)
- [Embeddings Guide](https://platform.openai.com/docs/guides/embeddings)
- [Azure AI Search](https://azure.microsoft.com/products/ai-services/ai-search)

## 🐛 Troubleshooting

### Poor Search Results
- Improve embedding quality (better source text)
- Increase Top K parameter
- Try different embedding models
- Add metadata filtering
- Implement reranking

### High Latency
- Cache embeddings
- Use faster vector store
- Reduce Top K
- Implement parallel search
- Use smaller embedding models

### Out of Memory
- Use persistent vector store (not in-memory)
- Reduce embedding dimensions
- Implement pagination
- Clear old data

### Relevance Issues
- Check embedding model quality
- Review chunking strategy
- Add contextual information
- Implement hybrid search
- Use query expansion

## 🎤 Presentation Talking Points
1. **The Problem** - Can't fit all data in context
2. **Cost Analysis** - Show token usage comparison
3. **RAG Introduction** - Retrieval + Augmentation + Generation
4. **Embeddings Explained** - Vectors and semantic similarity
5. **Demo: Without RAG** - Show cost and token usage
6. **Demo: With RAG** - Show efficiency and savings
7. **Architecture** - How RAG system works
8. **Vector Stores** - Options and trade-offs
9. **Best Practices** - Chunking, Top K, metadata
10. **Advanced Techniques** - Hybrid search, reranking
11. **Cost Savings** - Real numbers and ROI
12. **Production Considerations** - Scaling and monitoring
13. **Next Steps** - Advanced RAG techniques

