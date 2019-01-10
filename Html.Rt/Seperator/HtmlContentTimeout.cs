namespace Html.Rt.Seperator
{
    public class HtmlContentTimeout : HtmlContentDecorator
    {
        private int _timesCount;
        public HtmlContentTimeout(IHtmlContent content,int timesCount) : base(content)
        {
            this._timesCount = timesCount;
        }


        public override bool Next()
        {
            var result = base.Next();
            return result;
        }
    }
}