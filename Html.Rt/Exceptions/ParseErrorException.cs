namespace Html.Rt.Exceptions
{
    public interface IParseErrorException 
    {
        
    }

    public class ParseErrorException : System.Exception, IParseErrorException
    {
        public ParseErrorException(string message, System.Exception innerException) : base(message, innerException)
        {
            
        }
        
        public ParseErrorException(string message) : base(message)
        {
            
        }
    }
}