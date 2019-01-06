namespace Html.Rt
{
    public class Text: IHtmlMarkup
    {
        public string Markup { get; protected set; }
        public Text(string content)
        {
            this.Markup = content;
        }

        public override string ToString()
        {
            return $"[text :{this.Markup}]";
        }
    }

    public class TextContent : Text
    {
        public bool IsEmpty
        {
            get { return string.IsNullOrWhiteSpace(this.Markup); }
        }

        public TextContent(string content) : base(content)
        {
        }

        public TextContent():base(string.Empty)
        {
            
        }

        public void Reset()
        {
            this.Markup = string.Empty;
        }

        public void SetContent(string content)
        {
            this.Markup = content;
        }
    }
}