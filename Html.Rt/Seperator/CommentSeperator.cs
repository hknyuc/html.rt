using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Html.Rt.Seperator
{
    public class CommentSeperator :IHtmlSeperator
    {
        private int CanParseIndex(IHtmlContent content)
        {
            return (content.Content.ToString().IndexOf("<!--",StringComparison.Ordinal));
        }

        public ParseResult Parse(IHtmlContent content)
        {
            var canParseIndex = this.CanParseIndex(content);
            if (canParseIndex < 0) return new ParseResult();
            return new ParseResult(GetParsed(content), canParseIndex);
        }

        private static IEnumerable<IHtmlMarkup> GetParsed(IHtmlContent content)
        {
            var indexOfStart = content.Content.ToString().IndexOf("<!--", StringComparison.Ordinal);
            if(indexOfStart < 0) yield break;
            var lastOfEnd = content.NextIndexOf("-->");
            if (lastOfEnd > 0)
                content.NextTo(lastOfEnd);
            else content.JumpLast();
            var text = content.Content.ToString().Substring(4, content.Content.Length - 3-4);
            yield return new Comment(content.Content.ToString(), text);
        }
    }
}