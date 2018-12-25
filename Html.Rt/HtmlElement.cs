using System.Collections;
using System.Collections.Generic;

namespace Html.Rt
{
    public class HtmlElement : IHtmlElement
    {
        public string Name { get; }
        
        public string Markup { get; }
        
        public IEnumerable<IAttribute> Attributes { get; }

        private readonly IEnumerable<IHtmlMarkup> _nodes;

        public HtmlElement(string markup,string name,IEnumerable<IAttribute> attributes,IEnumerable<IHtmlMarkup> nodes)
        {
            this.Name = name;
            this.Attributes = attributes;
            this._nodes = nodes;
            this.Markup = markup;
        }

        public HtmlElement(string name)
        {
            this.Name = name;
            this.Attributes = new IAttribute[0];
            this._nodes = new IHtmlMarkup[0];
        }

        public IEnumerator<IHtmlMarkup> GetEnumerator()
        {
            return this._nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


    }
}