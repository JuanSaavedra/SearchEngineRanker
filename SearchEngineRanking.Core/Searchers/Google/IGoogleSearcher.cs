using SearchEngineRanking.Core.Delegates;

namespace SearchEngineRanking.Core.Searchers.Google;

public interface IGoogleSearcher
{
  event SearchResultsLoadedCallback ProcessCompleted; // event
  event SearchResultsLoadedErrorCallback ProcessError; // event
  Task SearchGoogleAsync(string keywords);
}