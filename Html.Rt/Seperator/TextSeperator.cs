using System.Collections.Generic;

namespace Html.Rt.Seperator
{
    public class TextSeperator :IHtmlSeperator
    {
        public bool CanParse(IHtmlContent content)
        {
            return true;
        }

        public ParseResult Parse(IHtmlContent content)
        {
            return new ParseResult(GetResult(content), 0);
        }

        private static IEnumerable<IHtmlMarkup> GetResult(IHtmlContent content)
        {
            yield return new Text(content.Content);
        }
    }
}