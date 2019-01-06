namespace Html.Rt
{
    public class EndTag :IHtmlMarkup
    {
        public string Name { get; }
        public string Markup { get; }
        public EndTag(string markup,string name)
        {
            this.Name = name;
            this.Markup = markup;
        }

        public override string ToString()
        {
            return $"[endTag :{this.ToHtml()}]";
        }
    }
}