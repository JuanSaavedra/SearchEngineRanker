using SearchEngineRanking.Core.Messages.Responses;

namespace SearchEngineRanking.Core.Scrappers
{
  public interface ISearchResultsScrapper
  {
    public SearchResultsScrapperResponse ProcessSearchResults(string data);
  }
}

