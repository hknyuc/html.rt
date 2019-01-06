using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Html.Rt.Exceptions;

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
        private readonly Regex _endRegex = new Regex(@"<\/([a-zA-Z][a-zA-Z0-9]*)\s*>$");
        private readonly Regex _startRegex = new Regex("<([a-zA-Z][a-zA-Z0-9]*)\\s*(\\s+|\\/>|>)");

        private IHtmlSeperator _mainSeperator;
        public ElementSeperator(IHtmlSeperator seperator)
        {
            this._mainSeperator = seperator;
        }

        public ElementSeperator()
        {
            this._mainSeperator = new EmptySeperator();
        }
        
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

        private bool NoChildElements(string tag)
        {
            return new string[] {"br", "hr"}.Contains(tag);
        }

        private IEnumerable<IHtmlMarkup> GetElements(string tag,IHtmlContent content)
        {
            if(NoChildElements(tag)) yield break;
            content.Outstrip();
            var result = this._mainSeperator.Parse(content);
            var count = 1;
            foreach (var element in result)
            {
                if (element is ITag elementTag)
                {
                    if (elementTag.Is(tag))
                        count++;

                    yield return elementTag;
                    continue;
                }

                if (element is EndTag endTag)
                {
                    if (endTag.Is(tag))
                        count--;
                    if(count == 0) yield break;
                    yield return endTag;
                    continue;
                }
                yield return element;
            }
        }

        private static IHtmlContent CloneContent(IHtmlContent content)
        {
            return (IHtmlContent) content.Clone();
        }

        private IEnumerable<IHtmlMarkup> GetTagElement(IHtmlContent content, RefIndex refIndex)
        {
            var matchResult = this._startRegex.Match(content.Content);
            var name = matchResult.Groups[1].Value;
            refIndex.Index =  matchResult.Index;
            if (content.CurrentChar == '>')
            {
                yield return new Tag(content.Content, name, ArraySegment<IAttribute>.Empty, GetElements(name, CloneContent(content)));
                yield break;

            }   
            
            var beginPosition = refIndex.Index + (matchResult.Length) +content.From ; // {from}this is test code <div{beginPosition} name='4'>{index}
            IHtmlContent currentContent = content;
            if (beginPosition < content.Index)
                currentContent =
                    new HtmlContent(content.RootContent.Substring(beginPosition, content.Index - beginPosition));
            var attributes = new AttributeCollection(GetAttributes(currentContent)).ToArray();
            yield return new Tag(content.Content,name,attributes,GetElements(name,CloneContent(currentContent)));
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

            beginPosition = beginPosition < 0 ? 0 : beginPosition; // this function is used for getAttributes. so it can gets from -1 position
           // if(beginPosition < 0 ) throw new ParseErrorException($"could not seperate attributes from element;beginPosition:[{beginPosition}] content [{content.Content}]");
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