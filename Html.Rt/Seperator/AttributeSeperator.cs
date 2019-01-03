using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using System.Threading;

namespace Html.Rt.Seperator
{
    public class AttributeSeperator :IHtmlSeperator
    {
        private readonly Regex _regex =
            new Regex(@"\s*([a-zA-Z][\w:\-]*)\s*(?:\s*=(\s*""(?:\\""|[^""])*""|\s*'(?:\\'|[^'])*'|[^\s>]+))?");
        private bool CanParse(IHtmlContent content)
        {
            return this._regex.IsMatch(content.Content);
        }

        //("|')((?:\\\1|(?:(?!\1).))*)\1
        public ParseResult Parse(IHtmlContent content)
        {
            var canParse = this.CanParse(content);
            if(!canParse)
                return new ParseResult();
            return new ParseResult(GetParsed(content.Content), 0);
        }

        private IEnumerable<IHtmlMarkup> GetParsed(string content)
        {
            var result = this._regex.Matches(content);
            foreach (Match match in result)
            {
                if(!match.Success) continue;
                var groups = match.Groups.ToArray();
                yield return new AttributeElement(content,groups[1].Value,PruneString(groups[2]?.Value));

            }
        }

        private static string PruneString(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            return value.Substring(1, value.Length - 2);
        }

    }


    /*
    public class AttributeSeperator2 : IHtmlSeperator
    {
        private enum Status
        {
            WaitKey=1,
            WaitValue=2,
            WaitEq=3,
            WaitBeginQuotes=4,
            WaitEndQuotes=5
        } 
        public bool CanParse(HtmlContent content)
        {
            return true;
        }

        public ParseResult Parse(HtmlContent content)
        {
            var attributeRaw = content.Content;
            for (var i = 0; i < attributeRaw.Length; i++)
            {
               
            }
        }

        private IEnumerable<IHtmlMarkup> GetParsed(string content)
        {
            var state = Status.WaitKey;
            var key = string.Empty;
            var value = string.Empty;
            char eq;
            var eqs = new char[]{'\'', '"'};
            var empty = ' ';
            for (var i = 0; i < content.Length; i++)
            {
                var ch = content[i];
                if (state == Status.WaitKey)
                    key += ch;
                if (state == Status.WaitValue)
                    value += ch;
                if (state == Status.WaitBeginQuotes)
                {
                    if (ch == empty) continue;
                    if (eqs.Contains(ch))
                        eq = ch;
                }

                if (state == Status.WaitEndQuotes)
                {
                    if(eqs.Contains())
                }
            }
        }
    }
    */
    
    public class AttributeElement : IAttribute
    {
        public string Key { get; }
        public string Value { get; }
        public string Markup { get; }

        public AttributeElement(string markup,string key, string value)
        {
            this.Key = key;
            this.Value = value;
            this.Markup = markup;
        }

   
    }

    public class AttributeCollection : IEnumerable<IAttribute>
    {

        private string _content;
        private IHtmlSeperator _seperator = new AttributeSeperator();
        public AttributeCollection(string content)
        {
            this._content = content;
            if (!this._seperator.CanParse(content))
                this._seperator = new EmptySeperator();
        }
        
        public IEnumerator<IAttribute> GetEnumerator()
        {
            return this._seperator.Parse(this._content).OfType<IAttribute>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}