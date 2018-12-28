using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace Html.Rt.Seperator
{

    internal class RefIndex
    {
        public int Index { get; set; }
    }
    public class ElementSeperator :IHtmlSeperator
    {
        private readonly  Regex _regex = new Regex(@"<([a-z][a-zA-Z0-90-9]+)\s*(.*)(\/)?>$");
        private readonly Regex _endRegex = new Regex(@"<\/([a-zA-Z0-9]+)\s*>$");


        public ElementSeperator()
        {
            
        }
        
        public bool CanParse(HtmlContent content)
        {
            return this.IsTagElement(content.Content) || this.IsEndTag(content.Content);
        }

        public ParseResult Parse(HtmlContent content)
        {
            var refIndex = new RefIndex();
            var r = GetResult(content, refIndex).ToArray();
            return new ParseResult(r, refIndex.Index);
        }

 

        private IEnumerable<IHtmlMarkup> GetResult(HtmlContent content,RefIndex refIndex)
        {
            return this.IsTagElement(content.Content) ? GetTagElement(content, refIndex) : GetEndTagElement(content,refIndex);
        }


        private IEnumerable<IHtmlMarkup> GetTagElement(HtmlContent content,RefIndex index)
        {
            var match = this._regex.Match(content.Content);
            var groups = match.Groups;
            index.Index = match.Index;

            yield return new HtmlElement(content.Content,groups[1].Value, new AttributeCollection(groups[2].Value), ImmutableArray<IHtmlMarkup>.Empty);
        }

        private IEnumerable<IHtmlMarkup> GetEndTagElement(HtmlContent content,RefIndex refIndex)
        {
            var match = this._endRegex.Match(content.Content);
            refIndex.Index = match.Index;
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