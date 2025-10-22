**Microsoft Agent Framework Sample**

This sample shows some of the features of the new Microsoft Agent Framework, released in preview in October 1, 2025.  

Microsoft Agent Framework references:
 - https://devblogs.microsoft.com/foundry/introducing-microsoft-agent-framework-the-open-source-engine-for-agentic-ai-apps/
 - https://azure.microsoft.com/en-us/blog/introducing-microsoft-agent-framework/

**Notes on the framework**
The framework has simplified syntax, with the ability to set multiple properties and then call two functions, one to create the agent, and the other to call the agent. For example:

```
  // create the agent
  AIAgent agent = new AzureOpenAIClient(AzureOpenAIEndpoint, new AzureCliCredential())
      .GetChatClient("gpt-4.1")
      .CreateAIAgent(instructions: "You are good at telling jokes.");
  
  // ask a question and get the response.
  var response = await agent.RunAsync("Tell me a joke about a pirate.");
```

This is a C# console program with the following functions:

```
await Agent_AskQuestion();                  //Ask a question and receive a response.
  
await Agent_AskQuestionStreamResponse();    //Ask the agent a question and get a streaming response.

await Agent_AskWithChatMessage();           //Ask the agent a question and pass in content such as a URL. Ask the agent questions about the contents of the URL.
  
await Agent_AskWithImage();                 //Ask the agent a question and pass in an image URL. Ask the agent a question about the image.

await Agent_MultiTurnConversation();        //Multi-turn conversation with the agent (chat history is preserved).
```
