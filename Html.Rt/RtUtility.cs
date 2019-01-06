using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Html.Rt.Seperator;

namespace Html.Rt
{
    public static class RtUtility
    {
        private static string[] _inputs = new[] {"input", "textarea", "button", "select"};
        public static bool CanParse(this IHtmlSeperator seperator, string content)
        {
            var c = new HtmlContent(content);
            c.JumpLast();
            return seperator.CanParse(c);
        }

        public static bool CanParse(this IHtmlSeperator seperator,HtmlContent content)
        {
            return seperator.Parse(content).IsSuccess;
        }

        public static IEnumerable<IHtmlMarkup> Parse(this IHtmlSeperator seperator,string content)
        {
            var c = new HtmlContent(content);
            c.JumpLast();
            return seperator.Parse(c);
        }

        public static bool IsInput(IHtmlMarkup htmlMarkup)
        {
            if (!(htmlMarkup is ITag tag)) return false;
            return _inputs.Any(x => x.Equals(tag.Name, StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<ITag> GetInputs(this IEnumerable<IHtmlMarkup> source)
        {
            foreach (var element in source)
            {
                if (IsInput(element)) yield return (ITag) element;
            }
        }

        public static string ToHtml(this ITag tag)
        {
            var result = "<" + tag.Name;
            var attributes = tag.Attributes.ToArray();
            var attrStr = attributes.Length != 0
                ? attributes.Aggregate("",
                    (aggretor, current) =>
                    {
                        var value = string.Empty;
                        if (current.Quotes == default(char))
                            value = current.Value;
                        else value = current.Quotes + current.Value + current.Quotes;
                        return aggretor + " " + current.Key + ((!string.IsNullOrWhiteSpace(current.Value)
                                   ? ("=" +value)
                                   : ""));
                    })
                : "";
            return result + attrStr + ">";
        }

        public static string ToHtml(this EndTag endTag)
        {
            return "</" + endTag.Name + ">";
        }

        public static string GetText(this ITag source)
        {
            var result = string.Empty;
            foreach (var element in source.Elements)
            {
                if (element is Text text)
                    result += text.Markup;
            }
            return result;
        }

        public static string GetValue(this ITag tag)
        {
            var result = tag.Attributes.FirstOrDefault(x => x.Key.Equals("value", StringComparison.OrdinalIgnoreCase));
            if (result == null) throw new NullReferenceException($"value is not found in this  [{tag.ToHtml()}]");
            return result.Value;
        }

        public static string GetName(this ITag tag)
        {
            var result = tag.Attributes.FirstOrDefault(x => x.Key.Equals("name", StringComparison.OrdinalIgnoreCase));
            if (result == null) throw new NullReferenceException($"name is not found in this  [{tag.ToHtml()}]");
            return result.Value;
        }
        
        public static string GetNameOrDefault(this ITag tag)
        {
            return tag.Attributes.FirstOrDefault(x => x.Key.Equals("name", StringComparison.OrdinalIgnoreCase))
                ?.Value;
        }

        public static bool Is(this ITag source,string tagName)
        {
            return source.Name.Equals(tagName, StringComparison.OrdinalIgnoreCase);
        }

        public static bool Is(this EndTag source, string tagName)
        {
            return source.Name.Equals(tagName, StringComparison.OrdinalIgnoreCase);
        }

    

        public static ITag GetElementById(this IEnumerable<IHtmlMarkup> source,string id)
        {
            foreach (var element in source)
            {
                if(!(element is ITag tag)) continue;
                var idValue = tag.Attributes["id"];
                if (idValue == id) return tag;
            }
            return null;
        }

        public static string GetValueOrDefault(this ITag tag)
        {
            return tag.Attributes.FirstOrDefault(x => x.Key.Equals("value", StringComparison.OrdinalIgnoreCase))
                ?.Value;
        }

        public static IEnumerable<IHtmlMarkup> ParseFromOrigin(this IHtmlSeperator seperator, string content)
        {
            return seperator.Parse(new HtmlContent(content));
        }

        private  static IEnumerable<IHtmlMarkup> GetNodesCanForgotTag(string name,IEnumerator<IHtmlMarkup> enumerator)
        {
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;
                if (current is EndTag endTag)
                {
                    var isTag = endTag.Name.Equals(name, StringComparison.OrdinalIgnoreCase);
                    if(isTag) yield break;
                }else if (current is ITag tag)
                {
                    yield break;
                }
                else
                {
                    yield return current;
                }
            }
        }

        /*
        private bool OnlyChildren(ITag tag)
        {
            return new string[]{"body"};
        }

        private bool IgnoreInBody()
        {
            return new string[]{"head","html"};
        }

        private bool IsInFullTag()
        {
            return new string[]
            {
                "div", "table", "ul","ol","article","aside","header","footer","details","summary"
                
            };
        }

        private static bool IsEmptyTag(string name)
        {
            return new string[]{"br","hr"};
        }

        public static IEnumerable<IHtmlMarkup> ToCompact(this IEnumerable<IHtmlMarkup> source)
        {
            foreach (var item in source)
            {
                var tag = item as ITag;
                if (tag == null)
                {
                    yield return item;
                    continue;
                }
                var tagName = tag.Name;
                

            }
        }
        */
        
       
    }
}