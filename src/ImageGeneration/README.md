# Image Generation

## 📋 Overview
This project demonstrates how to generate images using AI models through Azure OpenAI or OpenAI. Create images from text descriptions programmatically, enabling creative applications, content generation, and visual AI experiences.

## 🎯 Learning Objectives
- Generate images from text prompts
- Configure image generation parameters
- Handle image output (bytes, files)
- Understand DALL-E and other image models
- Save and display generated images

## 🎥 Video Tutorial
[Watch on YouTube](https://youtu.be/F8BxvnpWJ9s)

## 📦 Prerequisites
- .NET 8.0 or higher
- Azure OpenAI resource with DALL-E deployment OR OpenAI API key
- Understanding of basic API calls

## 🔧 NuGet Packages Required
```xml
<PackageReference Include="Azure.AI.OpenAI" />
<PackageReference Include="OpenAI" />
```

## 🚀 Quick Start

### Step 1: Configure User Secrets
```bash
# For Azure OpenAI
dotnet user-secrets set "AzureOpenAiEndpoint" "https://your-resource.openai.azure.com/"
dotnet user-secrets set "AzureOpenAiKey" "your-api-key"

# For OpenAI
dotnet user-secrets set "OpenAiApiKey" "your-api-key"
```

### Step 2: Deploy DALL-E Model (Azure only)
1. Navigate to Azure OpenAI resource
2. Deploy DALL-E 3 or DALL-E 2 model
3. Note the deployment name

### Step 3: Run the Application
```bash
dotnet run
```

## 💡 Code Explanation

### Using Azure OpenAI
```csharp
AzureOpenAIClient client = new(
    new Uri(configuration.AzureOpenAiEndpoint), 
    new ApiKeyCredential(configuration.AzureOpenAiKey)
);

ImageClient imageClient = client.GetImageClient("dalle-3");  // Your deployment name
```

### Using OpenAI Directly
```csharp
OpenAIClient client = new(configuration.OpenAiApiKey);
ImageClient imageClient = client.GetImageClient("dall-e-3");
```

### Generating an Image
```csharp
ClientResult<GeneratedImage> image = await imageClient.GenerateImageAsync(
    "A Tiger in a jungle with a party-hat", 
    new ImageGenerationOptions
    {
        Background = GeneratedImageBackground.Auto,
        Quality = GeneratedImageQuality.Auto,
        Size = GeneratedImageSize.W1024xH1024,
        OutputFileFormat = GeneratedImageFileFormat.Png,
    }
);
```

### Saving and Opening the Image
```csharp
byte[] bytes = image.Value.ImageBytes.ToArray();
string path = Path.Combine(Path.GetTempPath(), $"image-{Guid.NewGuid():N}.png");
File.WriteAllBytes(path, bytes);

// Open with default image viewer
Process.Start(new ProcessStartInfo
{
    FileName = path,
    UseShellExecute = true
});
```

## 🎓 Key Concepts

### Available Image Models

**DALL-E 3** (Recommended):
- Highest quality
- Better prompt understanding
- 1024x1024, 1024x1792, 1792x1024 sizes
- More expensive
- Natural language prompts

**DALL-E 2**:
- Good quality
- Faster generation
- 256x256, 512x512, 1024x1024 sizes
- More affordable
- Simpler prompts

### Image Generation Options

**Size**:
```csharp
GeneratedImageSize.W1024xH1024  // Square (default)
GeneratedImageSize.W1024xH1792  // Portrait
GeneratedImageSize.W1792xH1024  // Landscape
```

**Quality**:
```csharp
GeneratedImageQuality.Auto      // Default
GeneratedImageQuality.Standard  // Faster, cheaper
GeneratedImageQuality.High      // DALL-E 3 only, detailed
```

**Style** (DALL-E 3 only):
```csharp
GeneratedImageStyle.Natural     // Photographic
GeneratedImageStyle.Vivid       // Hyper-real, dramatic (default)
```

**Format**:
```csharp
GeneratedImageFileFormat.Png    // Recommended
GeneratedImageFileFormat.Jpeg   // Smaller file size
```

### Response Types

**Image Bytes**:
```csharp
byte[] bytes = image.Value.ImageBytes.ToArray();
File.WriteAllBytes("image.png", bytes);
```

**Image URL** (when available):
```csharp
Uri imageUrl = image.Value.ImageUri;
// Download or display directly
```

## 🎨 Prompt Engineering Tips

### Good Prompts
```csharp
// Specific and descriptive
"A photorealistic tiger wearing a colorful party hat, standing in a lush green jungle at sunset, cinematic lighting"

// Include style
"A tiger with a party hat, oil painting style, vibrant colors"

// Specify composition
"Close-up portrait of a tiger wearing a party hat, studio photography, professional lighting"
```

### Avoid Vague Prompts
```csharp
// Too vague
"tiger"

// Better
"A majestic Bengal tiger in its natural habitat"
```

### Prompt Structure
```
[Subject] + [Action/Pose] + [Setting] + [Style] + [Lighting] + [Details]
```

Example:
```
"A tiger (subject) sitting proudly (action) in a dense jungle (setting), 
watercolor painting style (style), soft natural lighting (lighting), 
with vibrant green foliage (details)"
```

## 📊 Expected Output
- PNG or JPEG file saved to temp directory
- Image opened in default image viewer
- Console shows file path

## 🎯 Use Cases
- **Content Creation**: Blog posts, social media
- **Product Mockups**: Visualize concepts
- **Art Generation**: Creative projects
- **Marketing**: Advertising materials
- **Game Development**: Asset creation
- **Education**: Visual learning aids
- **Presentations**: Custom graphics
- **Prototyping**: UI/UX concepts

## 🌟 Best Practices

### 1. Clear, Detailed Prompts
```csharp
// Good
"A professional studio photograph of a golden retriever puppy sitting on a white cushion, soft lighting, shallow depth of field"

// Less effective
"dog photo"
```

### 2. Error Handling
```csharp
try
{
    var image = await imageClient.GenerateImageAsync(prompt, options);
}
catch (ClientResultException ex) when (ex.Status == 400)
{
    // Prompt violated content policy
    Console.WriteLine("Prompt was rejected. Try a different description.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
```

### 3. Content Policy Compliance
```csharp
// Avoid prompts that:
// - Contain violence or gore
// - Include named individuals
// - Generate copyrighted characters
// - Contain adult content
// - Promote illegal activities
```

### 4. Retry Logic
```csharp
int maxRetries = 3;
for (int i = 0; i < maxRetries; i++)
{
    try
    {
        var image = await imageClient.GenerateImageAsync(prompt, options);
        break;
    }
    catch (Exception) when (i < maxRetries - 1)
    {
        await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, i)));
    }
}
```

### 5. Optimize for Cost
```csharp
// Use Standard quality for drafts
Quality = GeneratedImageQuality.Standard,

// Use smaller sizes when possible
Size = GeneratedImageSize.W1024xH1024,

// Use DALL-E 2 for bulk generation
```

## 💰 Cost Considerations

### DALL-E 3 Pricing (approximate)
- **1024x1024**: ~$0.040 per image
- **1024x1792 / 1792x1024**: ~$0.080 per image
- **HD Quality**: Additional cost

### DALL-E 2 Pricing
- **1024x1024**: ~$0.020 per image
- **512x512**: ~$0.018 per image
- **256x256**: ~$0.016 per image

### Cost Optimization
- Use DALL-E 2 for high-volume needs
- Standard quality vs HD
- Smaller sizes when appropriate
- Cache and reuse images
- Batch generation for efficiency

## 🚨 Content Policy

### Prohibited Content
- ❌ Violence, gore, self-harm
- ❌ Sexual or adult content
- ❌ Hate symbols or speech
- ❌ Named public figures
- ❌ Copyrighted characters (Mickey Mouse, etc.)
- ❌ Illegal activities

### Allowed Content
- ✅ Animals, nature, landscapes
- ✅ Abstract art, patterns
- ✅ Objects, products, food
- ✅ Architecture, interiors
- ✅ Generic people (no specific identities)
- ✅ Fictional characters (original)

## 🔗 Related Projects
- **ImageGeneration.GoogleGemini** - Google's image generation
- **ImageGeneration.XAiGrok** - X.AI's image generation
- **ToolCalling.Basics** - Image generation as a tool
- **MultiAgent.AgentAsTool** - Agent-driven image creation

## 📚 Additional Resources
- [DALL-E Documentation](https://platform.openai.com/docs/guides/images)
- [Azure OpenAI Image Generation](https://learn.microsoft.com/azure/ai-services/openai/how-to/dall-e)
- [Prompt Engineering Guide](https://platform.openai.com/docs/guides/prompt-engineering)
- [Content Policy](https://openai.com/policies/usage-policies)

## 🐛 Troubleshooting

### "Content policy violation" Error
- Review prompt for prohibited content
- Remove specific names or brands
- Make prompt more family-friendly
- Avoid controversial topics

### Poor Image Quality
- Add more descriptive details
- Specify art style or medium
- Include lighting and composition terms
- Try HD quality (DALL-E 3)
- Use larger image size

### Generation Takes Long Time
- Normal for detailed images (20-60 seconds)
- DALL-E 3 is slower than DALL-E 2
- Check network connectivity
- Azure region may affect speed

### "Model not found" Error
- Verify deployment name (Azure)
- Ensure DALL-E is deployed
- Check model availability in region
- Confirm API has image generation access

## 🎤 Presentation Talking Points
1. **Introduction** - AI-powered image generation
2. **Use Cases** - Content creation, marketing, prototyping
3. **DALL-E Models** - Comparison of 2 vs 3
4. **Setup** - Azure vs OpenAI
5. **Live Demo** - Generate image from prompt
6. **Prompt Engineering** - Writing effective prompts
7. **Configuration Options** - Size, quality, style
8. **Output Handling** - Save and display images
9. **Content Policy** - What's allowed and prohibited
10. **Cost Analysis** - Pricing and optimization
11. **Real-world Examples** - Show various generated images
12. **Next Steps** - Integration with agents and workflows

