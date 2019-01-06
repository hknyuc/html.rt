using System.Xml;
using System.Xml.XPath;
using Html.Rt.Seperator;

/*
namespace Html.Rt
{
    
    public class HtmlXPathNavigator :XPathNavigator
    {
        public override string Value { get; }
        
   
        public override string BaseURI { get; }
        public override bool IsEmptyElement { get; }
        public override string LocalName { get; }
        public override string Name { get; }
        public override string NamespaceURI { get; }
        public override XmlNameTable NameTable { get; }
        public override XPathNodeType NodeType { get; } = XPathNodeType.All;
        public override string Prefix { get; }

        private IHtmlContent _htmlContent;
        public HtmlXPathNavigator(IHtmlContent content)
        {
            this._htmlContent = content;
        }

        public override bool IsSamePosition(XPathNavigator other)
        {
            if (other is HtmlXPathNavigator navigator)
                return this._htmlContent.Index == navigator._htmlContent.Index;

            return false;
        }
        
        public override XPathNavigator Clone()
        {
            return new HtmlXPathNavigator((IHtmlContent) this._htmlContent.Clone());
        }


        public override bool MoveTo(XPathNavigator other)
        {
            if (!this.IsSamePosition(other)) return false;
            this._htmlContent.NextTo(((HtmlXPathNavigator) other)._htmlContent.Index);
        }

        public override bool MoveToFirstAttribute()
        {
            throw new System.NotImplementedException();
        }

        public override bool MoveToFirstChild()
        {
            throw new System.NotImplementedException();
        }

        public override bool MoveToFirstNamespace(XPathNamespaceScope namespaceScope)
        {
            throw new System.NotImplementedException();
        }

        public override bool MoveToId(string id)
        {
            throw new System.NotImplementedException();
        }

        public override bool MoveToNext()
        {
            throw new System.NotImplementedException();
        }

        public override bool MoveToNextAttribute()
        {
            throw new System.NotImplementedException();
        }

        public override bool MoveToNextNamespace(XPathNamespaceScope namespaceScope)
        {
            throw new System.NotImplementedException();
        }

        public override bool MoveToParent()
        {
            throw new System.NotImplementedException();
        }

        public override bool MoveToPrevious()
        {
            throw new System.NotImplementedException();
        }
    }
}
*/