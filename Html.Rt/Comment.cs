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

    public class CommentStart:IHtmlMarkup
    {
        public string Markup { get; }

        public CommentStart(string markup)
        {
            this.Markup = markup;
        }
    }

    public class CommentEnd : IHtmlMarkup
    {
        public string Markup { get; }
        public CommentEnd(string markup)
        {
            this.Markup = markup;
        }
    }
}