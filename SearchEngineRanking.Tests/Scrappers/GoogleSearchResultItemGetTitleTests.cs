using NUnit.Framework;
using SearchEngineRanking.Core.Services.Google;

namespace SearchEngineRanking.Tests.Scrappers
{
  public class GoogleSearchResultItemGetTitleTests
  {
    [Test]
    public void Test_Title_Is_Read()
    {
      /// AAA pattern
      var expectedResult = "Best Conveyancing Matter Management Software - Smokeball";      
      var htmlString = $"<div class=\"BNeawe vvjwJb AP7Wnd\">{expectedResult}</div>";

      // arrange
      var provider = new GoogleSearchResultScrapperRegexProvider();

      // act
      var result = provider.GetTitle(htmlString);
      System.Console.WriteLine(result);

      // assert      
      Assert.IsTrue(result == expectedResult);
    }
  }
}
