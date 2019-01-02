using System.Collections;
using System.Collections.Generic;

namespace Html.Rt.Seperator
{
    public interface IHtmlSeperator
    {
        ParseResult Parse(HtmlContent content);
    }

    public class ParseResult :IEnumerable<IHtmlMarkup>
    {
        public IEnumerable<IHtmlMarkup> Result { get; }
        
        public int From { get; }
        
        public bool IsSuccess { get; }

        public ParseResult(IEnumerable<IHtmlMarkup> result,int from)
        {
            this.Result = result;
            this.From = from;
            this.IsSuccess = true;
        }

        public ParseResult(bool isSuccess,int from)
        {
            this.IsSuccess = isSuccess;
            this.From = from;
        }

        public ParseResult()
        {
            this.IsSuccess = false;
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