using System;
using System.IO;
using NUnit.Framework;
using SearchEngineRanking.Core.Messages.Requests;
using SearchEngineRanking.Core.Scrappers.Google;
using SearchEngineRanking.Core.Services;
using SearchEngineRanking.Core.Services.Google;

namespace SearchEngineRanking.Tests.Integration;

[TestFixture]
public class IntegrationTests
{
  public string RawHTML { get; set; }

  [OneTimeSetUp]
  public void Init()
  {
    RawHTML = File.ReadAllText(@"Htmls/SearchResults.html");
  }

  [Test]
  public void Test_That_There_Are_100_Search_Items()
  {
    var parser = new GoogleSearchResultsScrapper(new GoogleSearchResultScrapperRegexProvider());
    var searchResults = parser.ProcessSearchResults(RawHTML);
    
    Assert.IsTrue(searchResults.Results.Count == 100);
  }

  [Test]
  public void Test_Smoke_Ball_Comes_First()
  {
    var parser = new GoogleSearchResultsScrapper(new GoogleSearchResultScrapperRegexProvider());
    var searchResults = parser.ProcessSearchResults(RawHTML);

    var rankingsFinder = new RankingsFinder();
    var rankings = rankingsFinder.GetRankings(new RankingFinderRequest { CompanyURL = "WWW.smokeball.com.au", SearchResultItems = searchResults.Results });

    var formatter = new RankingsFinderFormatter();
    var rankingsString = formatter.FormatRankings(rankings);
    Console.WriteLine(rankingsString);
    Assert.IsTrue(rankingsString == "1");
  }

  [Test]
  public void Test_page_Light_prime_comes_last()
  {
    var parser = new GoogleSearchResultsScrapper(new GoogleSearchResultScrapperRegexProvider());
    var searchResults = parser.ProcessSearchResults(RawHTML);

    var rankingsFinder = new RankingsFinder();
    var rankings = rankingsFinder.GetRankings(new RankingFinderRequest { CompanyURL = "WWW.pagelightprime.com", SearchResultItems = searchResults.Results });

    var formatter = new RankingsFinderFormatter();
    var rankingsString = formatter.FormatRankings(rankings);
    Console.WriteLine(rankingsString);
    Assert.IsTrue(rankingsString == "100");
  }
}