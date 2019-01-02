using System.Collections.Generic;
using Html.Rt.Seperator;

namespace Html.Rt
{
    public static class RtUtility
    {
        public static bool CanParse(this IHtmlSeperator seperator, string content)
        {
            return seperator.CanParse(new HtmlContent(content).JumpLast());
        }

        public static bool CanParse(this IHtmlSeperator seperator,HtmlContent content)
        {
            return seperator.Parse(content).IsSuccess;
        }

        public static IEnumerable<IHtmlMarkup> Parse(this IHtmlSeperator seperator,string content)
        {
            return seperator.Parse(new HtmlContent(content).JumpLast());
        }

        public static IEnumerable<IHtmlMarkup> ParseFromOrigin(this IHtmlSeperator seperator, string content)
        {
            return seperator.Parse(new HtmlContent(content));
        }
        
       
    }
}