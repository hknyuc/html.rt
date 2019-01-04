using System.Collections.Generic;

namespace Html.Rt.Seperator
{
    public class SeperatorIterator : IHtmlSeperator
    {
        private IHtmlSeperator _seperator;
        public SeperatorIterator(IHtmlSeperator seperator)
        {
            this._seperator = seperator;
        }
        public ParseResult Parse(IHtmlContent content)
        {
            while (content.Next())
            {
                var result = this._seperator.Parse(content);
                if (result.IsSuccess)
                {
                    content.Reset();
                    return result;
                }
            }
            content.Reset();
            return new ParseResult();
        }

    }
}