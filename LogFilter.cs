#pragma warning disable SKEXP0001
using Microsoft.SemanticKernel;

namespace Filters;

public class LogFilter() : IFunctionInvocationFilter
{
  public async Task OnFunctionInvocationAsync(FunctionInvocationContext context, Func<FunctionInvocationContext, Task> next)
  {
    Console.Write("Calling function: " + context.Function.PluginName + "." + context.Function.Name);
    await next(context);
    Console.WriteLine(" ...done");
  }
}
