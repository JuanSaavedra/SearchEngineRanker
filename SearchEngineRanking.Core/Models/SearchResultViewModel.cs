namespace SearchEngineRanking.Core.Models;

public class SearchResultModel
{
  public string Name { get; set; } = null!;
  public int ResultItemPosition { get; set; }
  public string Url { get; set; } = null!;
}