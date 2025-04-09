using System;
using System.Collections.Generic;

public class Lexer
{
    private int _position;
    private int _line = 1;
    private int _column = 1;
    private string _code;

    private readonly Dictionary<string, Token.TokenCategory> _keywordCategories = new()
    {
        { "FUNCTION", Token.TokenCategory.Keyword },
        { "START", Token.TokenCategory.Keyword },
        { "END", Token.TokenCategory.Keyword },
        { "ENT", Token.TokenCategory.Type },
        { "DEC", Token.TokenCategory.Type }
    };

    private readonly HashSet<char> _symbols = new() { '+', '-', '*', '/', '=', '(', ')', '{', '}', ';' };

    public List<Token> Tokenize(string code)
    {
        _code = code;
        _position = 0;
        _line = 1;
        _column = 1;

        var tokens = new List<Token>();

        while (_position < _code.Length)
        {
            char current = _code[_position];

            if (char.IsWhiteSpace(current))
            {
                HandleWhitespace(current);
                continue;
            }

            if (char.IsDigit(current))
            {
                tokens.Add(ReadNumber());
                continue;
            }

            if (_symbols.Contains(current))
            {
                tokens.Add(new Token
                {
                    Value = current.ToString(),
                    Category = Token.TokenCategory.Symbol,
                    Line = _line,
                    Column = _column
                });
                _position++;
                _column++;
                continue;
            }

            if (char.IsLetter(current))
            {
                tokens.Add(ReadWord());
                continue;
            }

            // Carácter no reconocido
            _position++;
            _column++;
        }

        return tokens;
    }

    private Token ReadNumber()
    {
        int start = _position;
        while (_position < _code.Length && char.IsDigit(_code[_position]))
        {
            _position++;
            _column++;
        }

        return new Token
        {
            Value = _code.Substring(start, _position - start),
            Category = Token.TokenCategory.Literal,
            Line = _line,
            Column = _column - (_position - start)
        };
    }

    private Token ReadWord()
    {
        int start = _position;
        while (_position < _code.Length && (char.IsLetterOrDigit(_code[_position]) || _code[_position] == '_'))
        {
            _position++;
            _column++;
        }

        string word = _code.Substring(start, _position - start);
        return new Token
        {
            Value = word,
            Category = DetermineCategory(word),
            Line = _line,
            Column = _column - (word.Length)
        };
    }

    private void HandleWhitespace(char c)
    {
        if (c == '\n')
        {
            _line++;
            _column = 1;
        }
        else
        {
            _column++;
        }
        _position++;
    }

    private Token.TokenCategory DetermineCategory(string value)
    {
        if (_keywordCategories.TryGetValue(value.ToUpper(), out var category))
            return category;

        return Token.TokenCategory.Identifier;
    }
}