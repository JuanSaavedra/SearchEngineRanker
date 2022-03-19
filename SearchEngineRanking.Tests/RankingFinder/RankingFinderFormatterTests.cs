using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using SearchEngineRanking.Core.Messages.Requests;
using SearchEngineRanking.Core.Services;

namespace SearchEngineRanking.Tests.RankingFinder
{
  [TestFixture]
  public class RankingFinderFormatterTests
  {
    [Test]
    public void Test_Ranking_Is_Found_Position_1()
    {
      var rankingFinder = Substitute.For<IRankingsFinder>();

      // setup
      var request = new RankingFinderRequest();
      rankingFinder.GetRankings(request).Returns(new List<int> { 1 });

      var formatter = new RankingsFinderFormatter(rankingFinder);
      Assert.IsTrue(formatter.GetFormattedResults(request) == "1");
    }

    [Test]
    public void Test_Ranking_Is_Not_Found()
    {
      var rankingFinder = Substitute.For<IRankingsFinder>();

      // setup
      var request = new RankingFinderRequest();
      rankingFinder.GetRankings(request).Returns(new List<int>());

      var formatter = new RankingsFinderFormatter(rankingFinder);
      var result = formatter.GetFormattedResults(request);
      Console.WriteLine(result);
      Assert.IsTrue( result == "0");
    }

    [Test]
    public void Test_Ranking_Is_Not_Found_2()
    {
      var rankingFinder = Substitute.For<IRankingsFinder>();

      // setup
      var request = new RankingFinderRequest();
      rankingFinder.GetRankings(request).Returns(new List<int>{0});

      var formatter = new RankingsFinderFormatter(rankingFinder);
      var result = formatter.GetFormattedResults(request);
      Console.WriteLine(result);
      Assert.IsTrue(result == "0");
    }

    [Test]
    public void Test_Ranking_Is_Found_Multiple_Places()
    {
      var rankingFinder = Substitute.For<IRankingsFinder>();

      // setup
      var request = new RankingFinderRequest();
      rankingFinder.GetRankings(request).Returns(new List<int> { 1, 2, 3, 4, 5 });

      var formatter = new RankingsFinderFormatter(rankingFinder);
      var result = formatter.GetFormattedResults(request);
      Console.WriteLine(result);
      Assert.IsTrue(result == "1,2,3,4,5");
    }

    [Test]
    public void Test_Ranking_Is_Found_Position_100()
    {
      var rankingFinder = Substitute.For<IRankingsFinder>();

      // setup
      var request = new RankingFinderRequest();
      rankingFinder.GetRankings(request).Returns(new List<int> { 100 });

      var formatter = new RankingsFinderFormatter(rankingFinder);
      var result = formatter.GetFormattedResults(request);
      Console.WriteLine(result);
      Assert.IsTrue(result == "100");
    }
  }
}
