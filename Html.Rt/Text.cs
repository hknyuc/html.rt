namespace Html.Rt
{
    public class Text: IHtmlMarkup
    {
        public string Content { get; }
        public Text(string content)
        {
            this.Content = content;
        }
    }
}