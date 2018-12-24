namespace Html.Rt
{
    public class Comment :IHtmlMarkup
    {
        public string Content { get; private set; }
        
        public Comment(string content)
        {
            this.Content = content;
        }
    }
}