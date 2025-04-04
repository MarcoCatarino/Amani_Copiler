using System.Linq;
using Core;  // Si 'SymbolTable' está en Core


/// <summary>
/// Aplica optimizaciones al código tokenizado
/// </summary>
public class Optimizer
{
    private readonly SymbolTable _symbolTable;

    public Optimizer(SymbolTable symbolTable)
    {
        _symbolTable = symbolTable;
    }

    /// <summary>
    /// Aplica todas las optimizaciones disponibles
    /// </summary>
    public List<Token> Optimize(List<Token> tokens)
    {
        var optimized = ConstantFolding(tokens);
        optimized = DeadCodeElimination(optimized);
        optimized = RemoveUnusedVariables(optimized);
        return optimized;
    }

    private List<Token> ConstantFolding(List<Token> tokens)
    {
        var result = new List<Token>();
        for (int i = 0; i < tokens.Count; i++)
        {
            if (i + 2 < tokens.Count &&
                IsNumber(tokens[i].Value) &&
                IsOperator(tokens[i + 1].Value) &&
                IsNumber(tokens[i + 2].Value))
            {
                int val1 = int.Parse(tokens[i].Value);
                int val2 = int.Parse(tokens[i + 2].Value);
                int folded = tokens[i + 1].Value switch
                {
                    "+" => val1 + val2,
                    "-" => val1 - val2,
                    "*" => val1 * val2,
                    "/" => val1 / val2,
                    _ => 0
                };

                result.Add(new Token
                {
                    Value = folded.ToString(),
                    Line = tokens[i].Line,
                    Column = tokens[i].Column
                });
                i += 2;
            }
            else
            {
                result.Add(tokens[i]);
            }
        }
        return result;
    }

    private List<Token> DeadCodeElimination(List<Token> tokens)
    {
        var result = new List<Token>();
        bool inDeadCode = false;
        int braceLevel = 0;

        for (int i = 0; i < tokens.Count; i++)
        {
            if ((tokens[i].Value == "If" || tokens[i].Value == "While") &&
                i + 2 < tokens.Count &&
                tokens[i + 1].Value == "(" &&
                tokens[i + 2].Value == "false")
            {
                inDeadCode = true;
                i += 2;
                continue;
            }

            if (tokens[i].Value == "{") braceLevel++;
            if (tokens[i].Value == "}")
            {
                braceLevel--;
                if (braceLevel == 0) inDeadCode = false;
            }

            if (!inDeadCode) result.Add(tokens[i]);
        }
        return result;
    }

    private List<Token> RemoveUnusedVariables(List<Token> tokens)
    {
        var unusedVars = _symbolTable.GetUnusedVariables().Select(symbol => symbol.Name).ToList();
        var result = new List<Token>();

        for (int i = 0; i < tokens.Count; i++)
        {
            if ((tokens[i].Value == "ENT" || tokens[i].Value == "DEC") &&
                i + 1 < tokens.Count &&
                unusedVars.Contains(tokens[i + 1].Value))
            {
                i += 3; // Saltar declaración completa
                continue;
            }
            result.Add(tokens[i]);
        }
        return result;
    }

    private bool IsNumber(string value) => int.TryParse(value, out _);
    private bool IsOperator(string value) => new[] { "+", "-", "*", "/" }.Contains(value);
}