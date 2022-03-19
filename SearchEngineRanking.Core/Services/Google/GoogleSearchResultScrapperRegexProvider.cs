using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using log4net;

namespace SearchEngineRanking.Core.Services.Google;

public class GoogleSearchResultScrapperRegexProvider : IGoogleSearchResultScrapperRegexProvider
{
  // TODO: Next version pass through configuration item
  public string MainDivClassName = "ZINbbc luh4tb xpd O9g5cc uUPGi";
  public string TitleClassName = "BNeawe vvjwJb AP7Wnd";

  public GoogleSearchResultScrapperRegexProvider()
  {
    
  }

  private readonly ILog _logger = LogManager.GetLogger("GoogleSearchResultScrapperRegexProvider");

  private readonly string MainDivRegEx = @"<div class=""ZINbbc luh4tb xpd O9g5cc uUPGi"">(.*?)</a>";
  private readonly string TitleRegEx = @"<div class=""BNeawe vvjwJb AP7Wnd"">(.*?)</div>";
  private readonly string AnchorRegEx = "<a.*? href=\"/url\\?q=(?<value>.*?)\".*?>";
 
  public IEnumerable<string> GetSearchResults(string rawHtml)
  {
    var regex = Regex.Matches(rawHtml, MainDivRegEx, RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.Singleline).Select(a=>a.ToString());
    return regex;
  }

  public string GetTitle(string rawHtml)
  {
    var result = Regex.Match(rawHtml, this.TitleRegEx, RegexOptions.Compiled | RegexOptions.Singleline);

    if (result.Success)
    {
      try
      {
        XElement titleElement = XElement.Parse(result.Value);
        return titleElement.Value;
      }
      catch (XmlException ex)
      {
        const string message = "XML Exception Error: when getting the Title for the search result  from raw string";
        _logger.Error(message + ". HTML String: " + rawHtml, ex);
        throw new ApplicationException(message, ex);
      }      
    }

    return string.Empty;
  }

  public string GetAnchor(string rawHtml)
  {
    try
    {
      var expression = new Regex(AnchorRegEx, RegexOptions.IgnoreCase);
      var result = expression.Match(rawHtml);

      if (result.Success)
      {
        var link = result.Groups["value"].Value;
        return SanitiseAnchor(link);
      }

      return string.Empty;
    }
    catch (Exception ex)
    {
      _logger.Error("Exception: Could not get the organisation link from raw HTML string " + rawHtml);
      throw new ApplicationException("Could not get the organisation link from raw HTML string.", ex);
    }
  }

  private string SanitiseAnchor(string link)
  {
    try
    {
      if (link.Contains("&amp;"))
      {
        var indexOf = link.IndexOf("&amp;", StringComparison.CurrentCulture);
        if (indexOf > -1)
        {
          return link.Substring(0, indexOf);
        }
      }

      return link;
    }
    catch (Exception ex)
    { 
      _logger.Error("could not sanitise anchor link from link " + link, ex);
      return link;
    }
  }
}