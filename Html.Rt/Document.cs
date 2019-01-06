using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.XPath;
using Html.Rt.Seperator;
using System.Linq;
namespace Html.Rt
{
    public class Document :IEnumerable<IHtmlMarkup>
    {

        private string _content;
        public Document(string content)
        {
            this._content = content;
        }
        

        IEnumerator<IHtmlMarkup> IEnumerable<IHtmlMarkup>.GetEnumerator()
        {
            return this.GetEnumerable();
        }

        private IEnumerator<IHtmlMarkup> GetEnumerable()
        {
            return new StandartHtmlSeperator().Parse(new HtmlContent(this._content)).Result.GetEnumerator();
        }


        public IEnumerator GetEnumerator()
        {
            return this.GetEnumerable();
        }
    }
}