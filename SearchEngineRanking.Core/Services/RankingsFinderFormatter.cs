using SearchEngineRanking.Core.Messages.Requests;

namespace SearchEngineRanking.Core.Services;

public class RankingsFinderFormatter : IRankingFinderFormatter
{
  private IRankingsFinder _finder { get; set; } 

  public RankingsFinderFormatter()
  {
    _finder = new RankingsFinder();
  }

  public RankingsFinderFormatter(IRankingsFinder finder)
  {
    _finder = finder;
  }

  public string GetFormattedResults(RankingFinderRequest request)
  {
    var results = _finder.GetRankings(request);
    return this.FormatRankings(results);
  }

  public string FormatRankings(List<int> rankings)
  {
    if (!rankings.Any())
    {
      return "0";
    }

    return string.Join(",", rankings);
  }
}

public interface IRankingFinderFormatter
{
  string GetFormattedResults(RankingFinderRequest request);
  string FormatRankings(List<int> rankings);
}