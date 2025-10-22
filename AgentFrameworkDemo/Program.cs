using System;
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;

namespace AgentFrameworkDemo;

class Program
{
    // Azure OpenAI endpoint
    private static readonly Uri AzureOpenAIEndpoint = new("YOUR_AZURE_OPENAI_ENDPOINT_HERE"); //TODO!: Add your Azure OpenAI endpoint here.
    
    static async Task Main(string[] args)
    {
        Console.WriteLine("-- Microsoft Agent Framework Demo --\n");

        try
        {
            //TODO!: Uncomment any of the following methods to try the different scenarios.
            
            // ask the agent a question.
            await Agent_AskQuestion();

            // ask the agent a question and get a streaming response (plain text).
            //await Agent_AskQuestionStreamResponse();

            // ask the agent a question and pass in content such as a URL.
            //await Agent_AskWithChatMessage();

            // ask the agent a question and pass in an image URL.
            //await Agent_AskWithImage();

            // have a multi-turn conversation with the agent (chat history is preserved).
            //await Agent_MultiTurnConversation();
        }

        catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
        
        Console.WriteLine("\nDemo complete! Press any key to exit...");
        Console.ReadKey();
    }

    /// <summary>
    /// Ask the agent a question in plain text, and receive a plain text response.
    /// </summary>
    static async Task Agent_AskQuestion()
    {
        Console.WriteLine("=== Agent_AskQuestion ===");
        
        AIAgent agent = new AzureOpenAIClient(AzureOpenAIEndpoint, new AzureCliCredential())
            .GetChatClient("gpt-4.1")
            .CreateAIAgent(instructions: "You are good at telling jokes.");
        
        Console.WriteLine("Requesting a pirate joke...\n");
        var response = await agent.RunAsync("Tell me a joke about a pirate.");
        Console.WriteLine($"Response: {response}\n");
    }

    /// <summary>
    /// Ask the agent a question and get a streaming response (plain text).
    /// </summary>
    static async Task Agent_AskQuestionStreamResponse()
    {
        Console.WriteLine("=== Agent_AskQuestionStreamResponse ===");
        
        AIAgent agent = new AzureOpenAIClient(AzureOpenAIEndpoint, new AzureCliCredential())
            .GetChatClient("gpt-4.1")
            .CreateAIAgent(instructions: "You are good at telling jokes.");
        
        Console.WriteLine("Requesting another pirate joke with streaming...\n");
        Console.Write("Streaming response: ");
        
        await foreach (var update in agent.RunStreamingAsync("Tell me a joke about a pirate."))
        {
            Console.Write(update);
            await Task.Delay(100);
        }
        
        Console.WriteLine("\n");
    }

    /// <summary>
    /// Ask the agent a question and pass in content such as a URL.
    /// </summary>
    static async Task Agent_AskWithChatMessage()
    {
        Console.WriteLine("=== Agent_AskWithChatMessage ===");
        
        AIAgent agent = new AzureOpenAIClient(AzureOpenAIEndpoint, new AzureCliCredential())
            .GetChatClient("gpt-4.1")
            .CreateAIAgent(instructions: "You are a helpful assistant.");

        Console.WriteLine("Using system message to change agent behavior...\n");
        
        ChatMessage message = new(ChatRole.User, [
            new TextContent("Tell me a joke about this image?"),
            new UriContent("https://upload.wikimedia.org/wikipedia/commons/1/11/Joseph_Grimaldi.jpg", "image/jpeg")
        ]);

        Console.WriteLine(await agent.RunAsync(message));
        Console.WriteLine();      
        
        ChatMessage systemMessage = new(
            ChatRole.System,
            """
            If the user asks you to tell a joke, refuse to do so, explaining that you are not a clown.
            Offer the user an interesting fact instead.
            """);
        ChatMessage userMessage = new(ChatRole.User, "Tell me a joke about a pirate.");

        Console.WriteLine(await agent.RunAsync([systemMessage, userMessage]));
        Console.WriteLine();
    }

    /// <summary>
    /// Ask the agent a question and pass in an image URL.
    /// </summary>
    static async Task Agent_AskWithImage()
    {
        Console.WriteLine("=== Agent_AskWithImage ===");
        
        AIAgent agent = new AzureOpenAIClient(AzureOpenAIEndpoint, new AzureCliCredential())
            .GetChatClient("gpt-4.1")
            .CreateAIAgent(
                name: "VisionAgent",
                instructions: "You are a helpful agent that can analyze images");
        
        Console.WriteLine("Analyzing an image...\n");
        
        ChatMessage message = new(ChatRole.User, [
            new TextContent("What do you see in this image?"),
            new UriContent("https://upload.wikimedia.org/wikipedia/commons/thumb/d/dd/Gfp-wisconsin-madison-the-nature-boardwalk.jpg/2560px-Gfp-wisconsin-madison-the-nature-boardwalk.jpg", "image/jpeg")
        ]);

        Console.WriteLine(await agent.RunAsync(message));
        Console.WriteLine();
    }

    /// <summary>
    /// Have a multi-turn conversation with the agent (chat history is preserved).
    /// </summary>
    static async Task Agent_MultiTurnConversation()
    {
        Console.WriteLine("=== Agent_MultiTurnConversation ===");
        
        AIAgent agent = new AzureOpenAIClient(AzureOpenAIEndpoint, new AzureCliCredential())
            .GetChatClient("gpt-4.1")
            .CreateAIAgent(instructions: "You are good at telling jokes.");
        
        Console.WriteLine("Starting multi-turn conversation...\n");
        
        AgentThread thread = agent.GetNewThread();

        Console.WriteLine("First turn:");
        Console.WriteLine(await agent.RunAsync("Tell me a joke about a pirate.", thread));
        Console.WriteLine();
        
        Console.WriteLine("Second turn (referencing previous joke):");
        Console.WriteLine(await agent.RunAsync("Now add some emojis to the joke and tell it in the voice of a pirate's parrot.", thread));
        Console.WriteLine();
    }
}
