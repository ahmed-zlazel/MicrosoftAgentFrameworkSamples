# Structured Output

## 📋 Overview
This project demonstrates how to get structured, typed responses from AI agents instead of plain text. Structured output ensures the AI returns data in a specific format (JSON schema), making it reliable for integration with other systems and eliminating parsing errors.

## 🎯 Learning Objectives
- Force AI to return specific data structures
- Define JSON schemas for responses
- Work with strongly-typed agent responses
- Compare structured vs unstructured output
- Handle complex nested data types

## 🎥 Video Tutorial
[Watch on YouTube](https://youtu.be/BNB7zO3Uqwc)

## 📦 Prerequisites
- .NET 8.0 or higher
- Azure OpenAI resource with deployed model
- Understanding of C# types and JSON serialization

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

### Defining Data Models
```csharp
public class MovieResult
{
    public List<Movie> Movies { get; set; }
}

public class Movie
{
    public string Title { get; set; }
    public int Year { get; set; }
    public double Rating { get; set; }
    public string Director { get; set; }
}
```
- Define the exact structure you want
- Use standard C# types and properties
- Can include nested objects, lists, enums

### Without Structured Output (Unreliable)
```csharp
AIAgent agent1 = client
    .GetChatClient(configuration.ChatDeploymentName)
    .CreateAIAgent(instructions: "You are an expert in IMDB Lists");

AgentRunResponse response1 = await agent1.RunAsync("What are the top 10 Movies according to IMDB?");
Console.WriteLine(response1);
```
- Returns plain text
- Format varies each time
- Requires manual parsing
- Error-prone

### With Structured Output (Guaranteed Format)
```csharp
ChatClientAgent agent2 = client
    .GetChatClient(configuration.ChatDeploymentName)
    .CreateAIAgent(instructions: "You are an expert in IMDB Lists");

AgentRunResponse<MovieResult> response2 = await agent2.RunAsync<MovieResult>(
    "What are the top 10 Movies according to IMDB?"
);

MovieResult movieResult = response2.Result;
```
- Returns strongly-typed object
- Guaranteed structure
- No parsing needed
- Type-safe access

### Using the Structured Data
```csharp
foreach (Movie movie in movieResult.Movies)
{
    Console.WriteLine($"{movie.Title} ({movie.Year}) - Rating: {movie.Rating}/10");
}
```

### Alternative: Manual JSON Schema Configuration
```csharp
JsonSerializerOptions jsonSerializerOptions = new()
{
    PropertyNameCaseInsensitive = true,
    TypeInfoResolver = new DefaultJsonTypeInfoResolver(),
    Converters = { new JsonStringEnumConverter() }
};

AIAgent agent3 = client
    .GetChatClient(configuration.ChatDeploymentName)
    .CreateAIAgent(
        instructions: "You are an expert in IMDB Lists",
        jsonOptions: jsonSerializerOptions
    );
```

## 🎓 Key Concepts

### Structured vs Unstructured Output

| Unstructured Output | Structured Output |
|---------------------|-------------------|
| Plain text | Typed objects |
| Format varies | Format guaranteed |
| Manual parsing needed | Automatic deserialization |
| Error-prone | Type-safe |
| Human-readable | Machine-friendly |
| Good for: UI display | Good for: System integration |

### How Structured Output Works
1. **You define**: C# class with desired structure
2. **Framework generates**: JSON schema from your class
3. **AI receives**: Schema as constraint
4. **AI generates**: JSON matching schema exactly
5. **Framework deserializes**: JSON to your C# object
6. **You receive**: Strongly-typed result

### Supported Types
✅ **Primitive Types:**
- `string`, `int`, `double`, `bool`, `DateTime`

✅ **Collections:**
- `List<T>`, `T[]`, `Dictionary<string, T>`

✅ **Complex Types:**
- Nested classes
- Enums
- Nullable types

✅ **Attributes:**
- `[Required]`, `[JsonPropertyName]`, `[Description]`

## 🛠️ Advanced Examples

### Using Enums
```csharp
public enum Genre
{
    Action,
    Comedy,
    Drama,
    SciFi,
    Horror
}

public class Movie
{
    public string Title { get; set; }
    public Genre Genre { get; set; }
}
```

### Required Properties
```csharp
using System.ComponentModel.DataAnnotations;

public class Movie
{
    [Required]
    public string Title { get; set; }
    
    [Required]
    public int Year { get; set; }
    
    public string? Director { get; set; }  // Optional
}
```

### Property Descriptions
```csharp
using System.ComponentModel;

public class Movie
{
    [Description("The full title of the movie")]
    public string Title { get; set; }
    
    [Description("IMDB rating from 0.0 to 10.0")]
    public double Rating { get; set; }
}
```

### Nested Objects
```csharp
public class MovieResult
{
    public List<Movie> Movies { get; set; }
    public SearchMetadata Metadata { get; set; }
}

public class SearchMetadata
{
    public int TotalResults { get; set; }
    public DateTime SearchDate { get; set; }
}
```

## 📊 Expected Output

### Without Structured Output:
```
Here are the top 10 movies according to IMDB:
1. The Shawshank Redemption (1994) - 9.3/10
2. The Godfather (1972) - 9.2/10
...
```

### With Structured Output:
```csharp
MovieResult {
    Movies = [
        { Title = "The Shawshank Redemption", Year = 1994, Rating = 9.3 },
        { Title = "The Godfather", Year = 1972, Rating = 9.2 },
        ...
    ]
}
```

## 🎯 Use Cases
- **API Integration**: Return data for other systems
- **Database Storage**: Direct insertion into databases
- **Data Processing**: Feed into analytics pipelines
- **UI Rendering**: Bind directly to UI components
- **Validation**: Ensure data meets requirements
- **Type Safety**: Catch errors at compile time
- **Multi-step Workflows**: Pass typed data between agents

## 🌟 Best Practices

### 1. Clear Property Names
```csharp
// Good
public string MovieTitle { get; set; }

// Bad
public string MT { get; set; }
```

### 2. Add Descriptions
```csharp
[Description("The year the movie was released (e.g., 1994)")]
public int ReleaseYear { get; set; }
```

### 3. Use Appropriate Types
```csharp
// Good
public DateTime ReleaseDate { get; set; }
public decimal BoxOffice { get; set; }

// Less ideal
public string ReleaseDate { get; set; }
public string BoxOffice { get; set; }
```

### 4. Handle Nullability
```csharp
public string Title { get; set; }           // Required
public string? Subtitle { get; set; }       // Optional
public List<string> Actors { get; set; }    // Required list
public List<string>? Awards { get; set; }   // Optional list
```

### 5. Keep Schemas Simple
- Avoid overly deep nesting
- Limit to essential properties
- Complex schemas may confuse AI
- Test with sample data first

## 🚨 Common Pitfalls

### ❌ Schema Too Complex
```csharp
// Too many nested levels
public class Level1 {
    public Level2 Child { get; set; }
}
public class Level2 {
    public Level3 Child { get; set; }
}
// ... Level3, Level4, Level5...
```

### ❌ Ambiguous Property Names
```csharp
public string Data { get; set; }      // What data?
public string Info { get; set; }      // What info?
```

### ✅ Better Approach
```csharp
public string MovieTitle { get; set; }
public string PlotSummary { get; set; }
```

## 💰 Cost Considerations
- Structured output adds tokens to response
- More complex schemas = more tokens
- AI needs to generate valid JSON
- Typically small overhead (5-10% more tokens)
- Worth it for reliability and integration

## 🔗 Related Projects
- **MultiAgent.ManualViaStructuredOutput** - Agents coordinating via structured data
- **ToolCalling.Basics** - Tools can return structured data
- **Workflow.Sequential** - Pass structured data between workflow steps
- **UsingRAGInAgentFramework** - Structured retrieval results

## 📚 Additional Resources
- [Structured Output Documentation](https://learn.microsoft.com/azure/ai-services/openai/how-to/structured-outputs)
- [JSON Schema](https://json-schema.org/)
- [System.Text.Json](https://learn.microsoft.com/dotnet/standard/serialization/system-text-json/)

## 🐛 Troubleshooting

### Schema Validation Errors
- Simplify your schema
- Remove circular references
- Ensure all types are serializable
- Test JSON schema generation

### AI Doesn't Follow Schema
- Some older models don't support structured output
- Use gpt-4o or gpt-4o-mini
- Verify model supports JSON mode
- Check if schema is too complex

### Deserialization Errors
- Verify property names match
- Check for missing required properties
- Use `[JsonPropertyName]` for mismatches
- Enable `PropertyNameCaseInsensitive`

### Null Reference Errors
- Use nullable types (`string?`)
- Initialize collections in constructor
- Add null checks when using data

## 🎤 Presentation Talking Points
1. **The Problem** - Plain text is unreliable for integration
2. **The Solution** - Force AI to return specific structures
3. **Demo: Without Structured Output** - Show inconsistent results
4. **Demo: With Structured Output** - Show guaranteed format
5. **How It Works** - JSON schema generation from C# classes
6. **Define Your Schema** - Show model creation
7. **Using the Data** - Type-safe access, no parsing
8. **Advanced Features** - Enums, nested objects, descriptions
9. **Best Practices** - Clear names, appropriate types
10. **Real-world Use Cases** - API integration, databases, workflows
11. **Cost & Performance** - Minimal overhead for major benefits
12. **Next Steps** - Multi-agent coordination with structured data

