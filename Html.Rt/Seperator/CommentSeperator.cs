using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Html.Rt.Seperator
{
    public class CommentSeperator :IHtmlSeperator
    {
        public bool CanParse(HtmlContent content)
        {
            return (content.Content.IndexOf("<!--",StringComparison.Ordinal) >= 0);
        }

        public ParseResult Parse(HtmlContent content)
        {
            return new ParseResult(GetParsed(content), content.Index - 4);
        }

        private static IEnumerable<IHtmlMarkup> GetParsed(HtmlContent content)
        {
            var indexOfStart = content.Content.IndexOf("<!--", StringComparison.Ordinal);
            if(indexOfStart < 0) yield break;
            var lastOfEnd = content.NextIndexOf("-->");
            if (lastOfEnd > 0)
                content.NextTo(lastOfEnd);
            else content.JumpLast();
            yield return new Comment(content.Content,content.Content);
        }
    }
}