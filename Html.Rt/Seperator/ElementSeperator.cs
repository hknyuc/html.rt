using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Html.Rt.Seperator
{

    internal class RefIndex
    {
        public int Index { get; set; }
    }
  /*  
    public class ElementMSeperator :IHtmlSeperator
    {
  
        /// <summary>
        /// <([a-zA-Z][a-zA-Z0-9]+)(\s+((([^=^'^"^\s^>^<]\s*=\s*(("|').*\7))|([^=^'^"^\s^>^<]))\s*)+)?>
        /// </summary>
        private readonly  Regex _regex = new Regex("<([a-zA-Z][a-zA-Z0-9]+)(\\s+((([^=^'^\"^\\s^>^<]\\s*=\\s*((\"|').*\\7))|([^=^'^\"^\\s^>^<]))\\s*)+)?>$");
        private readonly Regex _endRegex = new Regex(@"<\/([a-zA-Z0-9]+)\s*>$");


  
        
        private bool CanParse(IHtmlContent content)
        {
            return this.IsTagElement(content.Content) || this.IsEndTag(content.Content);
        }


        public ParseResult Parse(IHtmlContent content)
        {
            var result = this.CanParse(content);
            if (!result) return new ParseResult();
            var refIndex = new RefIndex();
            var r = GetResult(content, refIndex).ToArray();
            return new ParseResult(r, refIndex.Index);
        }


        private IEnumerable<IHtmlMarkup> GetResult(IHtmlContent content,RefIndex refIndex)
        {
            if (this.IsTagElement(content.Content)) return GetTagElement(content, refIndex);
            return GetEndTagElement(content, refIndex);
        }


        private IEnumerable<IHtmlMarkup> GetTagElement(IHtmlContent content,RefIndex index)
        {
            var match = this._regex.Match(content.Content);
            var groups = match.Groups;
            index.Index = match.Index;

            yield return new HtmlElement(content.Content,groups[1].Value, new AttributeCollection(groups[2].Value), ImmutableArray<IHtmlMarkup>.Empty);
        }

        private IEnumerable<IHtmlMarkup> GetEndTagElement(IHtmlContent content,RefIndex refIndex)
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
*/
    public class ElementSeperator : IHtmlSeperator
    {
        private readonly Regex _endRegex = new Regex(@"<\/([a-zA-Z0-9]+)\s*>$");
        private readonly Regex _startRegex = new Regex("<([a-zA-Z][a-zA-Z0-9]+)\\s*(\\s+|\\/>|>)");

       
        
        private bool IsEndTag(string content)
        {
            return this._endRegex.IsMatch(content);
        }

        private bool IsTagElement(string content)
        {
            return this._startRegex.IsMatch(content);
        }


        private IEnumerable<IHtmlMarkup> GetEndTagElement(IHtmlContent content,RefIndex refIndex)
        {
            var match = this._endRegex.Match(content.Content);
            refIndex.Index = match.Index;
            yield return new EndTag(content.Content, match.Groups[1].Value);
        }

        private IEnumerable<IHtmlMarkup> GetTagElement(IHtmlContent content, RefIndex refIndex)
        {
            var matchResult = this._startRegex.Match(content.Content);
            var name = matchResult.Groups[1].Value;
            refIndex.Index =  matchResult.Index;
            if (content.CurrentChar == '>')
            {
                yield return new HtmlElement(content.Content,name);
                yield break;

            }   
            
            var beginPosition = refIndex.Index + (matchResult.Length) +content.From ; // {from}this is test code <div{beginPosition} name='4'>{index}
            IHtmlContent currentContent = content;
            if (beginPosition < content.Index)
                currentContent =
                    new HtmlContent(content.RootContent.Substring(beginPosition, content.Index - beginPosition));
                   
            yield return new HtmlElement(content.Content,name,new AttributeCollection(GetAttributes(currentContent)).ToArray(), ImmutableArray<IHtmlMarkup>.Empty);
        }

  
        private static string GetAttributes(IHtmlContent content)
        {
            var escapeHtml = new EscapeStringHtmlContent(content);
            var beginPosition = content.Index;
            var lastPosition = beginPosition;
            while (escapeHtml.Next())
            {
                if (content.CurrentChar == '>')
                {
                    lastPosition = content.Index;
                    break;
                }
                
            }

            var isCatched = beginPosition != lastPosition;

            if (!isCatched)
            {
                return content.Content.Substring(beginPosition, content.Content.Length - beginPosition);
            }


            var result = content.RootContent.Substring(beginPosition, lastPosition-beginPosition);
            return result;
        }
        
       

        public ParseResult Parse(IHtmlContent content)
        {
            var isEndTag = this.IsEndTag(content.Content);
            var refIndex = new RefIndex();
            if (isEndTag) return new ParseResult(this.GetEndTagElement(content, refIndex).ToArray(), refIndex.Index);
            var isTagElement = this.IsTagElement(content.Content);
            if (isTagElement) return new ParseResult(this.GetTagElement(content, refIndex).ToArray(), refIndex.Index);
            return new ParseResult();
        }
    }
    
}