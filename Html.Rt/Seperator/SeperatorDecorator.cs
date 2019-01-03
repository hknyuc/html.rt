using System;

namespace Html.Rt.Seperator
{
    public class SeperatorDecorator :IHtmlSeperator
    {

        private IHtmlSeperator _seperator;
        public SeperatorDecorator(IHtmlSeperator seperator)
        {
            this._seperator = seperator;
        }
        public virtual ParseResult Parse(IHtmlContent content)
        {
            return this._seperator.Parse(content);
        }
    }
}