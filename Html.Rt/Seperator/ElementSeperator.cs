using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Html.Rt.Seperator
{
    public class ElementSeperator :IHtmlSeperator
    {
        private readonly  Regex _regex = new Regex(@"^<([a-z][a-zA-Z0-90-9]+)\s*(.*)(\/)?>$");
        private readonly Regex _endRegex = new Regex(@"^<\/([a-zA-Z0-9]+)\s*>$");
        private IHtmlSeperator _contextSeperator = new EmptySeperator();

        public ElementSeperator(IHtmlSeperator seperator)
        {
            this._contextSeperator = seperator;
        }

        public ElementSeperator()
        {
            
        }
        
        public bool CanParse(HtmlContent content)
        {
            return this.IsTagElement(content.Content) || this.IsEndTag(content.Content);
        }

        public IEnumerable<IHtmlMarkup> Parse(HtmlContent content)
        {
            if (this.IsTagElement(content.Content)) return GetTagElement(content);
            return GetEndTagElement(content);
        }


        private IEnumerable<IHtmlMarkup> GetTagElement(HtmlContent content)
        {
            var match = this._regex.Match(content.Content);
            var groups = match.Groups;
            IEnumerable<IHtmlMarkup> childNodes;
            if (groups[2].Success)
            {
                childNodes = new IHtmlMarkup[0];
            }
            else
            {
                var cloneHtmlContent = ((HtmlContent) content.Clone());
                cloneHtmlContent.Next();
                childNodes = this._contextSeperator.Parse(cloneHtmlContent);
            }
            yield return new HtmlElement(groups[0].Name,new AttributeCollection(groups[1].Value),childNodes);
        }

        private IEnumerable<IHtmlMarkup> GetEndTagElement(HtmlContent content)
        {
            var match = this._regex.Match(content.Content);
            yield return new HtmlElement(match.Groups[0].Value);
        }


        private bool IsTagElement(string content)
        {
            return this._regex.IsMatch(content);
        }

        private bool IsEndTag(string content)
        {
            return this._endRegex.IsMatch(content);
        }
    }
}