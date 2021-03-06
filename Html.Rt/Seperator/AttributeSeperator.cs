using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Html.Rt.Seperator
{
    public class AttributeSeperatorByRegex :IHtmlSeperator
    {
        private readonly Regex _regex =
            new Regex(@"\s*([a-zA-Z][\w:\-]*)\s*(?:\s*=(\s*""(?:\\""|[^""])*""|\s*'(?:\\'|[^'])*'|[^\s>]+))?");
        private bool CanParse(IHtmlContent content)
        {
            return this._regex.IsMatch(content.Content.ToString());
        }

        //("|')((?:\\\1|(?:(?!\1).))*)\1
        public ParseResult Parse(IHtmlContent content)
        {
            var canParse = this.CanParse(content);
            if(!canParse)
                return new ParseResult();
            return new ParseResult(GetParsed(content.Content.ToString()), 0);
        }

        private IEnumerable<IHtmlMarkup> GetParsed(string content)
        {
            var result = this._regex.Matches(content);
            foreach (Match match in result)
            {
                if(!match.Success) continue;
                var groups = match.Groups;
                yield return new AttributeElement(content,groups[1].Value,PruneString(groups[2]?.Value),default(char));

            }
        }

        private static string PruneString(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            return value.Substring(1, value.Length - 2);
        }

    }


    
    public class AttributeSeperator2 : IHtmlSeperator
    {
        private static char[] _qs = new char[] {'\'', '"'};
        private enum Status
        {
            InKey=1,
            WaitValue=2,
            InBeginQuotes=3,
            InPureValue=4
    
        }

        public ParseResult Parse(IHtmlContent content)
        {
            var startPosition = content.Index;
            return new ParseResult(GetAttributes(content), startPosition);
        }

        private static bool InQuotes(char ch)
        {
            return _qs.Contains(ch);
        } 

        private static IEnumerable<IHtmlMarkup> GetAttributes(IHtmlContent content)
        {
            var status = Status.InKey;
            var key = new StringBuilder();
            var value = new StringBuilder();
            var quotes = default(char);
            var allStringBuilder = new StringBuilder();

            void Clear()
            {
                key.Clear();
                allStringBuilder.Clear();
                value.Clear();
                quotes = default(char);
            }

            while (content.Next())
            {
                allStringBuilder.Append(content.CurrentChar);
                if(status == Status.InKey && key.Length == 0 && char.IsWhiteSpace(content.CurrentChar)) continue;
                if (status == Status.InKey)
                {
                    if (char.IsWhiteSpace(content.CurrentChar) && key.Length != 0)
                    {
                
                        yield return new AttributeElement(allStringBuilder.ToString(), key.ToString(), value.ToString(),quotes);
                        Clear();
                        continue;
                    }
                  
                    if (content.CurrentChar == '=')
                    {
                        status = Status.WaitValue;
                        continue;
                    }
                    key.Append(content.CurrentChar);
                   
                }
                else if (status == Status.WaitValue)
                {
                    if (InQuotes(content.CurrentChar) && content.BeforeChar != '\\')
                    {
                        status = Status.InBeginQuotes;
                        quotes = content.CurrentChar;
                    }else if (!char.IsWhiteSpace(content.CurrentChar) && !InQuotes(content.CurrentChar))
                    {
                        quotes = default(char);
                        status = Status.InPureValue;
                        value.Append(content.CurrentChar);
                    }
                }
                else if (status == Status.InBeginQuotes)
                {
                    if (content.CurrentChar == quotes && content.BeforeChar != '\\')
                    {
                        yield return new AttributeElement(allStringBuilder.ToString(), key.ToString(), value.ToString(),quotes);
                        status = Status.InKey;
                        Clear();
                    }
                    else
                    {
                        value.Append(content.CurrentChar);
                    }
                }else if (status == Status.InPureValue)
                {
                    if (char.IsWhiteSpace(content.CurrentChar))
                    {
                       
                        status = Status.InKey;
                        yield return new AttributeElement(allStringBuilder.ToString(), key.ToString(), value.ToString(), quotes);
                        Clear();
                    }
                    else
                    {
                        value.Append(content.CurrentChar);
                    }
                }
            }

            if (key.Length != 0)
                yield return new AttributeElement(allStringBuilder.ToString(), key.ToString(), value.ToString(), quotes);
        } 
    }    
    
    
    public class AttributeElement : IAttribute
    {
        public string Key { get; }
        public string Value { get; }
        public char Quotes { get; }
        public string Markup { get; }

        public AttributeElement(string markup,string key, string value,char quotes)
        {
            this.Key = key;
            this.Value = value;
            this.Markup = markup;
            this.Quotes = quotes;
        }

   
    }

    public class AttributeCollection : IEnumerable<IAttribute>
    {

        private HtmlContent _content;
        private IHtmlSeperator _seperator = new SeperatorIterator(new AttributeSeperator2());
        public AttributeCollection(string content)
        {
            this._content = new HtmlContent(content);
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