/// <summary>
/// Convierte código fuente en una lista de tokens.
/// </summary>

//public class Lexer
//{
//    /// <summary>
//    /// Tokeniza el código y filtra espacios/comentarios.
//    /// </summary>
//    public List<Token> Tokenize(string code)
//    {
//        List<Token> tokens = new List<Token>();
//        int line = 1, column = 1;

//        string[] rawTokens = Regex.Split(code, @"(\s+|\b|\W)");

//        foreach (string rawToken in rawTokens)
//        {
//            if (string.IsNullOrWhiteSpace(rawToken))
//            {
//                UpdatePosition(rawToken, ref line, ref column);
//                continue;
//            }

//            tokens.Add(new Token
//            {
//                Value = rawToken.Trim(),
//                Line = line,
//                Column = column
//            });

//            column += rawToken.Length;
//        }

//        return tokens;
//    }

//    private void UpdatePosition(string text, ref int line, ref int column)
//    {
//        foreach (char c in text)
//        {
//            if (c == '\n') { line++; column = 1; }
//            else column++;
//        }
//    }
//}

//public class Lexer
//{
//    private readonly Dictionary<string, Token.TokenCategory> _keywordCategories = new()
//    {
//        // Tipos
//        { "ENT", Token.TokenCategory.Type },
//        { "DEC", Token.TokenCategory.Type },
//        { "LYRIC", Token.TokenCategory.Type },
//        { "BOL", Token.TokenCategory.Type },

//        // Modificadores
//        { "const", Token.TokenCategory.Modifier },

//        // Palabras clave
//        { "START", Token.TokenCategory.Keyword },
//        { "END", Token.TokenCategory.Keyword },
//        // ... agregar otras keywords
//    };

//    private readonly HashSet<char> _operators = new() { '+', '-', '*', '/', '=', '!', '>', '<', '&', '|' };
//    private readonly HashSet<char> _symbols = new() { '{', '}', '(', ')', ';', ',' };

//    public List<Token> Tokenize(string code)
//    {
//        List<Token> tokens = new();
//        int line = 1, column = 1;

//        string[] rawTokens = Regex.Split(code, @"(\s+|\b|\W)");

//        foreach (string rawToken in rawTokens.Where(t => !string.IsNullOrWhiteSpace(t)))
//        {
//            string tokenValue = rawToken.Trim();
//            var token = new Token
//            {
//                Value = tokenValue,
//                Line = line,
//                Column = column,
//                Category = DetermineCategory(tokenValue)
//            };

//            tokens.Add(token);
//            column += rawToken.Length;
//        }
//        return tokens;
//    }

//    private Token.TokenCategory DetermineCategory(string value)
//    {
//        if (_keywordCategories.TryGetValue(value, out var category))
//            return category;

//        if (value.Length == 1 && _operators.Contains(value[0]))
//            return Token.TokenCategory.Operator;

//        if (value.Length == 1 && _symbols.Contains(value[0]))
//            return Token.TokenCategory.Symbol;

//        if (Regex.IsMatch(value, @"^\d"))
//            return Token.TokenCategory.Literal;

//        return Token.TokenCategory.Identifier;
//    }

//    private void UpdatePosition(string text, ref int line, ref int column)
//    {
//        foreach (char c in text)
//        {
//            if (c == '\n') { line++; column = 1; }
//            else column++;
//        }
//    }
//}

using System.Text.RegularExpressions;

public class Lexer
{
    private readonly Dictionary<string, Token.TokenCategory> _keywordCategories = new()
    {
        { "FUNCTION", Token.TokenCategory.Keyword },
        { "START", Token.TokenCategory.Keyword },
        { "END", Token.TokenCategory.Keyword },
        { "ENT", Token.TokenCategory.Type },
        { "DEC", Token.TokenCategory.Type }
    };

    //public List<Token> Tokenize(string code)
    //{
    //    List<Token> tokens = new();
    //    int line = 1, column = 1;

    //    string[] rawTokens = Regex.Split(code, @"(\s+|\b|\W)");

    //    foreach (string rawToken in rawTokens.Where(t => !string.IsNullOrWhiteSpace(t)))
    //    {
    //        string normalizedValue = rawToken.Trim().ToUpper();
    //        var token = new Token
    //        {
    //            Value = rawToken.Trim(),
    //            Line = line,
    //            Column = column,
    //            Category = _keywordCategories.TryGetValue(normalizedValue, out var category)
    //                      ? category
    //                      : DetermineCategory(rawToken)
    //        };

    //        tokens.Add(token);
    //        UpdatePosition(rawToken, ref line, ref column);
    //    }
    //    return tokens;
    //}

    public List<Token> Tokenize(string code)
    {
        List<Token> tokens = new List<Token>();
        int line = 1, column = 1;

        string[] rawTokens = Regex.Split(code, @"(\s+|\b|\W)");

        foreach (string rawToken in rawTokens.Where(t => !string.IsNullOrWhiteSpace(t)))
        {
            string tokenValue = rawToken.Trim();
            var token = new Token
            {
                Value = tokenValue,
                Line = line,
                Column = column,  // Asegura que la columna sea exacta
                Category = DetermineCategory(tokenValue)
            };

            tokens.Add(token);
            UpdatePosition(rawToken, ref line, ref column);  // Actualiza línea/columna
        }
        return tokens;
    }

    private void UpdatePosition(string text, ref int line, ref int column)
    {
        foreach (char c in text)
        {
            if (c == '\n') { line++; column = 1; }
            else column++;
        }
    }
    private Token.TokenCategory DetermineCategory(string value)
    {
        if (Regex.IsMatch(value, @"^\d")) return Token.TokenCategory.Literal;
        if (value.Length == 1 && "+-*/=(){};".Contains(value))
            return Token.TokenCategory.Symbol;
        return Token.TokenCategory.Identifier;
    }

    //private void UpdatePosition(string text, ref int line, ref int column)
    //{
    //    foreach (char c in text)
    //    {
    //        if (c == '\n') { line++; column = 1; }
    //        else column++;
    //    }
    //}
}