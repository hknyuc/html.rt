namespace Html.Rt
{
    public interface IAttribute : IHtmlMarkup
    {
        string Key { get; }
        string Value { get; }
        char Quotes { get; }
    }
}