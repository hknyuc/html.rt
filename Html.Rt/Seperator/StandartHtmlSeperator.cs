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
            this._seperators.Add(new CommentSeperator());
            this._seperators.Add(new ElementSeperator());
            //this._seperators.Add(new AttributeSeperator());
            // this._seperators.Add(new TextSeperator());
        }
        public bool CanParse(HtmlContent content)
        {
            return true;
            return this._seperators.Any(x => x.CanParse(content));
        }

        private IHtmlSeperator GetSeperator(HtmlContent content)
        {
            return this._seperators.FirstOrDefault(x => x.CanParse(content));
        }

        public IEnumerable<IHtmlMarkup> Parse(HtmlContent content)
        {
            var textContent = new TextContent();
            while (content.Next())
            {
                var seperator = GetSeperator(content);
                if (seperator == null)
                    textContent.SetContent(content.Content);
                else
                {
                    textContent.Reset();
                    foreach (var @item in seperator.Parse(content))
                    {
                        yield return @item;
                    }
                    //content.Outstrip();
                }
            }
            if (!textContent.IsEmpty)
              yield  return new Text(textContent.Markup);
        }

        public IEnumerable<IHtmlMarkup> Parse(string content)
        {
            return this.Parse(new HtmlContent(content));
        }
    }
}