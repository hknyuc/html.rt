using System.Collections.Generic;

namespace Html.Rt.Seperator
{
    public interface IHtmlSeperator
    {
        bool CanParse(HtmlContent content);
        IEnumerable<IHtmlMarkup> Parse(HtmlContent content);
    }
}