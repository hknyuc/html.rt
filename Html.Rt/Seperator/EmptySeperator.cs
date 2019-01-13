using System;

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
            return new ParseResult(Array.Empty<IHtmlMarkup>(), content.StartIndex);
        }
    }
}