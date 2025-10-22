** Microsoft Agent Framework Sample

This sample shows some of the features of the new Microsoft Agent Framework.
One of the features of the new framework is the ability to apply multiple parameters to one or two function calls total to call an agent. For example:

'''
  // create the agent
  AIAgent agent = new AzureOpenAIClient(AzureOpenAIEndpoint, new AzureCliCredential())
      .GetChatClient("gpt-4.1")
      .CreateAIAgent(instructions: "You are good at telling jokes.");
  
  // send the question and get the response.
  var response = await agent.RunAsync("Tell me a joke about a pirate.");
'''

This is a C# console program with the following functions:

await Agent_AskQuestion();
- Ask a question and receive a response.
  
await Agent_AskQuestionStreamResponse();
- Ask the agent a question and get a streaming response.

await Agent_AskWithChatMessage();
- Ask the agent a question and pass in content such as a URL. Ask the agent questions about the contents of the URL.
  
await Agent_AskWithImage();
- Ask the agent a question and pass in an image URL. Ask the agent a question about the image.

// have a multi-turn conversation with the agent (chat history is preserved).
//await Agent_MultiTurnConversation();
