using SearchEngineRanking.Core.Messages.Requests;

namespace SearchEngineRanking.Core.Services;

public interface IRankingsFinder
{
  List<int> GetRankings(RankingFinderRequest request);
}