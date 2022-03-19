using SearchEngineRanking.Core.Models;

namespace SearchEngineRanking.Core.Messages.Requests
{
  public class RankingFinderRequest
  {
    public List<SearchResultModel> SearchResultItems { get; set; }
    public string CompanyURL { get; set; } = string.Empty;

    public RankingFinderRequest()
    {
      SearchResultItems = new List<SearchResultModel>();
    }
  }
}
