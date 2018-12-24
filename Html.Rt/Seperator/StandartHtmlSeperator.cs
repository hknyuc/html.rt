using System;
using System.Collections.Generic;
using System.Linq;

namespace Html.Rt.Seperator
{
    public class StandartHtmlSeperator :IHtmlSeperator
    {
        private IList<IHtmlSeperator> _seperators;
        public StandartHtmlSeperator()
        {
            this._seperators = new List<IHtmlSeperator>();
            this._seperators.Add(new AttributeSeperator());
            this._seperators.Add(new ElementSeperator());
            this._seperators.Add(new TextSeperator());
            this._seperators.Add(new CommentSeperator());
        }
        public bool CanParse(HtmlContent content)
        {
            return this._seperators.Any(x => x.CanParse(content));
        }

        public IEnumerable<IHtmlMarkup> Parse(HtmlContent content)
        {
            throw new Exception();
        }
    }
}