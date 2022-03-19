using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SearchEngineRanking.Core.Messages.Requests;
using SearchEngineRanking.Core.Models;
using SearchEngineRanking.Core.Services;

namespace SearchEngineRanking.Tests.RankingFinder
{
  [TestFixture]
  public class RankingFinderTests
  {
    [Test]
    public void Test_That_No_Company_Passed_Throws_Exception()
    {
      var rankingFinder = new RankingsFinder();
      var rankingFinderRequest = new RankingFinderRequest();

      var model = new SearchResultModel();
      model.Name = "Acme";
      model.Url = "www.acme.com.au";
      model.ResultItemPosition = 1;

      rankingFinderRequest.SearchResultItems = new List<SearchResultModel> { model };
      Assert.Throws<ArgumentNullException>(() => { rankingFinder.GetRankings(rankingFinderRequest);});
    }

    [Test]
    public void Test_Ranking_Is_Found_Position_1()
    {
      var rankingFinder = new RankingsFinder();
      var rankingFinderRequest = new RankingFinderRequest();

      var model = new SearchResultModel();
      model.Name = "Acme";
      model.Url = "www.acme.com.au";
      model.ResultItemPosition = 1;

      rankingFinderRequest.SearchResultItems = new List<SearchResultModel> { model };
      rankingFinderRequest.CompanyURL = model.Url;
      var rankings = rankingFinder.GetRankings(rankingFinderRequest);
      Assert.IsTrue(rankings.Count == 1);
    }

    [Test]
    public void Test_Ranking_Is_Not_Found()
    {
      var rankingFinder = new RankingsFinder();
      var rankingFinderRequest = new RankingFinderRequest();
      rankingFinderRequest.CompanyURL = "https://www.fooEnterprises.com.au";

      for (int i = 1; i <= 100; i++)
      {
        var model = new SearchResultModel();
        model.Name = "Acme " + i;
        model.Url = "https://www.acme " + i + ".com.au";
        model.ResultItemPosition = i;

        rankingFinderRequest.SearchResultItems.Add(model);
      }
      
      var rankings = rankingFinder.GetRankings(rankingFinderRequest);
      Assert.IsTrue(rankings.Count() == 1);
      Assert.IsTrue(rankings.Single() == 0);
    }

    [Test]
    public void Test_Ranking_Is_Found_Multiple_Places()
    {
      var rankingFinder = new RankingsFinder();
      var rankingFinderRequest = new RankingFinderRequest();
      rankingFinderRequest.CompanyURL = "https://www.acme.com.au";

      for (var i = 1; i <= 5; i++)
      {
        var model = new SearchResultModel
        {
          Name = "Acme " + i,
          Url = rankingFinderRequest.CompanyURL,
          ResultItemPosition = i
        };

        rankingFinderRequest.SearchResultItems.Add(model);
      }

      var rankingString = rankingFinder.GetRankings(rankingFinderRequest);
      Console.WriteLine(rankingString);
      Assert.IsTrue(rankingString.Count() == 5);
    }

    [Test]
    public void Test_Ranking_Is_Found_Position_100()
    {
      var rankingFinder = new RankingsFinder();
      var rankingFinderRequest = new RankingFinderRequest();
      rankingFinderRequest.CompanyURL = "https://www.acme100.com.au";

      for (var i = 1; i <= 100; i++)
      {
        var model = new SearchResultModel
        {
          Name = "Acme " + i,
          Url = "https://www.acme" + i + ".com.au",
          ResultItemPosition = i
        };

        rankingFinderRequest.SearchResultItems.Add(model);
      }

      var rankingString = rankingFinder.GetRankings(rankingFinderRequest);
      Console.WriteLine(rankingString.Single());
      Assert.IsTrue(rankingString.Count() == 1);
      Assert.IsTrue(rankingString.Single() == 100);
    }
  }
}
