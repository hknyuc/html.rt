using System;
using System.Collections.Generic;

namespace Html.Rt.Seperator
{
    public class CommentSeperator :IHtmlSeperator
    {
        public bool CanParse(HtmlContent content)
        {
            return (content.IndexOf("<!--") == 0);
        }

        public IEnumerable<IHtmlMarkup> Parse(HtmlContent content)
        {
            var indexOfStart = content.Content.IndexOf("<!--", StringComparison.Ordinal);
            if(indexOfStart < 0) yield break;
            var lastOfEnd = content.IndexOf("-->");
            if (lastOfEnd > 0)
                content.NextTo(lastOfEnd);
            yield return new Comment(content.Content);
        }
    }
}