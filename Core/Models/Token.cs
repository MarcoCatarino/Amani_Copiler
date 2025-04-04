/// <summary>
/// Representa un token con su valor y ubicación en el código fuente.
/// </summary>
//public class Token
//{
//    public string Value { get; set; }       // Valor del token (ej: "if", "123")
//    public int Line { get; set; }           // Línea en el código (1-based)
//    public int Column { get; set; }         // Columna en el código (1-based)

//    public override string ToString() => $"{Value} (Línea {Line}, Columna {Column})";
//}

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
    public TokenCategory Category { get; set; }  // Nueva propiedad

    public override string ToString() => $"{Value} [{Category}] (Línea {Line}, Columna {Column})";
}