using SearchEngineRanking.Core.Models;

namespace SearchEngineRanking.Core.Messages.Responses
{
  public class SearchResultsScrapperResponse
  {
    public List<SearchResultModel> Results { get; set; } = new List<SearchResultModel>();
    public bool IsSuccess { get; set; } = false; 
    public List<string> ErrorMessages { get; set;} = new List<string>();
  }
}
