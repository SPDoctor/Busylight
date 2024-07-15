using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace Plugins;

public class ModelsPlugin
{
 
  [KernelFunction, Description("Get the name for a given product SKU")]
  public static string Name([Description("The product SKU")] string SKU)
  {
    if (models.TryGetValue(SKU, out var data))
    {
      return data.name;
    }
    return "";
  }

  [KernelFunction, Description("Get the range in miles for a given product SKU")]
  public static double Range([Description("The product SKU")] string SKU)
  {
    if (models.TryGetValue(SKU, out var data))
    {
      return data.range;
    }
    return -1;
  }

  [KernelFunction, Description("Get the length in metres for a given product SKU")]
  public static double Length([Description("The product SKU")] string SKU)
  {
    if (models.TryGetValue(SKU, out var data))
    {
      return data.length;
    }
    return -1;
  }

  [KernelFunction, Description("Get the colour for a given product SKU")]
  public static string Colour([Description("The product SKU")] string SKU)
  {
    if (models.TryGetValue(SKU, out var data))
    {
      return data.colour;
    }
    return "";
  }

  // look up table for name, colour, length, and range for a given SKU
  private static Dictionary<string, (string name, string colour, double length, double range)> models = new()
  {
    { "J10W", ("Jupiter", "white", 3.7, 150.0)},
    { "J10B", ("Jupiter", "black", 3.7, 150.0) },
    { "S12R", ("Saturn", "red", 4.2, 200.0) },
    { "S12B", ("Saturn", "black", 4.2, 200.0) },
    { "S12G", ("Saturn", "blue", 4.2, 200.0) },
    { "N20B", ("Neptune", "black", 4.6, 250.0) },
    { "N20G", ("Neptune", "blue", 4.6, 250.0) },
    { "M500", ("Mercury", "black", 4.9, 250.0) }
  };
}