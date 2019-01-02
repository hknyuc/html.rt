using System;
using System.Collections.Generic;
using System.Linq;

namespace Html.Rt.Seperator
{
    public class StyleOrScriptSeperator :IHtmlSeperator
    {
        private readonly IHtmlSeperator _elementSeperator = new ElementSeperator();
        public ParseResult Parse(HtmlContent content)
        {
            var result = _elementSeperator.Parse(content);
            if (!result.IsSuccess) return result;
            var first = result.First();
            var tagElement = first as IHtmlElement;
            if (tagElement == null || !IsSyleOrScript(tagElement)) return new ParseResult(new [] {first}, result.From);
            return new ParseResult(GetResult(tagElement, result.Result, content), content.From);
        }

        private static bool IsSyleOrScript(IHtmlElement element)
        {
            return new string[] {"script", "style"}.Any(x =>
                x.Equals(element.Name, StringComparison.OrdinalIgnoreCase));
        }

        private IEnumerable<IHtmlMarkup> GetResult(IHtmlElement mainTag, IEnumerable<IHtmlMarkup> source,HtmlContent content)
        {
            yield return mainTag;
            foreach (var item in GetNextElement(mainTag.Name,source,content))
                yield return item;
        }

        private IEnumerable<IHtmlMarkup> GetNextElement(string tagName,IEnumerable<IHtmlMarkup> source, HtmlContent content)
        {
            var position = content.Index;
            foreach (var element in source)
            {
                var isEndTag = element as EndTag;
                var lastPosition = content.Index;
                if (isEndTag == null)
                {
                    continue;
                }

                if (isEndTag.Name.Equals(tagName, StringComparison.OrdinalIgnoreCase))
                {
                    yield return new Text(content.Content.Substring(position, lastPosition - position));
                }
                yield return isEndTag;
            }
        }
        
    }
}