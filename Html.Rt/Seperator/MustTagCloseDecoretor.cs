using System.Linq;
using System;
using System.Collections.Generic;
namespace Html.Rt.Seperator
{
     public class MustTagCloseDecorator : SeperatorDecorator
    {
        private string[] _tagNames;
        public MustTagCloseDecorator(string[] tagNames,IHtmlSeperator seperator) : base(seperator)
        {
            this._tagNames = tagNames;
        }

        public override ParseResult Parse(IHtmlContent content)
        {
            var result =  base.Parse(content);
            if (!result.IsSuccess) return result;
            return new ParseResult(Decorate(result.Result,content), result.From);
        }

        private bool IsTags(IHtmlElement tag)
        {
            return this._tagNames.Any(x => x.Equals(tag.Name, StringComparison.OrdinalIgnoreCase));
        }
        private IEnumerable<IHtmlMarkup> Decorate(IEnumerable<IHtmlMarkup> source,IHtmlContent content)
        {
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var markup = enumerator.Current;
                if (!(markup is IHtmlElement isTag))
                {
                    yield return markup;
                    continue;
                }

                var isScriptOrStyle = IsTags(isTag);
                if (!isScriptOrStyle)
                    continue;

                yield return markup;
                foreach (var item in FoundEndTag(isTag,enumerator,content))
                    yield return item;
                

            }
            enumerator.Dispose();
        }

        private static IEnumerable<IHtmlMarkup> FoundEndTag(IHtmlElement isTag,IEnumerator<IHtmlMarkup> enumerator,IHtmlContent htmlContent)
        {
            var position = htmlContent.Index;
            var lastPosition = position;
            while (enumerator.MoveNext())
            {
        
                var nextMarkup = enumerator.Current;
                var endTag = nextMarkup as EndTag;
                if (endTag == null)
                {
                    lastPosition = htmlContent.Index;
                    continue;
                }
                
                if (endTag.Name.Equals(isTag.Name, StringComparison.OrdinalIgnoreCase))
                {
                    if (position != lastPosition)
                        yield return new RawText(htmlContent.RootContent.Substring(position,lastPosition-position));
                    yield return endTag;
                    break;
                }
                lastPosition = htmlContent.Index;
            }

        }
    }
}