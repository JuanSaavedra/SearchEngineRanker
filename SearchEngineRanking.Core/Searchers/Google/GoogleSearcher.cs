using SearchEngineRanking.Core.Delegates;
using SearchEngineRanking.Core.Messages.Responses;

namespace SearchEngineRanking.Core.Searchers.Google;

public class GoogleSearcher : IGoogleSearcher
{
  public event SearchResultsLoadedCallback? ProcessCompleted; // event
  public event SearchResultsLoadedErrorCallback? ProcessError; // event
  private const string GoogleSearchEngineURI = "www.google.com";
  public const int MaxResults = 100;
  
  public string GetSearchEngineUriString(string keywords)
  {
    var searchTerm = keywords.Replace(" ", "+");
    var uri = $"https://{GoogleSearchEngineURI}/search?num={MaxResults}&q={searchTerm}";
    return uri;
  }

  public async Task SearchGoogleAsync(string keywords)
  {
    using var client = new HttpClient();
    var httpResponse = await client.GetAsync(GetSearchEngineUriString(keywords));

    if (httpResponse.IsSuccessStatusCode)
    {
      var readAsString = httpResponse.Content.ReadAsStringAsync().Result;
      var googleResponse = new GoogleSearchResultResponse
      {
        RawHTML = readAsString
      };
      
      ProcessCompleted?.Invoke(googleResponse);
    }
    else
    {
      ProcessError?.Invoke("ERROR: Status Code " + httpResponse.StatusCode
                                                 + " - " + httpResponse.ReasonPhrase);
    }
  }
}