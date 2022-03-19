using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using SearchEngineRanking.Core.Services;
using SearchEngineRanking.Core.Services.Google;

namespace SearchEngineRanking.Tests.Scrappers
{
  [TestFixture]
  public class GoogleRegexProviderTests
  {
    private string RawHTML { get; set; }
    private string InvalidRawHTML { get; set; }

    [SetUp]
    public void Setup()
    {
      RawHTML = @"

<div class=""ZINbbc luh4tb xpd O9g5cc uUPGi"">
  <div class=""egMi0 kCrYT"">
    <a href=""/url?q=https://www.smokeball.com.au"">
      <h3 class=""zBAuLc l97dzf"">
        <div class=""BNeawe vvjwJb AP7Wnd"">Best Conveyancing Matter Management Software - Smokeball</div>
      </h3>
      <div class=""BNeawe UPmit AP7Wnd"">www.smokeball.com.au</div>
    </a>
  </div>
  <div class=""kCrYT"">
    <div>
      <div class=""BNeawe s3v9rd AP7Wnd"">
        <div>
          <div>
            <div class=""BNeawe s3v9rd AP7Wnd"">Smokeball's cloud-based practice...</div>
			    </div>
        </div>
      </div>
    </div>
  </div>
</div>
";
      InvalidRawHTML = @"

<div class=""___ luh4tb xpd O9g5cc uUPGi"">
  <div class=""egMi0 kCrYT"">
    <a href=""/url?q=https://www.smokeball.com.au"">
      <h3 class=""zBAuLc l97dzf"">
        <div class=""____BNeawe vvjwJb AP7Wnd"">Best Conveyancing Matter Management Software - Smokeball</div>
      </h3>
      <div class=""___BNeawe UPmit AP7Wnd"">www.smokeball.com.au</div>
    </a>
  </div>
  <div class=""kCrYT"">
    <div>
      <div class=""BNeawe s3v9rd AP7Wnd"">
        <div>
          <div>
            <div class=""BNeawe s3v9rd AP7Wnd"">Smokeball's cloud-based practice...</div>
			    </div>
        </div>
      </div>
    </div>
  </div>
</div>
";

    }

    [Test]
    public void Test_That_Title_Is_SmokeBall()
    {
      var provider = new GoogleSearchResultScrapperRegexProvider();
      var title = provider.GetTitle(RawHTML);
      Assert.IsTrue(title == "Best Conveyancing Matter Management Software - Smokeball");      
    }

    [Test]
    public void Test_That_Invalid_HTML_Throws_Exception()
    {
      var htmlString = "<div class=\"BNeawe vvjwJb AP7Wnd\">I'm an not valid</h1></div>";
      var provider = new GoogleSearchResultScrapperRegexProvider();
      Assert.Throws<ApplicationException>(() => provider.GetTitle(htmlString));
    }


    [Test]
    public void Test_That_Link_Is_SmokeBall()
    {
      var provider = new GoogleSearchResultScrapperRegexProvider();
      var link = provider.GetAnchor(RawHTML);
      Assert.IsTrue(link == "https://www.smokeball.com.au", link);
    }


    [Test]
    public void Test_Search_Item_Not_Found()
    {
      var provider = new GoogleSearchResultScrapperRegexProvider();
      var link = provider.GetSearchResults(InvalidRawHTML);
      Assert.IsTrue(!link.Any());
    }

    [Test]
    public void Test_Title_Not_Found()
    {
      var provider = new GoogleSearchResultScrapperRegexProvider();
      var link = provider.GetTitle(InvalidRawHTML);
      Console.WriteLine(link);
      Assert.IsTrue(link == "");
    }
  }
}
