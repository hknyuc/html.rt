using System.Collections.Generic;

namespace Html.Rt
{
    public interface IHtmlElement :IHtmlMarkup,IEnumerable<IHtmlMarkup>
    {
        string Name { get; }
        IEnumerable<IAttribute> Attributes { get; }
    }
}