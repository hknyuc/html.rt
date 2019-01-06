using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace Html.Rt.Seperator
{
    public class HtmlCodeSeperator :IHtmlSeperator
    {
        private readonly IHtmlSeperator _elementSeperator;

        public HtmlCodeSeperator(IHtmlSeperator mainSeperator)
        {
            this._elementSeperator = new ElementSeperator(mainSeperator);
        }

        public HtmlCodeSeperator()
        {
            this._elementSeperator = new ElementSeperator();
        }
        public ParseResult Parse(IHtmlContent content)
        {
            var result = _elementSeperator.Parse(content);
            if (!result.IsSuccess) return result;
            var first = result.First();
            var tagElement = first as ITag;
            if (tagElement == null || !IsSyleOrScript(tagElement)) return new ParseResult(new [] {first}, result.From);
            return new ParseResult(GetResult(tagElement, content), result.From);
        }

        private static bool IsSyleOrScript(ITag element)
        {
            return new string[] {"script", "style"}.Any(x =>
                x.Equals(element.Name, StringComparison.OrdinalIgnoreCase));
        }

        private IEnumerable<IHtmlMarkup> GetResult(ITag mainTag, IHtmlContent content)
        {
            yield return mainTag;
            content.Outstrip();
            var escapedStringHtml = new EscapeStringHtmlContent(content);
            var position = content.Index;
            bool isFounded = false;
            while (escapedStringHtml.Next())
            {
                var result = this._elementSeperator.Parse(content);
                if(!result.IsSuccess) continue;
                var first = result.Result.First();
                if (first is EndTag endTag)
                {
                    var c = escapedStringHtml.Content;
                    var isMainEndTag = endTag.Name.Equals(mainTag.Name, StringComparison.OrdinalIgnoreCase);
                    if (!isMainEndTag) continue;
                    if(escapedStringHtml.IsQuotes(result.From+content.From)) continue;
                    isFounded = true;
                    yield return new RawText(content.RootContent.Substring(position, result.From));
                    yield return endTag;
                    break;
                }
            }

            if (!isFounded)
                yield return new RawText(escapedStringHtml.Content);
        }

    }
}