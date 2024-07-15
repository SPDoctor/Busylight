using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Plugins;

var builder = Kernel.CreateBuilder();
string deploymentName = Environment.GetEnvironmentVariable("deploymentname", EnvironmentVariableTarget.User)!;
string endpoint = Environment.GetEnvironmentVariable("endpoint", EnvironmentVariableTarget.User)!;
string apiKey = Environment.GetEnvironmentVariable("apiKey", EnvironmentVariableTarget.User)!;

builder.Services.AddAzureOpenAIChatCompletion("gpt4", endpoint, apiKey);
builder.Plugins.AddFromType<CalcPlugin>();
builder.Plugins.AddFromType<LightPlugin>();
builder.Plugins.AddFromType<TimePlugin>();
builder.Plugins.AddFromType<ModelsPlugin>();
builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddConsole().SetMinimumLevel(LogLevel.Information));
var kernel = builder.Build();

// Create chat history
ChatHistory history = [];
history.AddSystemMessage("You are a helpful assistant. If the user doesn't provide enough information for you to complete a task, you will keep asking questions until you have enough information to complete the task. Please explain your reasoning with response.");

// Get chat completion service
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

// enable auto function calling
OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
{
  ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
};

// Start the conversation
while (true)
{
  // Get user input
  Console.Write("User > ");
  var userMessage = Console.ReadLine()!;
  if(userMessage == "exit" || userMessage == "quit") break;
  if(userMessage == "") continue;
  history.AddUserMessage(userMessage);

  // Get the response from the AI
  var result = chatCompletionService.GetStreamingChatMessageContentsAsync(
      history,
      executionSettings: openAIPromptExecutionSettings,
      kernel: kernel);

  // Stream the results
  string fullMessage = "";
  var first = true;
  await foreach (var content in result)
  {
    if (content.Role.HasValue && first)
    {
      Console.Write("Assistant > ");
      first = false;
    }
    Console.Write(content.Content);
    fullMessage += content.Content;
  }
  Console.WriteLine();

  // Add the message from the agent to the chat history
  history.AddAssistantMessage(fullMessage);
}