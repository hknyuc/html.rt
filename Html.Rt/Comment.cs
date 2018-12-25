namespace Html.Rt
{
    public class Comment :IHtmlMarkup
    {
        public string Markup { get; }
        public string Content { get; private set; }
        
        public Comment(string markup,string content)
        {
            this.Content = content;
            this.Markup = markup;
        }


    }
}