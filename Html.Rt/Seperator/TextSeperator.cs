using System.Collections.Generic;

namespace Html.Rt.Seperator
{
    public class TextSeperator :IHtmlSeperator
    {
        public bool CanParse(HtmlContent content)
        {
            return true;
        }

        public IEnumerable<IHtmlMarkup> Parse(HtmlContent content)
        {
            yield return new Text(content.Content);
        }
    }
}