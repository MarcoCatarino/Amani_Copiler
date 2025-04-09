public class Token
{
    public enum TokenCategory
    {
        Keyword,        // Palabras reservadas (If, While, etc.)
        Type,           // ENT, DEC, LYRIC
        Modifier,       // const
        Identifier,     // Nombres de variables/funciones
        Literal,        // Números, strings
        Operator,       // +, -, =, etc.
        Symbol          // {}, ;, ()
    }

    public string Value { get; set; }
    public int Line { get; set; }
    public int Column { get; set; }
    public TokenCategory Category { get; set; }

    public override string ToString() => $"{Value} [{Category}] (Línea {Line}, Columna {Column})";
}