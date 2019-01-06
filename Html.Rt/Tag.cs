using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Html.Rt
{
    public class Tag : ITag
    {
        public string Name { get; }

        public string Markup { get; }

        public Attributes Attributes { get; }
        public IEnumerable<IHtmlMarkup> Elements { get; }


        public Tag(string markup, string name, IEnumerable<IAttribute> attributes,IEnumerable<IHtmlMarkup> elements)
        {
            this.Name = name;
            this.Attributes = new Attributes(attributes.ToArray());
            this.Markup = markup;
            this.Elements = elements;
        }

        public Tag(string markup, string name)
        {
            this.Markup = markup;
            this.Name = name;
            this.Attributes = new Attributes();
            this.Elements = ArraySegment<IHtmlMarkup>.Empty;
        }

        public override string ToString()
        {
            return $"[tag:{this.ToHtml()}]";
        }
    }
}
