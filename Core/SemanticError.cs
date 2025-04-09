namespace Core
{
    public class SemanticError : System.Exception
    {
        public int? Line { get; }
        public SemanticError(string message, int? line = null) : base(message)
        {
            Line = line;
        }
    }
}