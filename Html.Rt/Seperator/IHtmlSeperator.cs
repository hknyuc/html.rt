using System.Collections;
using System.Collections.Generic;

namespace Html.Rt.Seperator
{
    public interface IHtmlSeperator
    {
        bool CanParse(HtmlContent content);
        ParseResult Parse(HtmlContent content);
    }

    public class ParseResult :IEnumerable<IHtmlMarkup>
    {
        public IEnumerable<IHtmlMarkup> Result { get; }
        
        public int From { get; }

        public ParseResult(IEnumerable<IHtmlMarkup> result,int from)
        {
            this.Result = result;
            this.From = from;
        }

        public IEnumerator<IHtmlMarkup> GetEnumerator()
        {
            return this.Result.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}