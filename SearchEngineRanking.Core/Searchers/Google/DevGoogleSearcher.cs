using SearchEngineRanking.Core.Delegates;
using SearchEngineRanking.Core.Messages.Responses;

namespace SearchEngineRanking.Core.Searchers.Google;

public class DevGoogleSearcher : IGoogleSearcher
{
  public event SearchResultsLoadedCallback? ProcessCompleted;
  public event SearchResultsLoadedErrorCallback? ProcessError;

  public async Task SearchGoogleAsync(string keywords)
  {
    var rawHtml = await File.ReadAllTextAsync(@"D:\Temp\google-search-results.html");
    ProcessCompleted?.Invoke(new GoogleSearchResultResponse { RawHTML = rawHtml});
  }
}