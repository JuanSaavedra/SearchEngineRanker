using SearchEngineRanking.Core.Messages.Requests;

namespace SearchEngineRanking.Core.Services
{
  public class RankingsFinder : IRankingsFinder
  {
    public List<int> GetRankings(RankingFinderRequest request)
    {
      if (string.IsNullOrEmpty(request.CompanyURL))
      {
        throw new ArgumentNullException(nameof(request.CompanyURL));
      }

      // if no results we return a "0"
      if (!request.SearchResultItems.Any())
      {
        return new List<int>{0};
      }

      var rankings = new List<int>();

      foreach (var item in request.SearchResultItems)
      {
        if (item.Url.Contains(request.CompanyURL, StringComparison.CurrentCultureIgnoreCase))
        {
          rankings.Add(item.ResultItemPosition);
        }
      }

      // if the rankings not found.. we return a "0" ..
      if (!rankings.Any())
      {
        rankings.Add(0);
      }

      return rankings;
    }
  }
}
