using System;
using System.Collections;
using System.Collections.Generic;
using Html.Rt.Seperator;
namespace Html.Rt
{
    public class Document :IEnumerable<IHtmlMarkup>
    {

        private readonly string _content;
        private readonly IHtmlContent _htmlContent;
        public Document(string content)
        {
            this._content = content;
        }

        public Document(IHtmlContent content)
        {
            this._htmlContent = content;
        }


        IEnumerator<IHtmlMarkup> IEnumerable<IHtmlMarkup>.GetEnumerator()
        {
            return this.GetEnumerable();
        }

        private IEnumerator<IHtmlMarkup> GetEnumerable()
        {
            return new StandartHtmlSeperator().Parse(this.GetHtmlContent()).Result.GetEnumerator();
        }

        private IHtmlContent GetHtmlContent()
        {
            if (this._htmlContent != null) return (IHtmlContent) this._htmlContent.Clone();
            return new HtmlContent(this._content);
        }


        public IEnumerator GetEnumerator()
        {
            return this.GetEnumerable();
        }
    }
}