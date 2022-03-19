namespace SearchEngineRanking.Core.Settings
{
  public static class SearchEngineRankingsApplicationSettings
  {
    static SearchEngineRankingsApplicationSettings()
    {
      var settings = System.Configuration.ConfigurationManager.AppSettings;

      var classNameSettings = settings["SearchResults.Class"];

      SearchEngineResultsClassName = classNameSettings ?? throw new ApplicationException("SearchResults.Class settings does not exist in App.Config. Please add it");
    }

    public static string SearchEngineResultsClassName { get; set; }
  }
}
