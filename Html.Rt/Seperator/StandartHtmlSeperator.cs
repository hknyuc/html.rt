using System.Collections.Generic;


namespace Html.Rt.Seperator
{
    public class PartialStandartHtmlSeperator :IHtmlSeperator
    {
        private IList<IHtmlSeperator> _seperators;
        public PartialStandartHtmlSeperator()
        {
            this._seperators = new List<IHtmlSeperator>();
            this._seperators.Add(new CommentSeperator());
            this._seperators.Add(new HtmlCodeSeperator());
            this._seperators.Add(new ElementSeperator());
            //this._seperators.Add(new AttributeSeperator());
            // this._seperators.Add(new TextSeperator());
        }

        private  ParseResult GetSeperator(IHtmlContent content)
        {
            ParseResult parseResult;
            foreach (var seperator in _seperators)
            {
                 parseResult = seperator.Parse(content);
                if (parseResult.IsSuccess) return parseResult;
            }

            return null;
        }

        public ParseResult Parse(IHtmlContent content)
        {
            return new ParseResult(GetResult(content), content.StartIndex);
        }

        private IEnumerable<IHtmlMarkup> GetResult(IHtmlContent content)
        {
            var textContent = new TextContent();
            while (content.Next())
            {
                var seperator = GetSeperator(content);
                if (seperator == null)
                    textContent.SetContent(content.Content);
                else
                {
                    var result = seperator;
            
                    //get from html.parse();
                    if (result.From != 0)
                    {
                        textContent.SetContent(content.Content.Substring(1,result.From-1));
                        if (!textContent.IsEmpty)
                        {
                            content.Outstrip();
                            yield return new Text(textContent.Markup);
                        }
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

  

    public class StandartHtmlSeperator : IHtmlSeperator
    {
        private IHtmlSeperator _partialStandartHtmlSeperator =  new PartialStandartHtmlSeperator();

        public ParseResult Parse(IHtmlContent content)
        {
            return this._partialStandartHtmlSeperator.Parse(content);
        }

    }
}