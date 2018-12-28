using System.Collections.Generic;

namespace Html.Rt.Seperator
{
    public class TextSeperator :IHtmlSeperator
    {
        public bool CanParse(HtmlContent content)
        {
            return true;
        }

        public ParseResult Parse(HtmlContent content)
        {
            return new ParseResult(GetResult(content), 0);
        }

        private static IEnumerable<IHtmlMarkup> GetResult(HtmlContent content)
        {
            yield return new Text(content.Content);
        }
    }
}