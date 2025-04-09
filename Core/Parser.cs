using Core.Models;
using System.Collections.Generic;

namespace Core
{
    public class Parser
    {
        private readonly SymbolTable _symbolTable;
        private readonly ErrorHandler _errorHandler;
        private List<Token> _tokens;
        private int _currentPosition;

        public Parser(SymbolTable table, ErrorHandler errors)
        {
            _symbolTable = table;
            _errorHandler = errors;
        }

        public AstNode Parse(List<Token> tokens)
        {
            _tokens = tokens;
            _currentPosition = 0;
            var programBlock = new BlockNode();

            while (!IsAtEnd())
            {
                var statement = ParseStatement();
                if (statement != null)
                {
                    programBlock.Statements.Add(statement);
                }
            }

            CheckUnusedSymbols();
            return programBlock;
        }

        private AstNode ParseStatement()
        {
            try
            {
                if (Match("ENT") || Match("DEC") || Match("CADENA"))
                    return ParseVariableDeclaration();

                if (Check(Token.TokenCategory.Identifier) && PeekNext(1)?.Value == "=")
                    return ParseAssignment();

                Advance(); // Skip unrecognized tokens
                return null;
            }
            catch (ParseError error)
            {
                _errorHandler.AddError("Error Sintáctico", Current().Line, error.Message);
                Synchronize();
                return null;
            }
        }

        private VariableDeclarationNode ParseVariableDeclaration()
        {
            var typeToken = Current();
            Advance(); // Consume type (ENT/DEC/CADENA)

            var nameToken = Consume(Token.TokenCategory.Identifier, "Se esperaba nombre de variable");

            var node = new VariableDeclarationNode
            {
                Type = typeToken.Value,
                Name = nameToken.Value,
                Line = typeToken.Line,
                Column = typeToken.Column
            };

            if (Match("="))
            {
                node.Value = ParseExpression();
            }

            _symbolTable.AddSymbol(node.Name, node.Type);
            return node;
        }

        private AssignmentNode ParseAssignment()
        {
            var identifier = Consume(Token.TokenCategory.Identifier, "Se esperaba identificador");
            Consume("=", "Se esperaba '=' después de identificador");

            var node = new AssignmentNode
            {
                VariableName = identifier.Value,
                Value = ParseExpression(),
                Line = identifier.Line,
                Column = identifier.Column
            };

            _symbolTable.MarkAsUsed(node.VariableName);
            return node;
        }

        private AstNode ParseExpression()
        {
            // Versión simplificada - solo maneja literales e identificadores por ahora
            if (Check(Token.TokenCategory.Literal) || Check(Token.TokenCategory.Identifier))
            {
                var token = Current();
                Advance();
                return new LiteralNode { Value = token };
            }
            throw new ParseError($"Expresión no válida en línea {Current().Line}");
        }

        #region Helpers

        private bool Match(string expected)
        {
            if (!Check(expected)) return false;
            Advance();
            return true;
        }

        private Token Consume(string expected, string errorMessage)
        {
            if (Check(expected)) return Advance();
            throw new ParseError(errorMessage);
        }

        private Token Consume(Token.TokenCategory category, string errorMessage)
        {
            if (Check(category)) return Advance();
            throw new ParseError(errorMessage);
        }

        private bool Check(string expected) =>
            !IsAtEnd() && Current().Value == expected;

        private bool Check(Token.TokenCategory category) =>
            !IsAtEnd() && Current().Category == category;

        private Token Advance() =>
            _tokens[_currentPosition++];

        private bool IsAtEnd() =>
            _currentPosition >= _tokens.Count;

        private Token Current() =>
            _tokens[_currentPosition];

        private Token Previous() =>
            _tokens[_currentPosition - 1];

        private Token PeekNext(int lookahead = 1) =>
            (_currentPosition + lookahead) < _tokens.Count ? _tokens[_currentPosition + lookahead] : null;

        private void Synchronize()
        {
            Advance();
            while (!IsAtEnd())
            {
                if (Previous().Value == ";") return;
                switch (Current().Value)
                {
                    case "ENT":
                    case "DEC":
                    case "CADENA":
                        return;
                }
                Advance();
            }
        }

        private void CheckUnusedSymbols()
        {
            foreach (var symbol in _symbolTable.GetUnusedSymbols())
            {
                _errorHandler.AddWarning("Advertencia", symbol.Line,
                    $"Variable '{symbol.Name}' declarada pero no utilizada");
            }
        }

        #endregion
    }

    internal class ParseError : System.Exception
    {
        public int? Line { get; }

        public ParseError(string message) : base(message) { }

        public ParseError(string message, int? line) : base(message)
        {
            Line = line;
        }
    }
}