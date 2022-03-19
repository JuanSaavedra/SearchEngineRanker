namespace SearchEngineRanking.Core.Services.Google;

public interface IGoogleSearchResultScrapperRegexProvider
{
  IEnumerable<string> GetSearchResults(string rawHtml);
  string GetTitle(string rawHtml);
  string GetAnchor(string rawHtml);
}