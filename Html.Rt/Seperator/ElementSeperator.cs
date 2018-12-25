using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Html.Rt.Seperator
{
    public class ElementSeperator :IHtmlSeperator
    {
        private readonly  Regex _regex = new Regex(@"^<([a-z][a-zA-Z0-90-9]+)\s*(.*)(\/)?>$");
        private readonly Regex _endRegex = new Regex(@"^<\/([a-zA-Z0-9]+)\s*>$");


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

            yield return new HtmlElement(content.Content,groups[1].Value, new AttributeCollection(groups[2].Value), ImmutableArray<IHtmlMarkup>.Empty);
        }

        private IEnumerable<IHtmlMarkup> GetEndTagElement(HtmlContent content)
        {
            var match = this._endRegex.Match(content.Content);
            if(match.Success)
               yield return new EndTag(content.Content,match.Groups[1].Value);
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