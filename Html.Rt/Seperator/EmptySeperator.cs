using System.Collections.Generic;
using System.Collections.Immutable;

namespace Html.Rt.Seperator
{
    public class EmptySeperator:IHtmlSeperator
    {
        public bool CanParse(HtmlContent content)
        {
            return true;
        }

        public  ParseResult Parse(HtmlContent content)
        {
            return new ParseResult(ImmutableArray<IHtmlMarkup>.Empty, content.StartIndex);
        }
    }
}