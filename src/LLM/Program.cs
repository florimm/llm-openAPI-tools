#pragma warning disable SKEXP0001
#pragma warning disable SKEXP0010
#pragma warning disable SKEXP0110
#pragma warning disable SKEXP0040

using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using SKAgentLocalFunctionCalling.Plugins;
using System.Reflection;
using System.Text;
using Microsoft.SemanticKernel.Plugins.OpenApi;

var config = new ConfigurationBuilder()
    .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
    .Build();

var modelId = "llama3.1";
var baseUrl = "http://localhost:11434";

var httpClient = new HttpClient
{
    Timeout = TimeSpan.FromMinutes(2)
};

var builder = Kernel.CreateBuilder().AddOpenAIChatCompletion(modelId: modelId!, apiKey: null, endpoint: new Uri(baseUrl!), httpClient: httpClient);
var kernel = builder.Build();

var HostName = "AI Assistant";
var HostInstructions = @"You are a helpful Assistant to answer their queries. Be respectful, short and precise in answering the queries and just anwering without repeating or any other explenation.
If the queries are related to getting the time or menu, Use the available plugin functions to get the answer.";

var settings = new OpenAIPromptExecutionSettings() { Temperature = 0.0, ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions };

ChatCompletionAgent agent =
           new()
           {
               Instructions = HostInstructions,
               Name = HostName,
               Kernel = kernel,
               Arguments = new(settings),
           };

KernelPlugin localTimePlugin = KernelPluginFactory.CreateFromType<LocalTimePlugin>();
agent.Kernel.Plugins.Add(localTimePlugin);


await kernel.ImportPluginFromOpenApiAsync("Menu", new Uri("http://localhost:5114/swagger/v1/swagger.json"));

Console.WriteLine("Assistant: Hello, I am your Assistant. How may i help you?");

AgentGroupChat chat = new();


try
{
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("User: ");
    await InvokeAgentAsync("What do we have today?");
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("User: ");
    await InvokeAgentAsync("Ingedients for Truffle Risotto ?");
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("User: ");
    await InvokeAgentAsync("Price for Truffle Risotto ?");
}
catch (Exception)
{

    throw;
}


// Local function to invoke agent and display the conversation messages.
async Task InvokeAgentAsync(string question)
{
    chat.AddChatMessage(new ChatMessageContent(AuthorRole.User, question));

    Console.ForegroundColor = ConsoleColor.Green;

    await foreach (ChatMessageContent content in chat.InvokeAsync(agent))
    {
        Console.WriteLine(content.Content);
    }
}

#pragma warning restore SKEXP0001
#pragma warning restore SKEXP0010
#pragma warning restore SKEXP0110
