using System.Collections.Generic;

namespace Html.Rt
{
    public interface ITag :IHtmlMarkup
    {
        string Name { get; }
        Attributes Attributes { get; }
        IEnumerable<IHtmlMarkup> Elements { get; }
    }
}