using System.Collections.Generic;

namespace Html.Rt.Seperator
{
    public class EmptySeperator:IHtmlSeperator
    {
        public bool CanParse(HtmlContent content)
        {
            return true;
        }

        public IEnumerable<IHtmlMarkup> Parse(HtmlContent content)
        {
            yield break;
        }
    }
}