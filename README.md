# Sample code to demonstrate use of Semantic Kernel.

After cloning or copying the repo, you'll need to open the solution in Visual Studio, which should automatically resolve the dependencies.

You will also need an Azure subscription with an Azure OpenAI resource provisioned and a deployed model (GPT3.5 or GPT4). You also need to create environment variables for your Azure OpenAI **endpoint**, **apiKey**, and the **deploymentname**.

You can also use an OpenAI subscription if you have one, but you'll need to adjust the code by changing line 14 to:
```
builder.Services.AddOpenAIChatCompletion(modelID, endpoint, apiKey);
```
Note that OpenAI uses a **modelID** rather than **deploymentName**, and you may also need to supply other parameters. It should work the same, but I haven't tested using the model through OpenAI directly.

You can also use Visual Studio Code and resolve the NuGet dependencies and setup at the command line:

```
# set up environment variables
[Environment]::SetEnvironmentVariable("endpoint", "https://xxxxxx.openai.azure.com/", "Machine")
[Environment]::SetEnvironmentVariable("deploymentName", "xxxx", "User")
[Environment]::SetEnvironmentVariable("apiKey", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", "User")

# install packages
dotnet add package Microsoft.SemanticKernel
```
