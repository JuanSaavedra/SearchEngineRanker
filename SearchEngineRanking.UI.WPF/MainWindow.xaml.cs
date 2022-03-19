using SearchEngineRanking.Core.Services;
using System.Windows;
using log4net;
using SearchEngineRanking.Core.Messages.Requests;
using SearchEngineRanking.Core.Messages.Responses;
using SearchEngineRanking.Core.Scrappers;
using SearchEngineRanking.Core.Searchers.Google;
using SearchEngineRanking.Core.Settings;
using SearchEngineRanking.WPF.ViewModels;

namespace SearchEngineRanking.WPF
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  ///
  public interface IMainWindow
  {
    void Show();
  }

  public partial class MainWindow : Window, IMainWindow
  {
    ILog _log = LogManager.GetLogger("MainWindow");

    // I know it could be null, but trust me Mr Compiler, it won't be!
    public SearchRankingsViewModel _searchRankingsViewModel { get; set; } = null!;
    
    private ISearchResultsScrapper _searchResultsScrapper { get; set; } = null!;
    private IGoogleSearcher _googleSearcher { get; set; } = null!;
    private IRankingsFinder _rankingsFinder { get; set; } = null!;
    private IRankingFinderFormatter _rankingFinderFormatter { get; set; } = null!;

    public MainWindow()
    {
      InitializeComponent();
    }

    public MainWindow(IGoogleSearcher googleSearcher, IRankingsFinder rankingsFinder, ISearchResultsScrapper searchResultsScrapper, IRankingFinderFormatter finderFormatter) : base()
    {
      _log.Debug(("Launching main window.."));
      this.Title = "Search Engine Ranking Utility";

      InitializeComponent();
      _log.Debug("Component Initialized..");

      _searchRankingsViewModel = new SearchRankingsViewModel();

      // IOC
      _googleSearcher = googleSearcher;
      _searchResultsScrapper = searchResultsScrapper;
      _rankingsFinder = rankingsFinder;
      _rankingFinderFormatter = finderFormatter;

      // setup the callbacks
      _googleSearcher.ProcessCompleted += Processor_ProcessCompleted;
      _googleSearcher.ProcessCompleted += Processor_Completed_SaveResultsToFile;
      _googleSearcher.ProcessError += Processor_ProcessError;

      DataContext = _searchRankingsViewModel;

      // ---- for debugging ------
      this._searchRankingsViewModel.CompanyURL = "https://www.smokeball.com.au";
      this._searchRankingsViewModel.KeyWords = "conveyancing software";
      _log.Debug("Dependencies in place...");

      // get the configuration settings
      var searchEngineClassName = SearchEngineRankingsApplicationSettings.SearchEngineResultsClassName;      
    }    

    private void GetSearchResultsButton_Click(object sender, RoutedEventArgs e)
    {
      DisableSearchButton();
      if (Valid())
      {
        this.HandleNewSearch();
      }
    }

    private bool Valid()
    {
      if (string.IsNullOrEmpty(this._searchRankingsViewModel.CompanyURL))
      {
        MessageBox.Show("Company URL cannot be null", "Error");
        EnableSearchButton();
        return false;
      }

      if (string.IsNullOrEmpty(this._searchRankingsViewModel.KeyWords))
      {
        MessageBox.Show("Keywords must be entered", "Error");
        EnableSearchButton();
        return false;
      }

      return true;
    }

    private async void HandleNewSearch()
    {
      _log.Debug("Starting downloading of data...");
      ShowLoadingCog();
      HideFeedbackMessage();
      await _googleSearcher.SearchGoogleAsync(this._searchRankingsViewModel.KeyWords);
    }

    private void Processor_ProcessError(string errorMessage)
    {
      HideLoadingCog();
      HideFeedbackMessage();
      DisableSearchButton();
      MessageBox.Show(errorMessage, "Error");
      _log.Error("There was a problem performing the search result " + errorMessage);
    }

    private void Processor_Completed_SaveResultsToFile(GoogleSearchResultResponse googleSearchResultResponse)
    {
      // save results to file for support if needed ..
      //File.WriteAllLines(@"C:\Temp\searchUtilityResults.html");
    }

    /// <summary>
    /// Once the Google search is completed, the callback is handled here
    /// </summary>
    /// <param name="googleSearchResultResponse">Google search result response</param>
    private void Processor_ProcessCompleted(GoogleSearchResultResponse googleSearchResultResponse)
    {
      _log.Debug("Raw HTML downloaded. Character count: " + googleSearchResultResponse.RawHTML.Length);
      
      // process search result
      var searchResultsScrapperResponse = _searchResultsScrapper.ProcessSearchResults(googleSearchResultResponse.RawHTML);
      
      // if successful we show results in list view
      if (searchResultsScrapperResponse.IsSuccess)
      {        
        _log.Debug("Search results parsed: " + searchResultsScrapperResponse.Results.Count + " records returned.");
        lvSearchResults.ItemsSource = searchResultsScrapperResponse.Results;
      }
      else
      {
        var errorMessage = string.Join(",", searchResultsScrapperResponse.ErrorMessages);
        _log.Debug("There was a problem processing the search results " + errorMessage );
      }

      // get ranking from the search result
      var rankingMessage = GetRanking(searchResultsScrapperResponse);

      HideLoadingCog();

      _log.Debug("Finding rankings...");
    
      ShowFeedbackMessage(rankingMessage);
      EnableSearchButton();

      _log.Debug("Rankings found: " + rankingMessage);
      _log.Debug("Done!");
      _log.Debug("---------------------------------");
    }

    private string GetRanking(SearchResultsScrapperResponse searchResultsScrapperResponse)
    {
      var rankings = _rankingsFinder.GetRankings(new RankingFinderRequest
      {
        CompanyURL = this._searchRankingsViewModel.CompanyURL, 
        SearchResultItems = searchResultsScrapperResponse.Results
      });

      var rankingStrings = _rankingFinderFormatter.FormatRankings(rankings);

      var rankingMessage = $"Ranking for {this._searchRankingsViewModel.CompanyURL} is: {rankingStrings}";
      return rankingMessage;
    }

    #region UI Fields

    private void ShowLoadingCog()
    {
      this.loadingCog.Visibility = Visibility.Visible;
    }

    private void HideLoadingCog()
    {
      this.loadingCog.Visibility = Visibility.Hidden;
    }

    private void ShowFeedbackMessage(string message)
    {
      FeedbackTB.Text = message;
      FeedbackTB.Visibility = Visibility.Visible;
      Check.Visibility = Visibility.Visible;
    }

    private void HideFeedbackMessage()
    {
      FeedbackTB.Text = string.Empty;
      FeedbackTB.Visibility = Visibility.Hidden;
      Check.Visibility = Visibility.Hidden;
    }

    private void EnableSearchButton()
    {
      SearchButton.IsEnabled = true;
    }

    private void DisableSearchButton()
    {
      SearchButton.IsEnabled = false;
    }
    
    #endregion


  }
}
