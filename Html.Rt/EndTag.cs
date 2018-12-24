namespace Html.Rt
{
    public class EndTag :IHtmlMarkup
    {
        public string Name { get; }
        public EndTag(string name)
        {
            this.Name = name;
        }
    }
}