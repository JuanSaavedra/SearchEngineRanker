using SearchEngineRanking.Core.Messages.Responses;
using SearchEngineRanking.Core.Models;
using SearchEngineRanking.Core.Services.Google;

namespace SearchEngineRanking.Core.Scrappers.Google
{
  public class GoogleSearchResultsScrapper : ISearchResultsScrapper
  {
    public IGoogleSearchResultScrapperRegexProvider _regexProvider { get; set; }

    public GoogleSearchResultsScrapper(IGoogleSearchResultScrapperRegexProvider regexProvider)
    {
      _regexProvider = regexProvider;
    }

    /// <summary>
    /// Gets the main search results
    /// </summary>
    /// <param name="htmlDocument"></param>
    /// <returns></returns>

    public SearchResultsScrapperResponse ProcessSearchResults(string htmlDocument)
    {
      var response = new SearchResultsScrapperResponse();

      if (string.IsNullOrEmpty(htmlDocument))
      {
        response.IsSuccess = false;
        response.ErrorMessages.Add("HTML string is null or empty. HTML must be provided");
        return response;
      }

      var searchResultItems = _regexProvider.GetSearchResults(htmlDocument);

      if (!searchResultItems.Any())
      {
        response.IsSuccess = false;
        response.ErrorMessages.Add("Main search results div could not be found");
        return response;
      }

      var results = new List<SearchResultModel>();

      var searchResultItemPosition = 1;

      foreach (var searchResultItem in searchResultItems)
      {
        // find anchor
        var anchor = _regexProvider.GetAnchor(searchResultItem);
        var titleMatch = _regexProvider.GetTitle(searchResultItem);

        var searchItemViewModel = new SearchResultModel
        {
          Name = titleMatch,
          Url = anchor,
          ResultItemPosition = searchResultItemPosition++
        };

        results.Add(searchItemViewModel);
      }

      response.Results = results;
      response.IsSuccess = true;
      return response;
    }
  }
}