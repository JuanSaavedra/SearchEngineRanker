using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using log4net;
using log4net.Config;
using SearchEngineRanking.Core.Scrappers;
using SearchEngineRanking.Core.Scrappers.Google;
using SearchEngineRanking.Core.Searchers.Google;
using SearchEngineRanking.Core.Services;
using SearchEngineRanking.Core.Services.Google;
using SearchEngineRanking.WPF.ViewModels;
using Unity;

namespace SearchEngineRanking.WPF
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    ILog logger = LogManager.GetLogger(typeof(App));

    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);

      XmlConfigurator.Configure();
      logger.Debug("Initialising ...");

      // setup IoC container
      var unityContainer = new UnityContainer();
      unityContainer.RegisterType<IGoogleSearcher, GoogleSearcher>();
      unityContainer.RegisterType<IMainWindow, MainWindow>();
      unityContainer.RegisterType<IGoogleSearchResultScrapperRegexProvider, GoogleSearchResultScrapperRegexProvider>();
      unityContainer.RegisterType<ISearchResultsScrapper, GoogleSearchResultsScrapper>();
      unityContainer.RegisterType<IRankingsFinder, RankingsFinder>();
      unityContainer.RegisterType<IRankingFinderFormatter, RankingsFinderFormatter>();
      unityContainer.Resolve<IMainWindow>().Show();
    }
  }
}
