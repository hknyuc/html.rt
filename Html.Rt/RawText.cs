namespace Html.Rt
{
    public class RawText : IHtmlMarkup
    {
        public string Markup { get; }
        public RawText(string content)
        {
            this.Markup = content;
        }

    }
}