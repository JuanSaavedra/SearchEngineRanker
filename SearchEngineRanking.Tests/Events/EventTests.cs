using System;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using SearchEngineRanking.Core.Delegates;
using SearchEngineRanking.Core.Messages.Responses;
using SearchEngineRanking.Core.Scrappers.Google;
using SearchEngineRanking.Core.Searchers.Google;
using SearchEngineRanking.Core.Services.Google;

namespace SearchEngineRanking.Tests.Events;

[TestFixture]
public class EventTests
{
  [Test]
  public Task TestEventIsRaised()
  {
    var _searcher = Substitute.For<IGoogleSearcher>();
    var eventWasRaised = false;
    var rawHtml = "";

    var processor = new GoogleSearchResultsScrapper(new GoogleSearchResultScrapperRegexProvider());

    // handle the event
    _searcher.ProcessCompleted += delegate(GoogleSearchResultResponse response)
    {
      var results = processor.ProcessSearchResults(response.RawHTML);
      rawHtml = response.RawHTML;
      Console.WriteLine("Google search result received");
      Console.WriteLine("results: " + results.Results.Count.ToString());
      eventWasRaised = true;
    };

    // raise the event on the sub
    _searcher.ProcessCompleted += Raise.Event<SearchResultsLoadedCallback>( new GoogleSearchResultResponse{ RawHTML = "hello"});
    Assert.That(eventWasRaised);
    Assert.That(rawHtml == "hello");
    return Task.CompletedTask;
  }
}