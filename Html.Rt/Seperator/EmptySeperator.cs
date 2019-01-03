using System.Collections.Generic;
using System.Collections.Immutable;

namespace Html.Rt.Seperator
{
    public class EmptySeperator:IHtmlSeperator
    {
        public bool CanParse(IHtmlContent content)
        {
            return true;
        }

        public  ParseResult Parse(IHtmlContent content)
        {
            return new ParseResult(ImmutableArray<IHtmlMarkup>.Empty, content.StartIndex);
        }
    }
}