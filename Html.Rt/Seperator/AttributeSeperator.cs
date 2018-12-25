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
            new Regex(@"\s*([a-zA-Z][\w:\-]*)(?:\s*=(\s*""(?:\\""|[^""])*""|\s*'(?:\\'|[^'])*'|[^\s>]+))?");
        public bool CanParse(HtmlContent content)
        {
            return this._regex.IsMatch(content.Content);
        }

        //("|')((?:\\\1|(?:(?!\1).))*)\1
        public IEnumerable<IHtmlMarkup> Parse(HtmlContent content)
        {
            var result = this._regex.Matches(content.Content);
            foreach (Match match in result)
            {
                if(!match.Success) continue;
                var groups = match.Groups.ToArray();
                yield return new AttributeElement(content.Content,groups[1].Value,PruneString(groups[2]?.Value));

            }
        }

        private static string PruneString(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            return value.Substring(1, value.Length - 2);
        }

    }
    
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