using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Text.Json;
using System.Web;

namespace Plugins;

public class BingPlugin
{
  [KernelFunction("Search")]
  [Description("Searches Bing for the specified query and returns the results")]
  public async Task<BingResults> SearchAsync(string userQuery)
  {
    Console.WriteLine("Searching Bing for: " + userQuery);
    var client = new HttpClient();
    string endpoint = "https://api.bing.microsoft.com/v7.0/search?";
    string apiKey = Environment.GetEnvironmentVariable("bingApiKey", EnvironmentVariableTarget.User)!;
    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey);
    var queryString = HttpUtility.ParseQueryString(string.Empty);
    queryString["q"] = userQuery;
    var query = endpoint + queryString;

    // Run the query
    HttpResponseMessage httpResponseMessage = await client.GetAsync(query).ConfigureAwait(false);
    var responseContentString = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
    // Deserialize the response content
    var responseObjects = JsonSerializer.Deserialize<BingSearchResponse>(responseContentString);

    // Convert the response to a BingResults object
    var results = new BingResults
    {
      Results = new List<BingResult>()
    };

    if (responseObjects.webPages != null)
      foreach (var result in responseObjects.webPages.value)
      {
        results.Results.Add(new BingResult
        {
          Title = result.name.ToString(),
          Description = result.snippet.ToString(),
          Url = result.url.ToString()
        });
      }

    return results;
  }
}

public class BingResults
{
  public List<BingResult> Results { get; set; }
}

public class BingResult
{
  public string Title { get; set; }
  public string Description { get; set; }
  public string Url { get; set; }
}

public class WebPages
{
  public string webSearchUrl { get; set; }
  public int totalEstimatedMatches { get; set; }
  public List<Value> value { get; set; }
}

public class Value
{
  public string id { get; set; }
  public string name { get; set; }
  public string url { get; set; }
  public string displayUrl { get; set; }
  public string snippet { get; set; }
}

public class BingSearchResponse
{
  public WebPages webPages { get; set; }
}