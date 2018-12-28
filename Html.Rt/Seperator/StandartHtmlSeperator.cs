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

        public ParseResult Parse(HtmlContent content)
        {
            return new ParseResult(GetResult(content), content.StartIndex);
        }

        private IEnumerable<IHtmlMarkup> GetResult(HtmlContent content)
        {
            var textContent = new TextContent();
            while (content.Next())
            {
                var seperator = GetSeperator(content);
                if (seperator == null)
                    textContent.SetContent(content.Content);
                else
                {
                    var result = seperator.Parse(content);
                    //get from html.parse();
                    if (result.From != 0)
                    {
                        textContent.SetContent(content.Content.Substring(0, result.From));
                        if (!textContent.IsEmpty) yield return new Text(textContent.Markup);
                    }

                    textContent.Reset();
                    
                    foreach (var @item in result)
                        yield return @item;
                    
                    content.Outstrip();
                }
            }
            if (!textContent.IsEmpty)
                yield  return new Text(textContent.Markup);
        }

   
    }
}