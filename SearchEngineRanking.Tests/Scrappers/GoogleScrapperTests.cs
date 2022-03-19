using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchEngineRanking.Core.Scrappers.Google;
using SearchEngineRanking.Core.Services;
using SearchEngineRanking.Core.Services.Google;

namespace SearchEngineRanking.Tests.Scrappers
{
  [TestFixture]
  public class GoogleScrapperTests
  {
    private GoogleSearchResultsScrapper _processor;

    [SetUp]
    public void Setup()
    {
      var regexProvider = new GoogleSearchResultScrapperRegexProvider();
      _processor = new GoogleSearchResultsScrapper(regexProvider);
    }

    [Test]
    public void Test_Blank_String_Returns_No_Results()
    {
      var htmlString = "";
      var response = _processor.ProcessSearchResults(htmlString);
      var errorMessage = string.Join(",",response.ErrorMessages);
      Console.WriteLine("Error Message " + errorMessage);
      Assert.IsFalse(response.IsSuccess);
    }

    [Test]
    public void Test_Invalid_Returns_No_Results()
    {
      var htmlString = "Im a weird string";
      var response = _processor.ProcessSearchResults(htmlString);
      Console.WriteLine(response.ErrorMessages[0]);
      Assert.IsTrue(response.IsSuccess == false);
    }
  }
}
